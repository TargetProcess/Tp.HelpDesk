using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Hd.Web.Extensions
{
    public class StateKeeper : IStateKeeper
    {
        private static Hashtable cache = new Hashtable();
        public object GetValue(object key)
        {
            return cache[key];
        }

        public void SetValue(object key, object value)
        {
            if (!cache.Contains(key))
                cache.Add(key, value);

            cache[key] = value;
        }

        public static void Clear()
        {
            cache.Clear();
        }
    }
}
