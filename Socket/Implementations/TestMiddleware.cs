using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class TestMiddleware : Middleware
    {
        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine("TestMiddleware invoke");

            await this.Next.Invoke(httpContext);
        }
    }
}
