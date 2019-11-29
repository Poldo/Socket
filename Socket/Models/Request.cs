using System.Collections.Generic;

namespace SocketApp.Models
{
    public class Request : Dictionary<string, string>
    {
        public string Verb;
        public string Path;
        public string Version;
    }
}
