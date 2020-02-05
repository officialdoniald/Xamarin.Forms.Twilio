using System;

namespace Xamarin.Forms.Twilio.PoC.Helper
{
    public class GlobalEvents
    {
        public static event EventHandler<TwilioEventArgs> OnPhoneCall;
        public static event EventHandler<TwilioEventArgs> OnPhoneHangout;
        public static event EventHandler<TwilioEventArgs> OnMute;

        public static void OnPhoneCall_Event(object sender, TwilioEventArgs args)
        {
            OnPhoneCall?.Invoke(sender, args);
        }

        public static void OnPhoneHangout_Event(object sender, TwilioEventArgs args)
        {
            OnPhoneHangout?.Invoke(sender, args);
        }

        public static void OnMute_Event(object sender, TwilioEventArgs args)
        {
            OnMute?.Invoke(sender, args);
        }
    }

    public class TwilioEventArgs
    {
        public string CallID { get; set; }
    }
}