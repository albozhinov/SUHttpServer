using SUHttpServer.Controllers;
using SUHttpServer.HTTP;
using SUHttpServer.Startup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SUHttpServer.Startup.Controllers
{
    public class HomeController : Controller
    {

        private const string FileName = "content.txt";

        public HomeController(Request request)
            : base(request)
        {

        }

        public Response Index() => Text("Hello from the server!");

        public Response Redirect() => Redirect("https://softuni.org/");

        public Response Html() => View();

        public Response HtmlFormPost()
        {
            var name = Request.Form["Name"];
            var age = int.Parse(Request.Form["Age"]);

            var model = new FromViewModel()
            {
                Name = name,
                Age = age
            };

            return View(model);
        }       

        public Response Content() => View();

        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(FileName, 
                new string[] { "https://judge.softuni.org/", "https://softuni.org/" })
                .Wait();

            return File(FileName);
        }

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }
            var responses = await Task.WhenAll(downloads);

            var responsesString = string.Join(Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
        }

        public Response Cookies()
        {
            if (this.Request.Cookies.Any(c => c.Name != SUHttpServer.HTTP.Session.SessionCookieName))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<h1>Cookies</h1>");
                sb.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");


                foreach (var item in this.Request.Cookies)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{HttpUtility.HtmlEncode(item.Name)}</td>");
                    sb.Append($"<td>{HttpUtility.HtmlEncode(item.Value)}</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                return Html(sb.ToString());
            }

            var addCookies = new CookieCollection();

            addCookies.Add("My-Cookie", "My-Value");
            addCookies.Add("My-Second-Cookie", "My-Second-Value");

            return Html("<h1>Cookies set!</h1>", addCookies);
        }

        public Response Session()
        {
            string currentDateKey = "CurrentDate";

            var sessionExists = this.Request.Session.ContainsKey(currentDateKey);

            if (sessionExists)
            {
                var currDate = this.Request.Session[currentDateKey];

                return Text($"Stored date: {currDate}!");
            }

            return Text("Current date stored!");
        }
    }
}
