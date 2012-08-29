using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.WebNotifications
{
	public static class WebNotificationFluent 
	{
		public static WebNotification ForClientId(this WebNotification notification, string clientId)
		{
			notification.ClientId = clientId;
			return notification;
		}

		public static WebNotification WithJson(this WebNotification notification, string json)
		{
			notification.Json = json;
			return notification;
		}
	}
}
