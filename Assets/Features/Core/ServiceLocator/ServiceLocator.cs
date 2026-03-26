using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Features.Core.ServiceLocatorScript
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
                throw new InvalidOperationException($"Service of type {type.Name} already registered.");

            _services[type] = service;
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service))
                return (T)service;

            throw new InvalidOperationException($"Service of type {type.Name} not registered.");
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            try
            {
                service = Get<T>();
                return true;
            }
            catch
            {
                service = null;
                return false;
            }
        }

        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            _services.Remove(type);
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}
