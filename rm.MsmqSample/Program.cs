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
                TimeSpan.FromMilliseconds(100)
                );
            // start clean for demo
            MsmqUtility.PurgeAll(msmqProcessor.Queues);
            // sender/receiver
            var sampleSenderReceiver = new SampleSenderReceiver();
            // send to queue
            msmqProcessor.Send(sampleSenderReceiver.GetItems());
            // receive from queues
            msmqProcessor.Receive(sampleSenderReceiver.ProcessItems_Ex);
        }
    }
}
