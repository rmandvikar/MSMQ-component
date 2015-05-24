using System;
using System.Diagnostics;

namespace rm.MsmqSample
{
    public class SampleSender
    {
        [DebuggerStepThrough]
        public Sample[] GetItems()
        {
            Console.WriteLine("get ...");
            var items = new Sample[10];
            for (int i = 0; i < items.Length; i++)
            {
                var id = i;
                items[i] = new Sample() { Id = id, Name = "sample" + id };
            }
            return items;
        }
    }
}
