MSMQ-component
==============

MSMQ component to send/receive items.

####Algorithm:

* `Send`: The items are sent to the `Queue`.
* `Receive`: The `Queue` is received in batches and each batch of items is processed using the `callback`. On exception, the whole batch of items is sent to the `ErrorQueue`. The `ErrorQueue` is received one item at a time and processed using the `callback`. On exception, the item is sent to the `FatalQueue`. The `FatalQueue` is not received at all and will contain items that cannot be processed.

Note: `Send` and `Receive` run in same thread.

####Example:

The MSMQ processor first sends sample items to the message queue and then processes them (in same thread).

```c#
// msmq processor
var msmqProcessor = new MsmqProcessor<Sample>(
    queue: MsmqUtility.GetQueue(@".\private$\queue"),
    errorQueue: MsmqUtility.GetQueue(@".\private$\errorQueue"),
    fatalQueue: MsmqUtility.GetQueue(@".\private$\fatalQueue"),
    queueBatchCount: 4,
    receiveTimeout: TimeSpan.FromMilliseconds(100)
    );
// sender/receiver
var sampleSenderReceiver = new SampleSenderReceiver();
// send to queue
msmqProcessor.Send(sampleSenderReceiver.GetItems());
// receive from queues using process callback
msmqProcessor.Receive(sampleSenderReceiver.ProcessItems);
```
