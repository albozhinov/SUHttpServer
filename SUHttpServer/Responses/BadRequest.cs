namespace SUHttpServer.Responses
{
    using SUHttpServer.HTTP;

    public class BadRequest : Response
    {
        public BadRequest(StatusCode statuscode) : base(statuscode)
        {

        }
    }
}
