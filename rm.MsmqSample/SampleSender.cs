using System.Diagnostics;

namespace rm.MsmqSample
{
    public class SampleSender
    {
        [DebuggerStepThrough]
        public Sample[] GetItems()
        {
            var items = new Sample[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Sample() { Id = i, Name = "sample" + i };
            }
            return items;
        }
    }
}
