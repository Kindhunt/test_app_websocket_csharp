using System;
using System.Threading.Tasks;

namespace TestAppWebsocket
{
    public delegate void NotificationBroadcastEventHandler(object source, NotificationReceivedEventArgs args);
    public interface IWebSocketService
    {
        public event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
        Task ConnectAsync(string serverUri, ApplicationNotificationVM notificationVM);
        Task BroadcastNotificationAsync();
        Task DisconnectAsync();
    }
    internal class WebSocketService : IWebSocketService
    {
        private Client _client;
        public event EventHandler<NotificationReceivedEventArgs> NotificationReceived;

        public WebSocketService(Client client) {
            _client = client;
        }
        public async Task ConnectAsync(string serverUri, ApplicationNotificationVM notificationVM) {
            await _client.Connect(serverUri, notificationVM);
        }

        public async Task BroadcastNotificationAsync() {
            await _client.BroadcastNotification();
        }

        public async Task DisconnectAsync() {
            await _client.Disconnect();
        }
    }
}
