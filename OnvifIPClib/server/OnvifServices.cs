﻿using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using Onvif_Interface.WsSecurity;
using OnvifIPClib.ServiceReferenceDevice;
using OnvifIPClib.ServiceReferencePtz;
using OnvifIPClib.ServiceReferenceEvents;

namespace SDS.Video.Onvif
{
    static class OnvifServices
    {

        public static DeviceClient GetOnvifDeviceClient(string ip, int port, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(string.Format("http://{0}:{1}/onvif/device_service", ip, port));

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            DeviceClient deviceClient = new DeviceClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password);
                deviceClient.Endpoint.Behaviors.Add(behavior);
            }

            return deviceClient;
        }

        public static DeviceClient GetOnvifDeviceClient(string ip, int port, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(string.Format("http://{0}:{1}/onvif/device_service", ip, port));

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            DeviceClient deviceClient = new DeviceClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                deviceClient.Endpoint.Behaviors.Add(behavior);
            }

            return deviceClient;
        }

        public static OnvifIPClib.ServiceReferenceMedia.MediaClient GetOnvifMediaClient(string Uri, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(Uri);

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            OnvifIPClib.ServiceReferenceMedia.MediaClient mediaClient = new OnvifIPClib.ServiceReferenceMedia.MediaClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                mediaClient.Endpoint.Behaviors.Add(behavior);
            }

            return mediaClient;
        }

        public static PTZClient GetOnvifPTZClient(string Uri, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(Uri);

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            PTZClient ptzClient = new PTZClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password);
                ptzClient.Endpoint.Behaviors.Add(behavior);
            }

            return ptzClient;
        }

        public static EventPortTypeClient GetEventClient(string uri, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(uri);

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            EventPortTypeClient client = new EventPortTypeClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                client.Endpoint.Behaviors.Add(behavior);
            }

            return client;
        }

        public static SubscriptionManagerClient GetSubscriptionManagerClient(string uri, double deviceTimeOffset, string username = "", string password = "") // string ip, int port, List<MessageHeader> headers)
        {
            EndpointAddress serviceAddress = new EndpointAddress(uri); // string.Format("http://{0}:{1}/onvif/event_service", ip, port));

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.WSAddressing10);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            SubscriptionManagerClient client = new SubscriptionManagerClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                client.Endpoint.Behaviors.Add(behavior);
            }

            return client;
        }

        public static NotificationProducerClient GetNotificationProducerClient(string uri, double deviceTimeOffset, string username = "", string password = "")
        {
            EndpointAddress serviceAddress = new EndpointAddress(uri);

            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.WSAddressing10);
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            NotificationProducerClient client = new NotificationProducerClient(bind, serviceAddress);

            if (username != string.Empty)
            {
                // Handles adding of SOAP Security header containing User Token (user, nonce, pwd digest)
                PasswordDigestBehavior behavior = new PasswordDigestBehavior(username, password, deviceTimeOffset);
                client.Endpoint.Behaviors.Add(behavior);
            }

            return client;
        }
    }
}
