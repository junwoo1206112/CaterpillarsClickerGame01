using System;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class GameContainer
    {
        private static GameContainer _instance;
        public static GameContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameContainer();
                }
                return _instance;
            }
        }

        private readonly Dictionary<Type, object> _instances = new();

        public void Register<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : TInterface
        {
            _instances[typeof(TInterface)] = instance;
        }

        public void Register<T>(T instance)
        {
            _instances[typeof(T)] = instance;
        }

        public T Resolve<T>()
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered.");
        }

        public bool TryResolve<T>(out T service)
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                service = (T)instance;
                return true;
            }

            service = default;
            return false;
        }

        public bool IsRegistered<T>()
        {
            return _instances.ContainsKey(typeof(T));
        }

        public void Clear()
        {
            _instances.Clear();
        }

        public static void Reset()
        {
            _instance?.Clear();
            _instance = null;
        }
    }
}
