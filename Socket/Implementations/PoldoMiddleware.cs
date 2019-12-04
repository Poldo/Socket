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
        //private TestInject TestService;
        //public PoldoMiddleware(TestInject testService) => this.TestService = testService;
        public override async Task Invoke(HttpContext httpContext)
        {
            //TestService.Name = "CIAO";
            //Console.WriteLine($"PoldoMiddleware invoke {TestService.Name}");
            Console.WriteLine($"PoldoMiddleware invoke");
            await this.NextInvoke(httpContext);
        }

    }
}
