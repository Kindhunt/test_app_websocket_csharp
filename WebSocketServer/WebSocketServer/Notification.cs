namespace WebSocketServer
{
    internal class Notification
    {
        public string MessageType { get; set; }
        public string Event { get; set; }
        public int UserId { get; set; }
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
}
