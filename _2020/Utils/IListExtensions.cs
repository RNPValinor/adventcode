using System.Collections.Generic;
using System.Linq;

namespace _2020.Utils
{
    public static class ListExtensions
    {
        public static T Pop<T>(this IList<T> list)
        {
            var value = list.FirstOrDefault();

            if (value != null)
            {
                list.RemoveAt(0);
            }

            return value;
        }
    }
}