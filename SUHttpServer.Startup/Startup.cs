namespace SUHttpServer.Startup
{
    using SUHttpServer.Routing;
    using SUHttpServer.Startup.Controllers; 
    using System.Threading.Tasks;

    public class Startup
    {             

        public static async Task Main()
        {

            await new HTTPServer(routes => routes
                          .MapGet<HomeController>("/", c => c.Index())
                          .MapGet<HomeController>("/Redirect", c => c.Redirect())
                          .MapGet<HomeController>("/HTML", c => c.Html())
                          .MapPost<HomeController>("/HTML", c => c.HtmlFormPost())
                          .MapGet<HomeController>("/Content", c => c.Content())
                          .MapPost<HomeController>("/Content", c => c.DownloadContent())
                          .MapGet<HomeController>("/Cookies", c => c.Cookies())
                          .MapGet<HomeController>("/Session", c => c.Session())
                          .MapGet<UserController>("/Login", c => c.Login())
                          .MapPost<UserController>("/Login", c => c.LogInUser())
                          .MapGet<UserController>("/Logout", c => c.Logout())
                          .MapGet<UserController>("/UserProfile", c => c.GetUserData()))
            .Start();
        }
    }
}
