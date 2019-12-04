using SocketApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{

    public interface IMiddleware
    {

        Task Invoke(HttpContext httpContext);
        Task NextInvoke(HttpContext httpContext);
    }
}
