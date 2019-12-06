using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocketApp.Models;

namespace SocketApp
{
    class OdlopMiddleware : Middleware
    {
        private readonly MyInject myInject;
        private readonly PoldoInject poldoInject;

        public OdlopMiddleware(MyInject myInject) {
            this.myInject = myInject;
        }
        public OdlopMiddleware(MyInject myInject, PoldoInject poldoInject)
        {
            this.myInject = myInject;
            this.poldoInject = poldoInject;
        }
        public override async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine($"OdlopMiddleware invoke {httpContext.Request.Verb}");
            Console.WriteLine($"OdlopMiddleware invoke {poldoInject.Name}");
            await this.NextInvoke(httpContext);
        }
    }
}
