using System;
using System.Diagnostics;

namespace rm.MsmqSample
{
    /// <summary>
    /// Demo class to create and process items.
    /// </summary>
    public class SampleSenderReceiver
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
        [DebuggerStepThrough]
        public void ProcessItems_Ex(Sample[] items)
        {
            Console.WriteLine("process ...");
            var throwEx = false;
            foreach (var item in items)
            {
                if (item.Id == 6)
                {
                    throwEx = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("id {0}: throwing ex ...", item.Id);
                    Console.ResetColor();
                    break;
                }
                Console.WriteLine("id {0}: done.", item.Id);
            }
            if (throwEx)
            {
                Console.WriteLine();
                throw new Exception("testing");
            }
            Console.WriteLine();
        }
        [DebuggerStepThrough]
        public void ProcessItems_NoEx(Sample[] items)
        {
            Console.WriteLine("process ...");
            foreach (var item in items)
            {
                Console.WriteLine("id {0}: done.", item.Id);
            }
            Console.WriteLine();
        }
    }
}
