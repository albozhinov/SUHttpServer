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
                          .MapControllers())
                          .Start();
        }
    }
}
