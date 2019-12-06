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
        //public PoldoMiddleware() { }

        private TestInject TestService;
        public MyInject MyInject;
        public PoldoMiddleware(TestInject testService, MyInject myInject)
        {
            TestService = testService;
            MyInject = myInject;
        }

        

        public override async Task Invoke(HttpContext httpContext)
        {
            TestService.Name = "CIAO";
            Console.WriteLine($"PoldoMiddleware invoke {TestService.Name} {MyInject.Name}");
            await this.NextInvoke(httpContext);
        }

    }
}
