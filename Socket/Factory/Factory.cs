using SocketApp.Services;
using System;
using System.Collections.Generic;

namespace SocketApp
{
    class Factory
    {
        private static Dictionary<Type, IService> Singletons = new Dictionary<Type, IService>();
        private Dictionary<Type, IService> FlyWeights = new Dictionary<Type, IService>();

        public IService Create(Type type)
        {
            if (type == null)
                return null;
            
            ServiceWrapper service = ServiceExtensions.GetServiceByType(type);
            if (service == null)
                throw new Exception("DIP non funzionante.");
            switch (service.FactoryType)
            {
                case FactoryType.Singleton:
                    if (!Singletons.ContainsKey(service.Type))
                    {
                        //Singletons.Add(service.Type, (IService)Activator.CreateInstance(service.Type, new TestInject()));
                        Singletons.Add(service.Type, (IService)Activator.CreateInstance(service.Type));
                    }
                    return Singletons[service.Type];
                case FactoryType.FlyWeight:
                    if (!FlyWeights.ContainsKey(service.Type))
                    {
                        //FlyWeights.Add(service.Type, (IService)Activator.CreateInstance(service.Type, new TestInject()));
                        FlyWeights.Add(service.Type, (IService)Activator.CreateInstance(service.Type));
                    }
                    return FlyWeights[service.Type];
                case FactoryType.Factory:
                    //return (IService)Activator.CreateInstance(service.Type, new TestInject());
                    return (IService)Activator.CreateInstance(service.Type);
            }
            return null;
        }
        //public object[] GetConstructor(Type type) 
        //{
            
        //}
    }



    public enum FactoryType
    {
        Singleton,
        FlyWeight,
        Factory
    }
}
