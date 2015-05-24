using System;
using rm.MsmqHelper;

namespace rm.MsmqSample
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoMsmq();
        }

        private static void DemoMsmq()
        {
            // msmq processor
            var msmqProcessor = new MsmqProcessor<Sample>(
                MsmqUtility.GetQueue(@".\private$\queue"),
                MsmqUtility.GetQueue(@".\private$\errorQueue"),
                MsmqUtility.GetQueue(@".\private$\fatalQueue"),
                4,
                TimeSpan.FromMilliseconds(100),
                new SampleReceiver()
                );
            // start clean for demo
            MsmqUtility.PurgeAll(msmqProcessor.Queues);
            // send to queue
            msmqProcessor.Send(new SampleSender().GetItems());
            // receive from queues
            msmqProcessor.Receive();
            // send to queue
            msmqProcessor.Send(new SampleSender().GetItems());
            // receive from queues
            msmqProcessor.Receive();
        }
    }
}
