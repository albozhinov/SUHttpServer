using System.Collections;
using System.Collections.Generic;

namespace SUHttpServer.HTTP
{
    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> cookies;

        public CookieCollection()
        {
            cookies = new Dictionary<string, Cookie>();
        }

        public string this[string name] => cookies[name].Value;

        public void Add(string name, string value)
            => cookies[name] = new Cookie(name, value);

        public bool Contains(string name)
            => cookies.ContainsKey(name);

        public IEnumerator<Cookie> GetEnumerator()
        {
            return cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
