using SocketApp.Models;
using SocketApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{
    public class PoldoMiddleware : Middleware
    {
        //private TestService TestService;
        //public PoldoMiddleware(TestService testService) => this.TestService = testService;
        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine("PoldoMiddleware invoke");
            await this.Next.Invoke(httpContext);
        }

    }
}
