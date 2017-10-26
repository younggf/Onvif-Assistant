﻿
using OnvifIPClib.ServiceReferencePtz;
using System;
using System.IO;

namespace SDS.Video.Onvif
{
    class OnvifPtz
    {
        private string User;
        private string Password;
        private PTZClient PtzClient;
        private OnvifIPClib.ServiceReferenceMedia.MediaClient MediaClient;
        private OnvifIPClib.ServiceReferenceMedia.Profile MediaProfile { get; set; }
        public bool PtzAvailable;

        //public OnvifPtz(string ip, int port, string user, string password)
        //{
        //    System.Net.IPAddress.TryParse(ip, out IP);
        //    Port = port;
        //    User = user;
        //    Password = password;

        //    PtzClient = OnvifServices.GetOnvifPTZClient(IP.ToString(), Port, User, Password);
        //    MediaClient = OnvifServices.GetOnvifMediaClient(IP.ToString(), Port, User, Password);
        //}

        public OnvifPtz(string mediaUri, string ptzUri, double deviceTimeOffset, string user, string password)
        {
            User = user;
            Password = password;

            if (string.IsNullOrEmpty(mediaUri) | string.IsNullOrEmpty(ptzUri))
                throw new Exception("Media and/or PTZ URI is empty or null.  PTZ object cannot be created");

            PtzClient = OnvifServices.GetOnvifPTZClient(ptzUri, deviceTimeOffset, User, Password);
            MediaClient = OnvifServices.GetOnvifMediaClient(mediaUri, deviceTimeOffset, User, Password);
        }

        /// <summary>
        /// Gets the first media profile that contains a PTZConfiguration from the the MediaClient GetProfiles command
        /// </summary>
        /// <returns>Media profile with PTZConfiguration</returns>
        private OnvifIPClib.ServiceReferenceMedia.Profile GetMediaProfile()
        {
            if (MediaProfile != null)
            {
                return MediaProfile;
            }
            else
            {
                //log.Warn(string.Format("PTZ Media profile not assigned.  Finding first available PTZ-enabled profile - THIS MAY CAUSE ISSUES (commands sent to wrong stream) AND NEEDS TO BE CHANGED"));
                // If no profile defined, take a guess and select the first available one - THIS NEEDS TO GO AWAY EVENTUALLY
                OnvifIPClib.ServiceReferenceMedia.Profile[] mediaProfiles = MediaClient.GetProfiles();

                foreach (OnvifIPClib.ServiceReferenceMedia.Profile p in mediaProfiles)
                {
                    if (p.PTZConfiguration != null)
                    {
                        // This should eliminate the redundant GetProfiles() / GetProfile() calls that were being done on every command
                        MediaProfile = MediaClient.GetProfile(p.token);
                        return MediaProfile;
                    }
                }
            }

            throw new Exception("No media profiles containing a PTZConfiguration on this device");
        }

        /// <summary>
        /// Pan the camera (uses the first media profile that is PTZ capable)
        /// </summary>
        /// <param name="speed">Percent of max speed to move the camera (1-100)</param>
        public void Pan(float speed)
        {
            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
            PTZConfigurationOptions ptzConfigurationOptions = PtzClient.GetConfigurationOptions(mediaProfile.PTZConfiguration.token);
            File.AppendAllText("info.txt", string.Format("Media Profile [Name: {0}, Token: {1}, PTZ Config. Name: {2}, PTZ Config. Token: {3}]\n", mediaProfile.Name, mediaProfile.token, mediaProfile.PTZConfiguration.Name, mediaProfile.PTZConfiguration.token));
            PTZSpeed velocity = new PTZSpeed();
            velocity.PanTilt = new Vector2D() { x = speed * ptzConfigurationOptions.Spaces.ContinuousPanTiltVelocitySpace[0].XRange.Max, y = 0 };

            PtzClient.ContinuousMove(mediaProfile.token, velocity, null);
        }

