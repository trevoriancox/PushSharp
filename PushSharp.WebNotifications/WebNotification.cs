using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WebNotifications
{
	public class WebNotification : Notification
	{
		public string ClientId
		{
			get;
			set;
		}

		public string Json
		{
			get;
			set;
		}
	}
}
