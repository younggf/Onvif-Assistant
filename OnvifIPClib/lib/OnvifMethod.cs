using OnvifIPClib.entity;
using OnvifIPClib.ServiceReferenceDevice;
using OnvifIPClib.ServiceReferenceMedia;
using Ozeki.Camera;
using SDS.Video.Onvif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace OnvifIPClib.lib
{
    public class OnvifMethod
    {
        public static event EventHandler<IPC> OnMessage;
        public static void GetIpCameras()
        {
            IPCameraFactory.DeviceDiscovered += IPCameraFactory_DeviceDiscovered;
            IPCameraFactory.DiscoverDevices();
        }

        static void IPCameraFactory_DeviceDiscovered(object sender, DiscoveryEventArgs e)
        {
            Console.WriteLine("[IPCamera] Host: " + e.Device.Host + " Port: " + e.Device.Port);
            //GuiThread(() => DiscoveredDeviceList.Items.Add("[IPCamera] Host: " + e.Device.Host + " Port: " + e.Device.Port));
            IPC ipc = new IPC();
            ipc.ip = e.Device.Host;
            ipc.port = e.Device.Port;
            if (OnMessage != null) OnMessage(null,ipc);

        }
        public static string getRtspUrl(string ip, int port, string userName, string password)
        {
            string rtspUrl = "";
            string xaddr = "http://" + ip + ":" + port + "/onvif/Media";

            DeviceClient client = OnvifServices.GetOnvifDeviceClient(ip, port); //, "service", "Sierra123")) // new DeviceClient(bind, serviceAddress))
            client.Endpoint.Behaviors.Add(new EndpointDiscoveryBehavior());

            //   gbxPtzControl.Visible = true;
            client = OnvifServices.GetOnvifDeviceClient(ip, port);
            MediaClient mclient = OnvifServices.GetOnvifMediaClient(xaddr, 0, userName, password);
            Profile[] mProfiles = mclient.GetProfiles();

            if (mProfiles.Length > 0)
            {
                Profile p = mProfiles[0];
                StreamSetup ss = new StreamSetup();
                ss.Stream = StreamType.RTPUnicast;
                ss.Transport = new Transport() { Protocol = TransportProtocol.RTSP };
                try
                {
                    MediaUri mu = mclient.GetStreamUri(ss, p.token);
                    rtspUrl = mu.Uri;
                }
                catch { };
            }
            return rtspUrl;
        }
    }
}
