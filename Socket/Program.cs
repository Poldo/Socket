using SocketApp.Models;
using SocketApp.Services;
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

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static Program()
        {

            Startup.ConfigureServices(new Service());
            Startup.ConfigureMiddleware(new MiddlewareManager());
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
            HttpContext httpContext = (HttpContext)ar.AsyncState;
            Socket handler = httpContext.workSocket;
            int read = handler.EndReceive(ar);
            if (read == httpContext.buffer.Length)
            {
                string content = Encoding.ASCII.GetString(httpContext.buffer, 0, read);
                Request req = httpContext.CreateRequestObject(content);
                httpContext.Request = req;
                Console.WriteLine($"Read {content.Length} bytes from socket.\n Data : {content}");
                Send(httpContext).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private static async Task Send(HttpContext httpContext)
        {
            await new StartChain().NextInvoke(httpContext);
        }

    }
}
