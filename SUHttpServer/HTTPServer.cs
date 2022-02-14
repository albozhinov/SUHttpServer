namespace SUHttpServer
{
using SUHttpServer.Common;
using SUHttpServer.HTTP;
using SUHttpServer.Routing;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

    public class HTTPServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;
        private readonly RoutingTable routingTable;

        public readonly IServiceCollection ServiceCollection;
                

        public HTTPServer(string _ipAddress, int _port, Action<IRoutingTable> routingTableConfiguration)
        {
            ipAddress = IPAddress.Parse(_ipAddress);
            port = _port;            

            serverListener = new TcpListener(ipAddress, port);

            routingTableConfiguration(routingTable = new RoutingTable());
            ServiceCollection = new ServiceCollection();
        }

        public HTTPServer(int port, Action<IRoutingTable> routingTable)
           : this("127.0.0.1", port, routingTable)
        {

        }


        public HTTPServer(Action<IRoutingTable> routingTable)
           : this(8080, routingTable)
        {

        }

        public async Task Start()
        {
            serverListener.Start();

            Console.WriteLine($"Server is listening on port {port}");
            Console.WriteLine($"Listening for request");

            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();
                    var strRequest = await ReadRequest(networkStream);
                    var request = Request.Parse(strRequest, ServiceCollection);
                    //string content = "Hello world";
                    var response = this.routingTable.MatchRequest(request);

                    AddSession(request, response);
                    Console.WriteLine(strRequest);
                    await WriteResponse(networkStream, response);
                    connection.Close();
                });               
            }
            
        }

        private static void AddSession(Request request, Response response)
        {
            var sessionExists = request.Session
                                       .ContainsKey(Session.SessionCurrentDateKey);

            if (!sessionExists)
            {
                request.Session[Session.SessionCurrentDateKey] = DateTime.Now.ToString();

                response.Cookies.Add(Session.SessionCookieName, request.Session.Id.ToString());
            }
        }

        private async Task WriteResponse(NetworkStream networkStream, Response response)
        {

            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            if (response.FileContent != null)
            {
                responseBytes = responseBytes
                                .Concat(response.FileContent)
                                .ToArray();
            }

            await networkStream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            byte[] bufffer = new byte[1024];
            StringBuilder request = new StringBuilder();
            int totalBytes = 0;

            do
            {
                var bytesRead = await networkStream.ReadAsync(bufffer, 0, bufffer.Length);
                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }

                request.Append(Encoding.UTF8.GetString(bufffer, 0, bytesRead));

            } while (networkStream.DataAvailable);

            return request.ToString();
        }
    }
}
