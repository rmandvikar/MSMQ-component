using System;
using System.Messaging;

namespace rm.MsmqHelper
{
    /// <summary>
    /// Defines methods to process items of type T from MSMQ.
    /// <para></para>
    /// The items are sent to the <paramref name="Queue"/>. The <paramref name="Queue"/> is 
    /// received in batches of <paramref name="QueueBatchCount"/> items. 
    /// On exception, the batch items are sent to the <paramref name="ErrorQueue"/>. 
    /// <para></para>
    /// The <paramref name="ErrorQueue"/> is received one item at a time. 
    /// On exception, the item is sent to the <paramref name="FatalQueue"/>. 
    /// <para></para>
    /// The <paramref name="FatalQueue"/> is not received at all.
    /// </summary>
    /// <typeparam name="T">Type to process.</typeparam>
    public interface IMsmqProcessor<T>
        where T : class, new()
    {
        /// <summary>
        /// The queue where items are sent. 
        /// </summary>
        MessageQueue Queue { get; }
        /// <summary>
        /// The queue where the batch items are sent when exception is thrown while processing
        /// <paramref name="Queue"/>.
        /// </summary>
        MessageQueue ErrorQueue { get; }
        /// <summary>
        /// The queue where the item is sent when exception is thrown while processing
        /// <paramref name="ErrorQueue"/>.
        /// </summary>
        MessageQueue FatalQueue { get; }
        /// <summary>
        /// The queues collection of Queue, ErrorQueue, and FatalQueue.
        /// </summary>
        MessageQueue[] Queues { get; }
        /// <summary>
        /// Batch count for <paramref name="Queue"/>.
        /// </summary>
        int QueueBatchCount { get; }
        /// <summary>
        /// The time to wait for message while receiving any queue. 
        /// </summary>
        TimeSpan ReceiveTimeout { get; }
        /// <summary>
        /// Send items to the <paramref name="Queue"/>.
        /// </summary>
        void Send(T[] items);
        /// <summary>
        /// Receive items from <paramref name="Queue"/> in batches and process them using <paramref name="processCallback"/>.
        /// <para></para>
        /// The <paramref name="processCallback"/> should not swallow exception.
        /// </summary>
        void Receive(Action<T[]> processCallback);
    }
}
