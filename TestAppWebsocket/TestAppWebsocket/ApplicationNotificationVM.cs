using System;
using System.ComponentModel;
using System.Windows.Input;
using Prism.Commands;

// MVVM
namespace TestAppWebsocket
{

    public class NotificationReceivedEventArgs : EventArgs
    {
        public NotificationReceivedEventArgs(Notification notification)
        {
            ReceivedNotification = notification;
        }
        public Notification ReceivedNotification { get; }
    }
    public class Notification
    {
        public string Event;
        public string MessageType;
        public int UserId;
        
        public Notification() { 
            Event = string.Empty;
            MessageType = string.Empty;
            UserId = 0; 
        }

        public Notification SetEvent(string val)
        {
            Event = val;
            return this;
        }
        public Notification SetMessageType(string val)
        {
            MessageType = val;
            return this;
        }
        public Notification SetUserId(int val)
        {
            UserId = val;
            return this;
        }
    }
    public class ApplicationNotificationVM : INotifyPropertyChanged
    {
        private Notification _notification;
        private IWebSocketService _webSocketService;

        public ApplicationNotificationVM()
        {
            _notification = new Notification();
            SendDataCommand = new DelegateCommand(SendData);

            NotificationReceived += (sender, e) =>
            {
                Event = e.ReceivedNotification.Event;
                MessageType = e.ReceivedNotification.MessageType;
                UserId = e.ReceivedNotification.UserId;
            };
        }

        public event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
        public void OnNotificationReceived(Notification notification) {
            NotificationReceived?.Invoke(this, new NotificationReceivedEventArgs(notification));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Event {
            get {
                return _notification.Event;
            }
            set {
                _notification.Event = value;
                OnPropertyChanged(nameof(Event));
            }
        }

        public string MessageType {
            get {
                return _notification.MessageType;
            }
            set {
                _notification.MessageType = value;
                OnPropertyChanged(nameof(MessageType));
            }
        }

        public int UserId {
            get {
                return _notification.UserId;
            }
            set {
                _notification.UserId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        public void SetWebSocketService(IWebSocketService ws) {
            _webSocketService = ws;
            _webSocketService.NotificationReceived += (sender, e) => {
                OnNotificationReceived(e.ReceivedNotification);
            };
        }
        public ICommand SendDataCommand { get; private set; }
        public void SendData() {
            _webSocketService.BroadcastNotificationAsync();
        }
    }
}

