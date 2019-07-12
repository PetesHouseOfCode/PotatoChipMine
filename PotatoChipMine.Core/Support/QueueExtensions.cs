using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Support
{
    public static class QueueExtensions
    {
        /// <summary>
        /// Attempts to remove and return the object at the beginning of the <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="result">
        /// When this method returns, if the operation was successful, <paramref name="result"/> contains the
        /// object removed. If no object was available to be removed, the value is unspecified.
        /// </param>
        /// <returns>true if an element was removed and returned from the beginning of the <see cref="Queue{T}"/>
        /// successfully; otherwise, false.</returns>
        public static bool TryDequeue<T>(this Queue<T> queue, out T result)
        {
            if (queue.Count <= 0)
            {
                result = default;
                return false;
            }

            result = queue.Dequeue();
            return true;
        }
    }
}
