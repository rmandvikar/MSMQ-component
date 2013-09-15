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
        /// Gets the fallback queue for given queue out of the queues array.
        /// </summary>
        public static MessageQueue GetFallbackQueue(MessageQueue q, MessageQueue[] queues)
        {
            if (q.QueueName == queues[0].QueueName)
            {
                return queues[1];
            }
            else if (q.QueueName == queues[1].QueueName)
            {
                return queues[2];
            }
            throw new ApplicationException("No fallback queue.");
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
