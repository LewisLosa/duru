namespace Common.Library
{
    /// <summary>
    /// Message broker for sending and receiving messages between components.
    /// </summary>
    public class MessageBroker
    {
        /// <summary>
        /// Singleton instance of the MessageBroker.
        /// </summary>
        private static readonly Lazy<MessageBroker> _instance = new Lazy<MessageBroker>(() => new MessageBroker());

        public static MessageBroker Instance => _instance.Value;

        /// <summary>
        /// Event for message receiving.
        /// </summary>
        public event EventHandler<MessageBrokerEventArgs> MessageReceived;

        private MessageBroker() { }

        /// <summary>
        /// Sends a message with no payload.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        public void SendMessage(string messageName) => SendMessage(messageName, null);

        /// <summary>
        /// Sends a message with a payload.
        /// </summary>
        /// <param name="messageName">The name of the message.</param>
        /// <param name="payload">The payload object.</param>
        public void SendMessage(string messageName, object payload)
        {
            var args = new MessageBrokerEventArgs(messageName, payload);
            MessageReceived?.Invoke(this, args);
        }
    }
}