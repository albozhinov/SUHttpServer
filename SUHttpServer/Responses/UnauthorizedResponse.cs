namespace SUHttpServer.Responses
{
    using SUHttpServer.HTTP;

    public class UnauthorizedResponse : Response
    {
        public UnauthorizedResponse(StatusCode statuscode) : base(statuscode)
        {
        }
    }
}
