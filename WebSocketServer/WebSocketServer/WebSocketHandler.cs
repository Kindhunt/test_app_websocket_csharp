using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketServer
{
    public static class WebSocketManager
    {
        private static ConcurrentDictionary<int, WebSocket> _webSockets = new ConcurrentDictionary<int, WebSocket>();

        public static void AddWebSocket(int clientId, WebSocket webSocket) {
            _webSockets.TryAdd(clientId, webSocket);
        }

        public static void RemoveWebSocket(int clientId) {
            _webSockets.TryRemove(clientId, out _);
        }
        public static async Task SendMessage(object _obj, WebSocket webSocket)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(_obj);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw new Exception($"Some problem when send message, here is error: {ex.Message}");
            }
        }
        public static async Task<T> ReceiveMessage<T>(WebSocket webSocket)
        {
            try
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string jsonString = string.Empty;
                if(result.MessageType == WebSocketMessageType.Text) {
                    jsonString = Encoding.UTF8.GetString(buffer);
                }
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Some problem when receive message, here is error: {ex.Message}");
            }
        }

        public static async Task BroadcastMessage(object _obj)
        {            
            if (_obj.GetType() == typeof(Notification))
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(System.Configuration.ConfigurationManager.AppSettings["DatabaseConnection"]) // Сделать конфиг файл, откуда будет браться подключение
                    .Options;

                using (var context = new AppDbContext(options))
                {
                    context.Notifications.Add(new NotificationDB((Notification)_obj));
                    context.SaveChanges();
                }
            }            

            foreach (var webSocket in _webSockets.Values) {
                try {
                    if (webSocket.State == WebSocketState.Open) {
                        await SendMessage(_obj, webSocket);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
    public class WebSocketHandler
    {
        private static int _clientsCounter = 0;
        public async Task HandleWebSocket(WebSocket webSocket)
        {
            Console.WriteLine("Handling new websocket connection...");
            // Обработка нового WebSocket-соединения
            var clientId = Interlocked.Increment(ref _clientsCounter);

            WebSocketManager.AddWebSocket(clientId, webSocket);

            try
            {
                byte[] buffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    Console.WriteLine("Receiving message...");
                    var notification = await WebSocketManager.ReceiveMessage<Notification>(webSocket);
                    if (notification.MessageType == "Broadcast")
                    {
                        Console.WriteLine("Broadcast message...");
                        await WebSocketManager.BroadcastMessage(notification.SetUserId(clientId));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            finally {
                WebSocketManager.RemoveWebSocket(clientId);
                webSocket.Dispose();
            }
        }
    }
}
