using System.Collections.Generic;

namespace SSDLMaintenanceTool.Helpers
{
    public class DescendingComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            // use the default comparer to do the original comparison for datetimes
            int ascendingResult = Comparer<T>.Default.Compare(x, y);

            // turn the result around
            return 0 - ascendingResult;
        }
    }
}
