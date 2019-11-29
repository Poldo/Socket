using SocketApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{
    interface IApplicationBuilder 
    {
        void Add<T>() where T : IMiddleware, new();
    }
    class PoldoMiddlewareManager : Middleware, IApplicationBuilder
    {

        private IMiddleware Last;

        public void Add<T>() where T : IMiddleware, new()
        {
            T t = new T();
            if (Last == null)
                SetSuccessor(t);
            else
                Last.SetSuccessor(t);
            Last = t;
        }

        public override async Task Invoke(HttpContext httpContext)
        {
            await Next.Invoke(httpContext);
        }

    }
}
