using System;
using System.Messaging;
using NUnit.Framework;
using rm.MsmqHelper;
using rm.MsmqSample;

namespace rm.MsmqHelperTest
{
    [TestFixture]
    public class MsmqProcessorTest
    {
        static MessageQueue queue = MsmqUtility.GetQueue(@".\private$\queue");
        static MessageQueue errorQueue = MsmqUtility.GetQueue(@".\private$\errorQueue");
        static MessageQueue fatalQueue = MsmqUtility.GetQueue(@".\private$\fatalQueue");

        [Test]
        public void Test_Ex()
        {
            var msmqProcessor = new MsmqProcessor<Sample>(
                queue, errorQueue, fatalQueue, 4, TimeSpan.FromMilliseconds(100)
                );
            MsmqUtility.PurgeAll(msmqProcessor.Queues);

            Assert.AreEqual(0, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(0, fatalQueue.GetAllMessages().Length);

            var sampleSenderReceiver = new SampleSenderReceiver();
            msmqProcessor.Send(sampleSenderReceiver.GetItems());

            Assert.AreEqual(10, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(0, fatalQueue.GetAllMessages().Length);

            msmqProcessor.Receive(sampleSenderReceiver.ProcessItems_Ex);

            Assert.AreEqual(0, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(1, fatalQueue.GetAllMessages().Length);
        }
        [Test]
        public void Test_NoEx()
        {
            var msmqProcessor = new MsmqProcessor<Sample>(
                queue, errorQueue, fatalQueue, 4, TimeSpan.FromMilliseconds(100)
                );
            MsmqUtility.PurgeAll(msmqProcessor.Queues);

            Assert.AreEqual(0, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(0, fatalQueue.GetAllMessages().Length);

            var sampleSenderReceiver = new SampleSenderReceiver();
            msmqProcessor.Send(sampleSenderReceiver.GetItems());

            Assert.AreEqual(10, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(0, fatalQueue.GetAllMessages().Length);

            msmqProcessor.Receive(sampleSenderReceiver.ProcessItems_NoEx);

            Assert.AreEqual(0, queue.GetAllMessages().Length);
            Assert.AreEqual(0, errorQueue.GetAllMessages().Length);
            Assert.AreEqual(0, fatalQueue.GetAllMessages().Length);
        }
    }
}
