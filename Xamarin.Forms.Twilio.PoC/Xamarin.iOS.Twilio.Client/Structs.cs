using System;
using Foundation;

namespace Xamarin.iOS.Twilio.Client
{
	public enum TCConnectionState
	{
		Pending = 0,
		Connecting,
		Connected,
		Disconnected
	}

	public enum TCDeviceState
	{
		Offline = 0,
		Ready,
		Busy
	}

	public class TCConnectionParameters : DictionaryContainer
	{

		public TCConnectionParameters() : base(new NSMutableDictionary())
		{
		}

		public TCConnectionParameters(NSDictionary dictionary) : base(dictionary)
		{
		}

		static NSString sourceKey = new NSString("Source");
		static NSString targetKey = new NSString("Target");

		public string Source
		{
			get { return GetStringValue(sourceKey); }
			set { SetStringValue(sourceKey, value); }
		}

		public string Target
		{
			get { return GetStringValue(targetKey); }
			set { SetStringValue(targetKey, value); }
		}

	}
}

