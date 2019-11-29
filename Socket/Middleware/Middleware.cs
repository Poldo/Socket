
using SocketApp.Models;
using System.Threading.Tasks;

namespace SocketApp
{

    public abstract class Middleware : IMiddleware
    {
        protected IMiddleware Next;
        public abstract Task Invoke(HttpContext httpContext);
        public void SetSuccessor(IMiddleware next)
            => this.Next = next;
    }
}