        /// <summary>
        /// Tilt the camera (uses the first media profile that is PTZ capable)
        /// </summary>
        /// <param name="speed">Percent of max speed to move the camera (1-100)</param>
        public void Tilt(float speed)
        {
            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
            PTZConfigurationOptions ptzConfigurationOptions = PtzClient.GetConfigurationOptions(mediaProfile.PTZConfiguration.token);

            PTZSpeed velocity = new PTZSpeed();
            velocity.PanTilt = new Vector2D() { x = 0, y = speed * ptzConfigurationOptions.Spaces.ContinuousPanTiltVelocitySpace[0].YRange.Max };

            PtzClient.ContinuousMove(mediaProfile.token, velocity, null);
        }

        /// <summary>
        /// Zoom the camera (uses the first media profile that is PTZ capable)
        /// </summary>
        /// <param name="speed">Percent of max speed to move the camera (1-100)</param>
        public void Zoom(float speed)
        {
            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
            PTZConfigurationOptions ptzConfigurationOptions = PtzClient.GetConfigurationOptions(mediaProfile.PTZConfiguration.token);

            PTZSpeed velocity = new PTZSpeed();
            velocity.Zoom = new Vector1D() { x = speed * ptzConfigurationOptions.Spaces.ContinuousZoomVelocitySpace[0].XRange.Max };

            PtzClient.ContinuousMove(mediaProfile.token, velocity, null);
        }

        /// <summary>
        /// Stop the camera (uses the first media profile that is PTZ capable).
        /// NOTE: may not work if not issued in conjunction with a move command
        /// </summary>
        public void Stop()
        {
            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
            PtzClient.Stop(mediaProfile.token, true, true);
        }

        /// <summary>
        /// Move PTZ to provided preset number (defaults to media profile 0)
        /// </summary>
        /// <param name="presetNumber">Preset to use</param>
        public void ShowPreset(int presetNumber)
        {
            string presetToken = string.Empty;

            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
            string profileToken = mediaProfile.token;

            PTZPreset[] presets = PtzClient.GetPresets(profileToken);
            if (presets.Length >= presetNumber)
            {
                presetToken = presets[presetNumber - 1].token;

                PTZSpeed velocity = new PTZSpeed();
                velocity.PanTilt = new Vector2D() { x = (float)-0.5, y = 0 }; ;

                PtzClient.GotoPreset(profileToken, presetToken, velocity);
            }
            else
            {
                throw new Exception(string.Format("Invalid Preset requested - preset number {0}", presetNumber));
            }
        }

        /// <summary>
        /// *DON'T USE - not completed. Call up a preset by Profile/Preset token
        /// </summary>
        /// <param name="profileToken"></param>
        /// <param name="presetToken"></param>
        private void ShowPreset(string profileToken, string presetToken)
        {
            OnvifIPClib.ServiceReferenceMedia.Profile[] mediaProfiles = MediaClient.GetProfiles();
            profileToken = mediaProfiles[0].token;

            if (IsValidPresetToken(profileToken, presetToken))
            {
                PTZSpeed velocity = new PTZSpeed();
                velocity.PanTilt = new Vector2D() { x = (float)-0.5, y = 0 }; ;

                PtzClient.GotoPreset(profileToken, presetToken, velocity);
            }
            else
            {
                throw new Exception(string.Format("Invalid Preset requested - preset token {0}", presetToken));
            }
        }

        public bool IsValidPresetToken(string profileToken, string presetToken)
        {
            PTZPreset[] presets = PtzClient.GetPresets(profileToken);
            foreach (PTZPreset p in presets)
            {
                if (p.token == presetToken)
                    return true;
            }

            return false;
        }

        public PTZStatus GetPtzLocation()
        {
            OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();

            PTZStatus status = PtzClient.GetStatus(mediaProfile.token);
            return status;
        }

        public bool IsPtz()
        {
            try
            {
                OnvifIPClib.ServiceReferenceMedia.Profile mediaProfile = GetMediaProfile();
                PtzAvailable = true;
            }
            catch (Exception ex)
            {
                PtzAvailable = false;
            }

            return PtzAvailable;
        }

    }
}
