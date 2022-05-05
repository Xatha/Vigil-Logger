using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLibrary
{
    public static class ListUtils
    {
        public static T FindFirstOfType<T>(this List<object> list)
        {
            T result = default;

            foreach (var item in list)
            {
                if (item is T)
                {
                    result = (T)item;
                }
            }
            return result;
        }
    }
}
