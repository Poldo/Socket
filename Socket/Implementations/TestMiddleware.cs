using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class TestMiddleware : Middleware
    {
        private readonly TestInject testService;

        public TestMiddleware(TestInject testService) {
            this.testService = testService;
        }
        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"TestMiddleware invoke {testService.Name}");

            await this.NextInvoke(httpContext);
        }
    }
}
