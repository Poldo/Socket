using SocketApp.Models;
using SocketApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{
    interface IApplicationBuilder
    {
        void Add<T>() where T : IMiddleware;

    }

    class MiddlewareManager : IApplicationBuilder
    {
        public void Add<T>() where T : IMiddleware => Middleware.AddService<T>();
    }
}
