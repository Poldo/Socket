using SocketApp.Models;
using SocketApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketApp
{
    class Startup
    {


        //internal static void ConfigureService() 
        //{
        //    applicationBuilder.GetType().GetConstructors().SelectMany(x => x.GetParameters()).Select(x => x.ParameterType)
        //    Activator.CreateInstance(typeof(string), new object[1] { new TestService() });
        //}

        internal static void ConfigureMiddleware(IApplicationBuilder applicationBuilder)  
        {
            applicationBuilder.Add<PoldoMiddleware>();
            applicationBuilder.Add<OdlopMiddleware>();
            applicationBuilder.Add<TestMiddleware>();
        }

    }
}
