using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace GCMSharp.Client
{
	public class GcmClient
	{
		public delegate void GcmUnRegisteredDelegate(Context context, string registrationId);
		public delegate void GcmRegisteredDelegate(Context context, string registrationId);
		public delegate void GcmMessageReceivedDelegate(Context context, Intent intent);
		public delegate void GcmRecoverableErrorDelegate(Context context, string errorId);
		public delegate void GcmErrorDelegate(Context context, string errorId);
		
		public static event GcmUnRegisteredDelegate UnRegistered;
		public static event GcmRegisteredDelegate Registered;
		public static event GcmMessageReceivedDelegate MessageReceived;
		public static event GcmRecoverableErrorDelegate RecoverableError;
		public static event GcmErrorDelegate Error;
		
		public static string[] SenderIDs { get { return GCMService.SenderIDs; } private set { GCMService.SenderIDs = value; } }
		
		static GcmClient()
		{
			GCMService.GcmRegisteredAction = (context, registrationId) => 
			{
				var evt = Registered;
				if (evt != null)
					evt(context, registrationId);
			};

			GCMService.GcmUnRegisteredAction = (context, registrationId) => 
			{
				var evt = UnRegistered;
				if (evt != null)
					evt(context, registrationId);
			};

			GCMService.GcmMessageReceivedAction = (context, intent) => 
			{
				var evt = MessageReceived;
				if (evt != null)
					evt(context, intent);
			};

			GCMService.GcmRecoverableErrorAction = (context, errorId) => 
			{
				var evt = RecoverableError;
				if (evt != null)
					evt(context, errorId);
			};

			GCMService.GcmErrorAction = (context, errorId) => 
			{
				var evt = Error;
				if (evt != null)
					evt(context, errorId);
			};
		}
		
		public static void Initialize(Context context, params string[] senderIds)
		{
			SenderIDs = senderIds;
			GCMSharp.Client.GCMRegistrar.CheckDevice(context);
			GCMSharp.Client.GCMRegistrar.CheckManifest(context);
		}
		
		public static void Register(Context context)
		{
			GCMSharp.Client.GCMRegistrar.Register(context, SenderIDs);
		}
		
		public static void UnRegister(Context context)
		{
			GCMSharp.Client.GCMRegistrar.UnRegister(context);
		}
		
		public static string GetRegistrationId(Context context)
		{
			var registrationId = string.Empty;
			registrationId = GCMSharp.Client.GCMRegistrar.GetRegistrationId(context);
			return registrationId;
		}
	}
}

