using Xamarin.iOS.Twilio.Client;
using Foundation;
using UIKit;
using Xamarin.Forms.Twilio.PoC.Helper;

namespace Xamarin.Forms.Twilio.PoC.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public TCDevice _device;
        public TCConnection _connection;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            GlobalEvents.OnPhoneCall += MainActivity_OnPhoneCall;
            GlobalEvents.OnPhoneHangout += MainActivity_OnPhoneHangout;
            GlobalEvents.OnMute += GlobalEvents_OnMute;

            return base.FinishedLaunching(app, options);
        }

        private void MainActivity_OnPhoneCall(object sender, TwilioEventArgs e)
        {
            //NSDictionary parameters = NSDictionary.FromObjectsAndKeys(
            //    new object[] { from, to },
            //    new object[] { "Source", "Target" }
            //);

            NSDictionary parameters = NSDictionary.FromObjectsAndKeys(
                new object[] { e.CallID },
                new object[] { "ClaraCallId" }
            );

            _connection = _device.Connect(parameters, null);
        }

        private void GlobalEvents_OnMute(object sender, TwilioEventArgs e)
        {
            _connection.Muted = !_connection.Muted;
        }

        private void MainActivity_OnPhoneHangout(object sender, TwilioEventArgs e)
        {
            _connection.Disconnect();
        }

        private void SetupDeviceEvents()
        {
            if (_device != null)
            {
                // When a new connection comes in, 
                //store it and use it to accept the incoming call.
                _device.ReceivedIncomingConnection += (sender, e) => {
                    _connection = e.Connection;
                    _connection.Accept();
                };
            }
        }
    }
}
