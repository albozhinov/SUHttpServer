namespace SUHttpServer.Attributes
{
    using SUHttpServer.HTTP;

    public class HttpPostAttribute : HttpMethodAttribute
    {

        public HttpPostAttribute()
            : base(Method.Post)
        {

        }
    }
}
