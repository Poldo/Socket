using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class MvcMiddleware : Middleware
    {

        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"MvcMiddleware invoke {httpContext.Request.Path}");
            Socket handler = httpContext.workSocket;

            Response response = new Response
            {
                Version = "HTTP/1.1",
                Code = "200 OK"
            };
            response.Add("Server", "PoldoServer");
            response.Add("Connection", "close");
            httpContext.Response = response;

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
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);

            //await this.NextInvoke(httpContext);

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
    }
}
