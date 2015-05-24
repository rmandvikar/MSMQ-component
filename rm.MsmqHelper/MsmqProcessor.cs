using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

namespace rm.MsmqHelper
{
    #region todo
    //todo: add multi-threading support
    #endregion

    /// <summary>
    /// MSMQ processor to process items of type <paramref name="T"/>. 
    /// </summary>
    public class MsmqProcessor<T> : IMsmqProcessor<T>
        where T : class, new()
    {
        #region members

        protected readonly MessageQueue queue;
        protected readonly MessageQueue errorQueue;
        protected readonly MessageQueue fatalQueue;
        protected readonly int queueBatchCount;
        protected readonly TimeSpan receiveTimeout;
        protected readonly MessageQueue[] queues;
        protected readonly IReceiver<T> receiver;

        #endregion

        #region ctors

        public MsmqProcessor(
            MessageQueue queue,
            MessageQueue errorQueue,
            MessageQueue fatalQueue,
            int queueBatchCount,
            TimeSpan receiveTimeout,
            IReceiver<T> receiver
            )
        {
            this.queue = queue;
            this.errorQueue = errorQueue;
            this.fatalQueue = fatalQueue;
            this.queueBatchCount = queueBatchCount;
            this.receiveTimeout = receiveTimeout;
            this.queues = new[] { queue, errorQueue, fatalQueue };
            this.receiver = receiver;
        }

        #endregion

        #region IMsmqProcessor<T> methods

        /// <summary>
        /// Send items to the <paramref name="Queue"/>.
        /// </summary>
        public void Send(IEnumerable<T> items)
        {
            Send(items, Queue);
        }

        /// <summary>
        /// Receive from <paramref name="Queue"/> in batches of <paramref name="QueueBatchCount"/>.
        /// Then, receive from <paramref name="ErrorQueue"/> one item at a time.
        /// </summary>
        public void Receive()
        {
            Receive(Queue, QueueBatchCount);
            Receive(ErrorQueue, 1);
        }

        public MessageQueue Queue
        {
            get { return queue; }
        }

        public MessageQueue ErrorQueue
        {
            get { return errorQueue; }
        }

        public MessageQueue FatalQueue
        {
            get { return fatalQueue; }
        }

        public MessageQueue[] Queues
        {
            get { return queues; }
        }

        public int QueueBatchCount
        {
            get { return queueBatchCount; }
        }

        public TimeSpan ReceiveTimeout
        {
            get { return receiveTimeout; }
        }

        public IReceiver<T> Receiver
        {
            get { return receiver; }
        }

        #endregion

        /// <summary>
        /// Send items to the <paramref name="q"/>.
        /// </summary>
        protected void Send(IEnumerable<T> items, MessageQueue q)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        q.Send(item);
                    }
                }
            }
        }
        /// <summary>
        /// Receive from <paramref name="q"/> in batches of <paramref name="batchCount"/> and 
        /// hand off to <paramref name="Receiver"/>.
        /// </summary>
        protected void Receive(MessageQueue q, int batchCount)
        {
            var isLastBatch = false;
            while (!isLastBatch)
            {
                T[] items = null;
                try
                {
                    items = GetNextBatch(q, batchCount, ref isLastBatch);
                    receiver.Receive(items);
                }
                catch (Exception)
                {
                    var fallbackQueue = MsmqUtility.GetFallbackQueue(q, Queues);
                    Send(items, fallbackQueue);
                }
            }
        }
        /// <summary>
        /// Get next batch of <paramref name="batchCount"/>items from <paramref name="q"/>. 
        /// Set <paramref name="isLastBatch"/> if current batch is the last batch.
        /// </summary>
        protected T[] GetNextBatch(MessageQueue q, int batchCount, ref bool isLastBatch)
        {
            var items = new T[batchCount];
            var count = 0;
            while (count < batchCount)
            {
                try
                {
                    var item = (T)q.Receive(ReceiveTimeout).Body;
                    items[count] = item;
                    count++;
                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    {
                        isLastBatch = true;
                        break;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return count == batchCount ? items : items.Take(count).ToArray();
        }
    }
}
