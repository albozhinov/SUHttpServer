using System;
using System.Collections.Generic;

namespace SUHttpServer.Common
{
    public static class Guard
    {
        public static void AgainstNull(object value, string name = null)
        {
            if (value == null)
            {
                name ??= "Value";

                throw new ArgumentNullException($"{name} can not be null");
            }
        }

        public static void AgainstDuplicatedKey<T,V>(IDictionary<T,V> dictionary, T key, string name)
        {
            if (dictionary.ContainsKey(key))
            {
                throw new ArgumentException($"{name} already contains key {key.ToString()}");   
            }
        }
    }
}
