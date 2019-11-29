using SocketApp.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketApp
{
    class Program
    {
        private static readonly PoldoMiddlewareManager poldoMiddlewareManager = new PoldoMiddlewareManager();

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static Program() 
        {
            Startup.ConfigureMiddleware(poldoMiddlewareManager);
        }

        static void Main(string[] args)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEP = new IPEndPoint(ipHostInfo.AddressList[2], 12220);

            Console.WriteLine($"Local address and port : {localEP.ToString()}");

            Socket listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(10);

                while (true)
                {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Closing the listener...");

        }


        public static void AcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("Connessione Accettata");
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Signal the main thread to continue.  
            allDone.Set();

            // Create the state object.  
            HttpContext httpContext = new HttpContext();
            httpContext.workSocket = handler;
            httpContext.buffer = new byte[handler.Available];
            handler.BeginReceive(httpContext.buffer, 0, handler.Available, 0,
                new AsyncCallback(ReadCallback), httpContext);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            Console.WriteLine("Leggo");
            HttpContext httpContext  = (HttpContext)ar.AsyncState;
            Socket handler = httpContext.workSocket;
            int read = handler.EndReceive(ar);
            if (read == httpContext.buffer.Length)
            {
                string content = Encoding.ASCII.GetString(httpContext.buffer, 0, read);
                Request req = CreateRequestObject(content);
                httpContext.Request = req;
                Console.WriteLine($"Read {content.Length} bytes from socket.\n Data : {content}");
                Send(httpContext).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }


        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static async Task Send(HttpContext httpContext)
        {
            Socket handler = httpContext.workSocket;
           
            Response response = new Response
            {
                Version = "HTTP/1.1",
                Code = "200 OK"
            };
            response.Add("Server","PoldoServer");
            response.Add("Connection", "close");
            httpContext.Response = response;

           
            await poldoMiddlewareManager.Invoke(httpContext);

            StringBuilder str = new StringBuilder();
            str.Append("<!DOCTYPE html>\r\n");
            str.Append("<html class=\"client -nojs\" lang=\"it\" dir=\"ltr\">\r\n");
            str.Append("<head>\r\n");
            str.Append("<meta charset=\"UTF -8\" />\r\n");
            str.Append("<title>POLDO</title>\r\n");
            str.Append("<body>\r\n");
            str.Append("<div style=\"width: 200px; height: 200px; background: red; \"></div>");
            str.Append("</body>\r\n");
            str.Append("</html>");
            StringBuilder strBuilder = response.ToStringBuilder(str.ToString());
            byte[] byteData = Encoding.ASCII.GetBytes(strBuilder.ToString());
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }


        private static Request CreateRequestObject(string content)
        {
            if (content == null || content.Length == 0) { return new Request(); }
            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            string[] firstLineParts = lines[0].Split(" ");
            Request request = new Request
            {
                Verb = firstLineParts[0],
                Path = firstLineParts[1],
                Version = firstLineParts[2]
            };
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Length != 0)
                {
                    string[] parts = lines[i].Split(":", 2);
                    if (parts.Length == 2)
                    {
                        request.Add(parts[0], parts[1]);
                    }
                }
            }
            return request;
        }

    }
}
