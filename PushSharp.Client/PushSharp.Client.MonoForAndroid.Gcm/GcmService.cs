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
	[Service]
	internal class GCMService : GCMBaseIntentService
	{
		public const string TAG = "GCM-SERVICE";

		public static Action<Context, string> GcmUnRegisteredAction;
		public static Action<Context, string> GcmRegisteredAction;
		public static Action<Context, Intent> GcmMessageReceivedAction;
		public static Action<Context, string> GcmRecoverableErrorAction;
		public static Action<Context, string> GcmErrorAction;

		public static string[] SenderIDs { get; set; }
		
		public GCMService() : base(SenderIDs) {}
		
		protected override void OnRegistered (Context context, string registrationId)
		{
			Log.Verbose(TAG, "Registered: " + registrationId);
			//Send back to the server
			//	var wc = new WebClient();
			//	var result = wc.UploadString("http://your.server.com/api/register/", "POST", 
			//		"{ 'registrationId' : '" + registrationId + "' }");
			
			var evt = GcmRegisteredAction;
			if (evt != null)
				evt(context, registrationId);
		}
		
		protected override void OnUnRegistered (Context context, string registrationId)
		{
			Log.Verbose(TAG, "Unregistered: " + registrationId);
			//Remove from the web service
			//	var wc = new WebClient();
			//	var result = wc.UploadString("http://your.server.com/api/unregister/", "POST",
			//		"{ 'registrationId' : '" + lastRegistrationId + "' }");
			
			var evt = GcmUnRegisteredAction;
			if (evt != null)
				evt(context, registrationId);
		}
		
		protected override void OnMessage (Context context, Intent intent)
		{
			Log.Info(TAG, "Message Received");
			
			var msg = new StringBuilder();
			
			if (intent != null && intent.Extras != null)
			{
				foreach (var key in intent.Extras.KeySet())
					msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
			}
			
			//Store the message
			var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
			var edit = prefs.Edit();
			edit.PutString("last_msg", msg.ToString());
			edit.Commit();
			
			var evt = GcmMessageReceivedAction;
			if (evt != null)
				evt(context, intent);
		}
		
		protected override bool OnRecoverableError (Context context, string errorId)
		{
			Log.Warn(TAG, "Recoverable Error: " + errorId);
			
			var evt = GcmRecoverableErrorAction;
			if (evt != null)
				evt(context, errorId);
			
			return base.OnRecoverableError (context, errorId);
		}
		
		protected override void OnError (Context context, string errorId)
		{
			Log.Error(TAG, "Error: " + errorId);
			
			var evt = GcmErrorAction;
			if (evt != null)
				evt(context, errorId);
		}
	}
}

