using System;
using System.Collections.Generic;
using System.Text;

namespace SocketApp.Services

{
    public interface IService
    {
    }
    public class Service : IService { }
    public static class ServiceExtensions
    {

        private static List<ServiceWrapper> services = new List<ServiceWrapper>();


        public static ServiceWrapper GetServiceByType(Type type)
        {
            return services.Find(x => x.Type == type);
        }


        public static void AddScoped<T>(this IService service) where T : IService
        {
            services.Add(new ServiceWrapper
            {
                Type = typeof(T),
                FactoryType = FactoryType.FlyWeight
            }
            );
        }

        public static void AddSingleton<T>(this IService service) where T : IService
        {
            services.Add(new ServiceWrapper
            {
                Type = typeof(T),
                FactoryType = FactoryType.Singleton
            }
            );
        }

        public static void AddTransient<T>(this IService service) where T : IService
        {
            services.Add(new ServiceWrapper
            {
                Type = typeof(T),
                FactoryType = FactoryType.Factory
            }
            );
        }
    }

    public class ServiceWrapper
    {
        internal Type Type;
        internal FactoryType FactoryType;
    }
}
