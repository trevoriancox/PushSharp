using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Common;
using SignalR;

namespace PushSharp.WebNotifications
{
	public class WebNotificationPushChannel : PushChannelBase
	{
		SignalR.Hosting.Self.Server server = null;
		
		SignalR.Hubs.IHubContext hubContext;

		public WebNotificationPushChannel(WebNotificationPushChannelSettings channelSettings = null, PushServiceSettings serviceSettings = null)
			: base(channelSettings, serviceSettings)
		{
			server = new SignalR.Hosting.Self.Server(channelSettings.ServerUri.ToString());

			var path = "/";
			if (!string.IsNullOrEmpty(channelSettings.Path))
				path = "/" + channelSettings.Path.TrimStart('/');

			server.MapHubs(path);
					
			server.Start();

			hubContext = server.ConnectionManager.GetHubContext<ChannelSignalRHub>();
		}
		
		protected override void SendNotification(Notification notification)
		{
			var webNotification = notification as WebNotification;

			var connectionId = ChannelSignalRHub.GetConnectionId(webNotification.ClientId);

			if (webNotification != null)
				hubContext.Clients[connectionId].say(webNotification.Json);
		}

		public override void Stop(bool waitForQueueToDrain)
		{
			base.Stop(waitForQueueToDrain);
			
			if (server != null)
				server.Stop();
		}
	}

	class ChannelSignalRHub : SignalR.Hubs.Hub, SignalR.Hubs.IDisconnect
	{
		static Dictionary<string, string> connectionClientMap = new Dictionary<string, string>();

		public static string GetConnectionId(string clientId)
		{
			if (connectionClientMap.ContainsKey(clientId))
				return connectionClientMap[clientId];
			else
				return null;
		}

		public bool Register(string clientId)
		{
			var connectionId = Context.ConnectionId;

			if (connectionClientMap.ContainsKey(clientId))
				connectionClientMap[clientId] = connectionId;
			else
				connectionClientMap.Add(clientId, connectionId);

			return true;
		}

		public Task Disconnect()
		{
			var connectionId = Context.ConnectionId;
			var clientDeviceId = string.Empty;

			foreach (var key in connectionClientMap.Keys)
			{
				if (connectionClientMap[key].Equals(connectionId))
				{
					clientDeviceId = key;
					break;
				}
			}

			if (!string.IsNullOrEmpty(clientDeviceId))
				connectionClientMap.Remove(clientDeviceId);

			return this.Clients.leave(connectionId, DateTime.Now.ToString());
		}
	}
}
