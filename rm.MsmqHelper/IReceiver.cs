using System.Collections.Generic;

namespace rm.MsmqHelper
{
    /// <summary>
    /// Defines methods to receive items of type T.
    /// </summary>
    public interface IReceiver<T>
    {
        /// <summary>
        /// Receives items.
        /// </summary>
        void Receive(IEnumerable<T> items);
    }
}
