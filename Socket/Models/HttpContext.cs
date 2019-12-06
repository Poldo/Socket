using System;
using System.Net.Sockets;

namespace SocketApp.Models
{
    public class HttpContext
    {

        public Socket workSocket = null;
        public byte[] buffer;
        public Request Request;
        public Response Response;


        internal Request CreateRequestObject(string content)
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
