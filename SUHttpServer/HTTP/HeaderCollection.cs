namespace SUHttpServer.HTTP
{
    using System.Collections;
    using System.Collections.Generic;

    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> headers = new Dictionary<string, Header>();

        public string this[string name] => this.headers[name].Value;

        public int Count => headers.Count;

        public bool Contains(string name) => this.headers.ContainsKey(name);

        public void Add (string name, string value)
        {
            if (headers.ContainsKey(name))
            {
                headers[name].Name = name;
                headers[name].Value = value;
                return;
            }

            headers.Add(name, new Header(name, value));
        }

        public IEnumerator<Header> GetEnumerator() => this.headers.Values.GetEnumerator();      

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
    }
}
