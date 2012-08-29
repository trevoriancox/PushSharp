using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Common;
using SignalR;

namespace PushSharp.WebNotifications
{
	public class WebNotificationPushService : PushServiceBase
	{
		public WebNotificationPushService(WebNotificationPushChannelSettings channelSettings, PushServiceSettings serviceSettings)
			: base(channelSettings, serviceSettings)
		{
		}

		//Given how signalr works, we only want a singleton instance of it, as it really doesn't make sense to try 'scaling' to multiple channels here
		WebNotificationPushChannel channelSingleton = null;

		protected override PushChannelBase CreateChannel(PushChannelSettings channelSettings)
		{
			var webNotificationChannelSettings = channelSettings as WebNotificationPushChannelSettings;

			if (channelSingleton == null)
				channelSingleton = new WebNotificationPushChannel(webNotificationChannelSettings, this.ServiceSettings);

			return channelSingleton;
		}
	}
}
