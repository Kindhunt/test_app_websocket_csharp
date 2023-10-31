using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAppWebsocket
{
    public delegate void Notify(Notification notification);
    internal class Client
    {
        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private ApplicationNotificationVM _notificationVM;
     
        public event Notify NotificationReceived;

        public async Task Connect(string serverUri, ApplicationNotificationVM notificationVM) {
            _notificationVM = notificationVM;

            await _clientWebSocket.ConnectAsync(new Uri(serverUri), CancellationToken.None);
            await SubscribeToNotifications();
        }
        private async Task SendMessage(object _obj)
        {
            try {
                string jsonString = JsonConvert.SerializeObject(_obj);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex) {
                throw new Exception($"Some problem when send message, here is error: {ex.Message}");
            }
        }
        private async Task<T> ReceiveMessage<T>()
        {
            try
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string jsonString = Encoding.UTF8.GetString(buffer);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex) {
                throw new Exception($"Some problem when receive message, here is error: {ex.Message}");
            }
        }

        public async Task BroadcastNotification()
        {
            Notification notificationSend = new Notification().SetMessageType("Broadcast").SetEvent("Broadcasting message to clients"); 
            await SendMessage(notificationSend);
        }

        public async Task SubscribeToNotifications() {
            while (true) {
                var notification = await ReceiveMessage<Notification>();
                if (notification.MessageType == "Broadcast") {
                    OnNotificationReceived(notification);
                }
            }
        }

        public async Task Disconnect()
        {
            await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting...", CancellationToken.None);
            _clientWebSocket.Dispose();
        }
        protected virtual void OnNotificationReceived(Notification notification) {
            _notificationVM.Event = notification.Event;
            _notificationVM.MessageType = notification.MessageType;
            _notificationVM.UserId = notification.UserId;

            NotificationReceived?.Invoke(notification);
        }
    }
}
