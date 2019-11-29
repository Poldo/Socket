using System.Net.Sockets;

namespace SocketApp.Models
{
    public class HttpContext
    {

        public Socket workSocket = null;
        public byte[] buffer;
        public Request Request;
        public Response Response;
        
    }
}
