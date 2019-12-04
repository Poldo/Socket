using System;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class StaticFileMiddleware : Middleware
    {

        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"StaticFileMiddleware invoke {httpContext.Request.Path}");
            if (!httpContext.Request.Path.Contains(".ico"))
            {
                await this.NextInvoke(httpContext);
            }
        }
    }
}
