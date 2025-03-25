using System;

namespace Common.Library
{
    /// <summary>
    /// Event args for passing data when raising event in MessageBroker
    /// </summary>
    public class MessageBrokerEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructor for MessageBrokerEventArgs class
        /// </summary>
        public MessageBrokerEventArgs() : base()
        {
        }

        /// <summary>
        /// Constructor for MessageBrokerEventArgs class
        /// </summary>
        /// <param name="messageName">A Message Name</param>
        /// <param name="payload">The Payload for the Message</param>
        public MessageBrokerEventArgs(string messageName, object payload) : base()
        {
            MessageName = messageName;
            MessagePayload = payload;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Get/Set a Unique Message Name
        /// </summary>
        public string MessageName { get; set; }
        /// <summary>
        /// Get/Set the payload for the message
        /// </summary>
        public object MessagePayload { get; set; }
        #endregion
    }
}