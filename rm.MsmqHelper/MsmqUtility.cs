using System;
using System.Messaging;

namespace rm.MsmqHelper
{
    /// <summary>
    /// MSMQ utility methods.
    /// </summary>
    public static class MsmqUtility
    {
        /// <summary>
        /// Gets queue for path. Creates if queue does not exist.
        /// </summary>
        public static MessageQueue GetQueue(string path)
        {
            MessageQueue q;
            if (!MessageQueue.Exists(path))
            {
                q = MessageQueue.Create(path);
            }
            else
            {
                q = new MessageQueue(path);
            }
            return q;
        }
        /// <summary>
        /// Purge given queues.
        /// </summary>
        public static void PurgeAll(MessageQueue[] queues)
        {
            foreach (var q in queues)
            {
                q.Purge();
            }
        }
    }
}
