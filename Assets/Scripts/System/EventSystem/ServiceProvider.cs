using System;
using System.Collections.Generic;

namespace Code.Service
{
    public sealed class ServiceProvider
    {
        private static ServiceProvider _instance;
        public static ServiceProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceProvider();
                }
                return _instance;
            }
            private set => _instance = value;
        }

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        private ServiceProvider() { }

        public void AddService<TServiceType>(IService service) where TServiceType : class, IService
        {
            if (!_services.ContainsKey(typeof(TServiceType)))
                _services.Add(typeof(TServiceType), service);
        }

        public bool RemoveService<TServiceType>() where TServiceType : class, IService
        {
            if (!_services.ContainsKey(typeof(TServiceType)))
                throw new KeyNotFoundException();

            return _services.Remove(typeof(TServiceType));
        }

        public bool ContainsService<TServiceType>() where TServiceType : class, IService
        {
            return _services.ContainsKey(typeof(TServiceType));
        }

        public TServiceType GetService<TServiceType>() where TServiceType : class, IService
        {
            return _services[typeof(TServiceType)] as TServiceType;
        }

        public void ClearAllServices()
        {
            _services.Clear();
        }

        public void ClearAllNonPersistanceServices()
        {
            List<Type> nonPersistanceServiceTypes = new List<Type>();
            foreach (KeyValuePair<Type, IService> service in _services)
            {
                if (!service.Value.IsPersistance)
                    nonPersistanceServiceTypes.Add(service.Key);
            }
            foreach (Type keyToRemove in nonPersistanceServiceTypes)
            {
                _services.Remove(keyToRemove);
            }
        }
    }
}
