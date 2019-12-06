using SocketApp.Services;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

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
            List<IService> cons = GetConstructorParams(service.Type);
            switch (service.FactoryType)
            {
                case FactoryType.Singleton:
                    if (!Singletons.ContainsKey(service.Type))
                    {
                        if (cons.Count != 0)
                        {
                            Singletons.Add(service.Type, (IService)Activator.CreateInstance(service.Type, cons.ToArray()));
                        }
                        else
                        {
                            Singletons.Add(service.Type, (IService)Activator.CreateInstance(service.Type));
                        }
                    }
                    return Singletons[service.Type];
                case FactoryType.FlyWeight:
                    if (!FlyWeights.ContainsKey(service.Type))
                    {
                        if (cons.Count != 0)
                        {
                            FlyWeights.Add(service.Type, (IService)Activator.CreateInstance(service.Type, cons.ToArray()));
                        }
                        else
                        {
                            FlyWeights.Add(service.Type, (IService)Activator.CreateInstance(service.Type));
                        }
                    }
                    return FlyWeights[service.Type];
                case FactoryType.Factory:
                    if (cons.Count != 0)
                    {
                        return (IService)Activator.CreateInstance(service.Type, cons.ToArray());
                    }
                    return (IService)Activator.CreateInstance(service.Type);
            }
            return null;
        }
        private static Dictionary<Type, ParameterInfo[]> ConstructorsParam = new Dictionary<Type, ParameterInfo[]>();
        private static readonly object TrafficLight = new object();
        private static ParameterInfo[] Instance(Type type)
        {
            if (!ConstructorsParam.ContainsKey(type))
            {
                lock (TrafficLight)
                {
                    if (!ConstructorsParam.ContainsKey(type))
                    {
                        ParameterInfo[] parameters = type.GetConstructors()
                        .Where(x => x.GetParameters().Where(y => CheckIfServiceExists(y.ParameterType)).Count() == x.GetParameters().Count())
                        .OrderByDescending(x => x.GetParameters().Count()).FirstOrDefault().GetParameters();
                        ConstructorsParam.Add(type, parameters);
                    }
                }
            }
            return ConstructorsParam[type];
        }
        private List<IService> GetConstructorParams(Type type)
        {
            List<IService> cons = new List<IService>();
            foreach (ParameterInfo parameterInfo in Instance(type))
                cons.Add(Create(parameterInfo.ParameterType));
            return cons;
        }

        private static bool CheckIfServiceExists(Type parameterType) => ServiceExtensions.GetServiceByType(parameterType) != null;

    }



    public enum FactoryType
    {
        Singleton,
        FlyWeight,
        Factory
    }
}
