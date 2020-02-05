namespace Xamarin.Forms.Twilio.PoC.Implementation.Twilio
{
    public interface ITwilioClient
    {
        void MakeOutboundCall(string callid);
        void HangoutCall();
        void Mute();
    }
}