using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Com.Twilio.Client;
using Android.Support.V4.App;
using System;
using Android;
using Android.Content;
using Xamarin.Forms.Twilio.PoC.Helper;
using System.Collections.Generic;
using static Com.Twilio.Client.Twilio;

namespace Xamarin.Forms.Twilio.PoC.Droid
{
    [Activity(Label = "Xamarin.Forms.Twilio.PoC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IInitListener
    {
        public Com.Twilio.Client.Device _device;
        public IConnection _connection;
        public string TAG = "YOUR APPLICATION TAG - NOT NECESSARY";
        public bool isMicrophoneGranted = false;
        public string Token = "YOUR TWILIO TOKEN";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            GlobalEvents.OnPhoneCall += MainActivity_OnPhoneCall;
            GlobalEvents.OnPhoneHangout += MainActivity_OnPhoneHangout;
            GlobalEvents.OnMute += GlobalEvents_OnMute;

            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.RecordAudio }, 1);
        }

        protected override void OnResume()
        {
            base.OnResume();

            var intent = this.Intent;
            var connection = intent.GetParcelableExtra(Com.Twilio.Client.Device.ExtraConnection).JavaCast<IConnection>();

            if (intent.GetParcelableExtra(Com.Twilio.Client.Device.ExtraDevice) is Com.Twilio.Client.Device device && connection != null)
            {
                intent.RemoveExtra(Com.Twilio.Client.Device.ExtraDevice);
                intent.RemoveExtra(Com.Twilio.Client.Device.ExtraConnection);
                HandleIncomingConnection(device, connection);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            int i = 0;

            foreach (var item in permissions)
            {
                if (item == Manifest.Permission.RecordAudio && grantResults[i] == Permission.Granted)
                {
                    isMicrophoneGranted = true;

                    Com.Twilio.Client.Twilio.Initialize(this.ApplicationContext, this);

                    break;
                }

                i++;
            }
        }

        public void OnInitialized()
        {
            if (_device == null)
            {
                try
                {
                    _device = Com.Twilio.Client.Twilio.CreateDevice(Token, null);

                    var intent = new Intent(this.ApplicationContext, typeof(MainActivity));
                    var pendingIntent = PendingIntent.GetActivity(this.ApplicationContext, 0, intent, PendingIntentFlags.UpdateCurrent);

                    _device.SetIncomingIntent(pendingIntent);
                }
                catch (Exception ex)
                {
                   //HANDLE THE ERROR
                }
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            this.Intent = intent;
        }

        void HandleIncomingConnection(Com.Twilio.Client.Device device, IConnection connection)
        {
            if (_connection != null)
                _connection.Disconnect();
            _connection = connection;
            _connection.Accept();
        }

        public void OnError(Java.Lang.Exception p0)
        {
            //HANDLE THE ERROR
        }

        private void GlobalEvents_OnMute(object sender, TwilioEventArgs e)
        {
            _connection.Muted = !_connection.Muted;
        }

        private void MainActivity_OnPhoneCall(object sender, TwilioEventArgs e)
        {
            var parameters = new Dictionary<string, string>() {
                { "CallId", e.CallID }
            };

            if (_device != null)
            {
                _connection = _device.Connect(parameters, null);
                _connection.Muted = false;
            }
        }

        private void MainActivity_OnPhoneHangout(object sender, TwilioEventArgs e)
        {
            if (_connection != null)
            {
                _connection.Disconnect();
            }
        }
    }
}