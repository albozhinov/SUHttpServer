namespace SUHttpServer.Startup.Controllers
{
using SUHttpServer.Controllers;
    using SUHttpServer.HTTP;

    public class UserController : Controller
    {
        private const string Username = "user";

        private const string Password = "user123";

        public UserController(Request request)
            : base(request)
        {

        }

        public Response Login() => View();

        public Response LogInUser()
        {
            Request.Session.Clear();

            var userNameMatches = Request.Form["Username"] == Username;
            var passwordMathces = Request.Form["Password"] == Password;

            if (userNameMatches && passwordMathces)
            {
                if (!Request.Session.ContainsKey(Session.SessionUserKey))
                {
                    Request.Session[Session.SessionUserKey] = "MyUserId";
                var cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName, Request.Session.Id);

                return Html("<h3>Logged successfully!<h3/>", cookies);
                }

            return Html("<h3>Logged successfully!<h3/>");
            }

            return Redirect("/Login");
        }

        public Response Logout()
        {
            Request.Session.Clear();


            return Html("<h3>Logged out successfully!</h3>");
        }

        public Response GetUserData()
        {
            if (Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Html($"<p>Currently logged-in user is " + $"with username <strong>'{Username}'</strong>!</p>");
            }

            return Redirect("/Login");
        }

    }
}
