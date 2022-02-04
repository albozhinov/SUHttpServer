using SUHttpServer.Common;
using System.Collections.Generic;

namespace SUHttpServer.HTTP
{
    public class Session
    {
        //fields
        public const string SessionCookieName = "MyWebServerSID";

        public const string SessionCurrentDateKey = "CurrentDate";

        public const string SessionUserKey = "AutenticatedUserKey";

        private Dictionary<string, string> data;

        //Constructor
        public Session(string id)
        {
            Guard.AgainstNull(id, nameof(id));

            this.Id = id;

            this.data = new Dictionary<string, string>();
        }

        // Properties
        public string Id { get; init; }


        //Methods
        public string this[string key]
        {
            get => this.data[key];
            set => this.data[key] = value;
        }

        public bool ContainsKey(string key) => this.data.ContainsKey(key);

        public void Clear() => this.data.Clear();
    }
}
