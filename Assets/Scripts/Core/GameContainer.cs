using System;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class GameContainer
    {
        private readonly Dictionary<Type, object> _instances = new();

        public void Register<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : TInterface
        {
            _instances[typeof(TInterface)] = instance;
        }

        public T Resolve<T>()
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
        }

        public bool IsRegistered<T>()
        {
            return _instances.ContainsKey(typeof(T));
        }

        public void Clear()
        {
            _instances.Clear();
        }
    }
}
