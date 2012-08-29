using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WebNotifications
{
    public class WebNotificationPushChannelSettings : PushChannelSettings
    {
		public WebNotificationPushChannelSettings(Uri serverUri, string path = null) : base()
		{
			this.ServerUri = serverUri;
			this.Path = path;
		}

		public Uri ServerUri
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}
    }
}
