namespace SUHttpServer.HTTP
{
    using SUHttpServer.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Request
    {
        public Method Method { get; private set; }

        public string Url { get; set; }

        public HeaderCollection Headers { get; private set; }

        public CookieCollection Cookies { get; set; }

        public string Body { get; private set; }

        public Session Session { get; private set; }

        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public IReadOnlyDictionary<string, string> Query { get; private set; }

        public static IServiceCollection ServiceCollection { get; private set; }

        private static Dictionary<string, Session> Sessions = new();

        private static Dictionary<string, string> ParseFormData(string bodyLines) => HttpUtility.UrlDecode(bodyLines)
                        .Split('&')
                        .Select(part => part.Split('='))
                        .Where(part => part.Length == 2)
                        .ToDictionary(
                                part => part[0],
                                part => part[1],
                                StringComparer.InvariantCultureIgnoreCase);


        public static Request Parse(string request, IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;

            var lines = request.Split("\r\n");
            var firstLine = lines.First().Split(" ");

            (string url, Dictionary<string, string> query) = ParseUrl(firstLine[1]);
            Method method = ParseMethod(firstLine[0]);

            HeaderCollection headers = ParseHeaders(lines.Skip(1));

            var cokies = ParseCookies(headers);

            var bodyLines = lines.Skip(headers.Count + 2);

            var body = string.Join("\r\n", bodyLines);

            var form = ParseForm(headers, body);

            var session = GetSession(cokies);

            return new Request()
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = body,
                Form = form,
                Cookies = cokies,
                Session = session,
                Query = query
            };
        }

        private static (string url, Dictionary<string, string> query) ParseUrl(string queryString)
        {
            string url = string.Empty;
            var query = new Dictionary<string, string>();
            var parts = queryString.Split("?", 2);

            if (parts.Length > 1)
            {
                var queryParams = parts[1].Split("&");

                foreach (var pair in queryParams)
                {
                    var param = pair.Split('=');

                    if (param.Length == 2)
                    {
                        query.Add(param[0], param[1]);
                    }
                }
            }

            url = parts[0];

            return (url, query);
        }

        private static Session GetSession(CookieCollection cokies)
        {
            var sessionId = cokies.Contains(Session.SessionCookieName) ? cokies[Session.SessionCookieName] : Guid.NewGuid().ToString();

            if (!Sessions.ContainsKey(sessionId))
            {
                Sessions[sessionId] = new Session(sessionId);
            }

            return Sessions[sessionId];
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookies = new CookieCollection();

            if (headers.Contains(Header.Cookie))
            {
                string cookieHeader = headers[Header.Cookie];

                string[] allCookies = cookieHeader.Split(';', StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookie in allCookies)
                {
                    string[] values = cookie.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    cookies.Add(values[0].Trim(), values[1].Trim());
                }
            }
            return cookies;
        }

        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType) && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }
            return formCollection;
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> lines)
        {
            var headers = new HeaderCollection();

            foreach (var line in lines)
            {
                if (line == String.Empty)
                {
                    break;
                }

                var parts = line.Split(":", 2);

                if (parts.Length != 2)
                {
                    throw new InvalidOperationException("Request headers is not valid");
                }


                headers.Add(parts[0], parts[1].Trim());
            }
            return headers;
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                //return Enum.Parse<Method>(method);
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"Method {method} is not supported");
            }
        }
    }
}
