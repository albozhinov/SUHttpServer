namespace SUHttpServer.Responses
{
    using SUHttpServer.HTTP;

    public class NotFoundResponse : Response
    {
        public NotFoundResponse(StatusCode statuscode) : base(statuscode)
        {
        }
    }
}
