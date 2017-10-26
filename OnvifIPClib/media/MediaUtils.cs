
using OnvifIPClib.ServiceReferenceMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnvifIPClib.media
{
    class MediaUtils
    {
        public static List<string> GetMediaProfileUris(MediaClient mclient, Profile p)
        {
            List<string> uris = new List<string>();
            StreamSetup ss = new StreamSetup();

            // Unicast options
            ss.Stream = StreamType.RTPUnicast;

            //ss.Transport = new Transport() { Protocol = TransportProtocol.HTTP };

            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            ss.Transport = new Transport() { Protocol = TransportProtocol.RTSP };
            try
            {
                MediaUri mu = mclient.GetStreamUri(ss, p.token);
                uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            }
            catch { };

            //ss.Transport = new Transport() { Protocol = TransportProtocol.TCP };
            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            //ss.Transport = new Transport() { Protocol = TransportProtocol.UDP };
            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            //// Multicast options
            //ss.Stream = StreamType.RTPMulticast;

            //ss.Transport = new Transport() { Protocol = TransportProtocol.HTTP };

            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            //ss.Transport = new Transport() { Protocol = TransportProtocol.RTSP };
            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            //ss.Transport = new Transport() { Protocol = TransportProtocol.TCP };
            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            //ss.Transport = new Transport() { Protocol = TransportProtocol.UDP };
            //try
            //{
            //    MediaUri mu = mclient.GetStreamUri(ss, p.token);
            //    uris.Add(ss.Transport.Protocol.ToString() + "\t" + mu.Uri + " (" + ss.Stream.ToString() + ")");
            //}
            //catch { };

            return uris;
        }

    }
}
