namespace SUHttpServer.Attributes
{
    using SUHttpServer.HTTP;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute()
            : base(Method.Get)
        {

        }
    }
}
