using System;
using System.Collections.Generic;
using System.Text;

namespace SocketApp.Models
{
    public class Response: Dictionary<string, string>
    {
        public string Version;
        public string Code;

        internal StringBuilder ToStringBuilder(string body)
        {
            StringBuilder str = new StringBuilder();
            str.Append("HTTP/1.1 200 OK\r\n");
            str.Append("Server: Linux UPnP/1.0 (WDCR:Microsoft Windows NT 6.2.9200.0)\r\n");
            str.Append("Connection: close\r\n");
            str.Append("\r\n");
            str.Append("\r\n");
            str.Append(body);
            return str;
        }
    }
}
