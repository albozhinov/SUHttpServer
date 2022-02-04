using SUHttpServer.HTTP;
using System;

namespace SUHttpServer.Routing
{
    public interface IRoutingTable
    {
        IRoutingTable Map(Method method, string path, Func<Request, Response> responseFunction);
    }
}
