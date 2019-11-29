using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class OdlopMiddleware : Middleware
    {
        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"OdlopMiddleware invoke {httpContext.Request.Verb}");
            //await this.Next.Invoke(httpContext);
        }
    }
}
