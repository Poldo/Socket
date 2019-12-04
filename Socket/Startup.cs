using SocketApp.Services;

namespace SocketApp
{
    class Startup
    {


        internal static void ConfigureServices(IService service)
        {
            service.AddScoped<PoldoMiddleware>();
            service.AddSingleton<OdlopMiddleware>();
            service.AddTransient<TestMiddleware>();
            //service.AddScoped<TestInject>();
            service.AddSingleton<StaticFileMiddleware>();
            service.AddScoped<MvcMiddleware>();
        }




        internal static void ConfigureMiddleware(IApplicationBuilder applicationBuilder)  
        {
            applicationBuilder.Add<StaticFileMiddleware>();
            applicationBuilder.Add<PoldoMiddleware>();
            applicationBuilder.Add<OdlopMiddleware>();
            applicationBuilder.Add<TestMiddleware>();
            applicationBuilder.Add<MvcMiddleware>();
        }

    }
}
