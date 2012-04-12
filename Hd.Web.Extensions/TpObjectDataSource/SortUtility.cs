using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace MyUtil
{
    /// <summary>
    /// Helper class for sorting collections of custom objects.
    /// </summary>
    public static class SortUtility
    {
        /// <summary>
        /// Returns a sorted version of a provided collection. Function argument is not altered by the method.
        /// </summary>
        /// <param name="collection">Collection to be sorted.</param>
        /// <param name="propertyName">Name of a property to sort by.</param>
        /// <returns>Sorted collection. <i>Returned collection type will be List&lt;object&gt;.</i></returns>
        public static IEnumerable Sort(IEnumerable collection, string propertyName)
        {
            return SortInternal<object>(MakeList(collection), propertyName, false, false);
        }
        /// <summary>
        /// Returns a sorted version of a provided collection. Function argument is not altered by the method.
        /// </summary>
        /// <param name="collection">Collection to be sorted.</param>
        /// <param name="propertyName">Name of a property to sort by.</param>
        /// <param name="reverse">When true, the collection will be sorted in reverse order.</param>
        /// <returns>Sorted collection. <i>Returned collection type will be List&lt;object&gt;.</i></returns>
        public static IEnumerable Sort(IEnumerable collection, string propertyName, bool reverse)
        {
            return SortInternal<object>(MakeList(collection), propertyName, reverse, false);
        }

        private static List<object> MakeList(IEnumerable collection)
        {
            List<object> list = new List<object>();
            foreach (object o in collection)
            {
                list.Add(o);
            }
            return list;
        }

        /// <summary>
        /// Returns a sorted version of a provided collection. Function argument is not altered by the method.
        /// </summary>
        /// <typeparam name="T">Type of a generic IEnumerable.</typeparam>
        /// <param name="collection">Collection to be sorted.</param>
        /// <param name="propertyName">Name of a property to sort by.</param>
        /// <returns>Sorted collection.</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection, string propertyName)
        {
            List<T> collectionToSort = new List<T>(collection);
            return SortInternal<T>(collectionToSort, propertyName, false, false);
        }

        /// <summary>
        /// Returns a sorted version of a provided collection. Function argument is not altered by the method.
        /// </summary>
        /// <typeparam name="T">Type of a generic IEnumerable.</typeparam>
        /// <param name="collection">Collection to be sorted.</param>
        /// <param name="propertyName">Name of a property to sort by.</param>
        /// <param name="reverse">When true, the collection will be sorted in reverse order.</param>
        /// <returns>Sorted collection.</returns>
        public static List<T> Sort<T>(IEnumerable<T> collection, string propertyName, bool reverse)
        {
            List<T> collectionToSort = new List<T>(collection);
            return SortInternal<T>(collectionToSort, propertyName, reverse, false);
        }

        private static List<T> SortInternal<T>(List<T> collection, string propertyName, bool reverse, bool copySource)
        {
            List<T> collectionToSort;
            if (copySource)
            {
                collectionToSort = new List<T>(collection);
            }
            else
            {
                collectionToSort = collection;
            }
            collectionToSort.Sort(delegate(T o1, T o2) { return Compare(o1, o2, propertyName); });
            if (reverse)
            {
                collectionToSort.Reverse();
            }
            return collectionToSort;
        }
        private static int Compare(object o1, object o2, string propertyName)
        {
            PropertyInfo property = o1.GetType().GetProperty(propertyName);
            IComparable value1 = property.GetValue(o1, null) as IComparable;
            if (value1 == null)
            {
                throw new ArgumentException("It is not possible to sort " + o1.ToString() + " by " + propertyName + ". Property is not IComparable.");
            }
            IComparable value2 = property.GetValue(o2, null) as IComparable;
            return value1.CompareTo(value2);
        }
    }
}
