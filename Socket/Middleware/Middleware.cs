
using SocketApp.Models;
using SocketApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocketApp
{
    public class StartChain : Middleware
    {
        public override async Task Invoke(HttpContext httpContext)
        {
            await this.NextInvoke(httpContext);
        }
    }
    public abstract class Middleware : IMiddleware, IService
    {
        private IMiddleware next;
        private IMiddleware Next => next ?? (next = new Factory().Create(Middlewares[Last++]) as IMiddleware);

        private int Last;
        private protected void SetLast(int last) => this.Last = last;
        private static List<Type> Middlewares = new List<Type>();
        public abstract Task Invoke(HttpContext httpContext);
        public async Task NextInvoke(HttpContext httpContext)
        {
            if (Next != null)
            {
                (Next as Middleware).SetLast(this.Last);
                await Next?.Invoke(httpContext);
            }
        }

        internal static void AddService<T>() where T : IMiddleware
        {
            Middlewares.Add(typeof(T));
        }
    }





}

