using System.Collections.Generic;

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
