using Foundation;
using Xamarin.Forms.Twilio.PoC.Helper;
using Xamarin.Forms.Twilio.PoC.Implementation.Twilio;

namespace Xamarin.Forms.Twilio.PoC.iOS.Implementation.Twilio
{
    public class TwilioClient : ITwilioClient
    {
        [Preserve(Conditional = true)]
        public TwilioClient()
        { }

        public void HangoutCall()
        {
            GlobalEvents.OnPhoneHangout_Event(this, new TwilioEventArgs());
        }

        public void MakeOutboundCall(string callid)
        {
            GlobalEvents.OnPhoneCall_Event(this, new TwilioEventArgs()
            {
                CallID = callid
            });
        }

        public void Mute()
        {
            GlobalEvents.OnMute_Event(this, new TwilioEventArgs());
        }
    }
}