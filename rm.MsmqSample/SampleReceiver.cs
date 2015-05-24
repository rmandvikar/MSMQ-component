using System;
using System.Collections.Generic;
using System.Diagnostics;
using rm.MsmqHelper;

namespace rm.MsmqSample
{
    /// <summary>
    /// Demo class to create and process items. Throws exception to simulate.
    /// </summary>
    public class SampleReceiver : IReceiver<Sample>
    {
        [DebuggerStepThrough]
        public void Receive(IEnumerable<Sample> items)
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
    }

    /// <summary>
    /// Demo class to create and process items. Does not throw exception.
    /// </summary>
    public class SampleReceiver_NoEx : IReceiver<Sample>
    {
        [DebuggerStepThrough]
        public void Receive(IEnumerable<Sample> items)
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
