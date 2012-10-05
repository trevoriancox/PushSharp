
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
using GCMSharp.Client;

namespace PushSharp.ClientSample.MonoForAndroid
{
	public class GcmHandler
	{
		//Be sure to use your app's own SENDER ID here!
		public const string SENDER_ID = "YOUR_SENDER_ID_HERE";

		public static void Initialize(Context context)
		{
			//You can change this call to include multiple SENDER IDs if
			// if you need to
			GcmClient.Initialize(context, SENDER_ID);
		}
		
		public static void Register (Context context)
		{
			GcmClient.Register(context);
		}

		public static void UnRegister(Context context)
		{
			GcmClient.UnRegister(context);
		}

		public static string GetRegistrationId (Context context)
		{
			return GcmClient.GetRegistrationId(context);
		}

		//CTOR to setup some event handling
		static GcmHandler()
		{
			GcmClient.Registered += (context, registrationId) => 
			{
				//Send back to the server
				//	var wc = new WebClient();
				//	var result = wc.UploadString("http://your.server.com/api/register/", "POST", 
				//		"{ 'registrationId' : '" + registrationId + "' }");
				
				createNotification("PushSharp-GCM Registered...", "The device has been Registered, Tap to View!");
			};

			GcmClient.UnRegistered += (context, registrationId) => {

				//Remove from the web service
				//	var wc = new WebClient();
				//	var result = wc.UploadString("http://your.server.com/api/unregister/", "POST",
				//		"{ 'registrationId' : '" + lastRegistrationId + "' }");
				
				createNotification("PushSharp-GCM Unregistered...", "The device has been unregistered, Tap to View!");
			};

			GcmClient.MessageReceived += (context, intent) => 
			{
				var msg = new StringBuilder();
				
				if (intent != null && intent.Extras != null)
				{
					foreach (var key in intent.Extras.KeySet())
						msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
				}
				
				//Store the message - You don't need to do this, just showing an example of something you could
				// do with the message intent ...
				var prefs = Android.App.Application.Context.ApplicationContext.GetSharedPreferences(context.PackageName, FileCreationMode.Private);
				var edit = prefs.Edit();
				edit.PutString("last_msg", msg.ToString());
				edit.Commit();
				
				createNotification("PushSharp-GCM Msg Rec'd", "Message Received for C2DM-Sharp... Tap to View!");
			};

			GcmClient.Error += (context, errorId) => 
			{
				//Do something to show the user an error maybe?
			};

			GcmClient.RecoverableError += (context, errorId) => 
			{
				//Do something to show the user an error maybe?
			};
		}

		//You don't need to do this yourself, this is just an example of how to show in the notification
		// menu that something happened :)
		static void createNotification(string title, string desc)
		{
			var context = Android.App.Application.Context.ApplicationContext;

			//Create notification
			var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
			
			//Create an intent to show ui
			var uiIntent = new Intent(context, typeof(DefaultActivity));
			
			//Create the notification
			var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);
			
			//Auto cancel will remove the notification once the user touches it
			notification.Flags = NotificationFlags.AutoCancel;
			
			//Set the notification info
			//we use the pending intent, passing our ui intent over which will get called
			//when the notification is tapped.
			notification.SetLatestEventInfo(context,
			                                title,
			                                desc,
			                                PendingIntent.GetActivity(context, 0, uiIntent, 0));
			
			//Show the notification
			notificationManager.Notify(1, notification);
		}
	}
}

