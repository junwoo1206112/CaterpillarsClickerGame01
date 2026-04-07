using UnityEngine;
using System.Collections.Generic;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class TouchFunctionPool
    {
        private readonly Dictionary<string, Queue<ITouchFunction>> _pool = new();
        private readonly Dictionary<string, ITouchFunction> _activeFunctions = new();
        private readonly List<ITouchFunction> _availableFunctions = new();

        public int TotalPooled => _pool.Count;
        public int ActiveCount => _activeFunctions.Count;

        public void Initialize(ITouchFunction prototype, int initialCount = 5)
        {
            string key = prototype.FunctionName;

            if (!_pool.ContainsKey(key))
            {
                _pool[key] = new Queue<ITouchFunction>();

                for (int i = 0; i < initialCount; i++)
                {
                    ITouchFunction instance = prototype.Clone();
                    _pool[key].Enqueue(instance);
                    _availableFunctions.Add(instance);
                }

                Debug.Log($"[TouchFunctionPool] Initialized {initialCount} instances of {key}");
            }
        }

        public ITouchFunction Get(string functionName)
        {
            if (!_pool.ContainsKey(functionName) || _pool[functionName].Count == 0)
            {
                Debug.LogWarning($"[TouchFunctionPool] No available instance of {functionName}. Creating new one.");
                return CreateNew(functionName);
            }

            ITouchFunction function = _pool[functionName].Dequeue();
            _activeFunctions[functionName] = function;
            _availableFunctions.Remove(function);

            Debug.Log($"[TouchFunctionPool] Retrieved {functionName}. Remaining: {_pool[functionName].Count}");

            return function;
        }

        public void Return(ITouchFunction function)
        {
            string key = function.FunctionName;

            if (_activeFunctions.ContainsKey(key))
            {
                _activeFunctions.Remove(key);
            }

            function.Reset();

            if (_pool.ContainsKey(key))
            {
                _pool[key].Enqueue(function);
                _availableFunctions.Add(function);
                Debug.Log($"[TouchFunctionPool] Returned {key}. Pool size: {_pool[key].Count}");
            }
            else
            {
                _pool[key] = new Queue<ITouchFunction>();
                _pool[key].Enqueue(function);
                _availableFunctions.Add(function);
            }
        }

        public void ReturnAll(string functionName)
        {
            if (_activeFunctions.ContainsKey(functionName))
            {
                var function = _activeFunctions[functionName];
                Return(function);
            }
        }

        public ITouchFunction GetAvailable(System.Func<ITouchFunction, bool> predicate)
        {
            foreach (var function in _availableFunctions)
            {
                if (predicate(function))
                {
                    return function;
                }
            }
            return null;
        }

        public List<ITouchFunction> GetAllActive()
        {
            return new List<ITouchFunction>(_activeFunctions.Values);
        }

        public bool IsActive(string functionName)
        {
            return _activeFunctions.ContainsKey(functionName);
        }

        public void Clear()
        {
            foreach (var kvp in _pool)
            {
                kvp.Value.Clear();
            }
            _pool.Clear();
            _activeFunctions.Clear();
            _availableFunctions.Clear();

            Debug.Log("[TouchFunctionPool] Cleared all pools");
        }

        public void Expand(string functionName, int count)
        {
            if (!_pool.ContainsKey(functionName))
            {
                _pool[functionName] = new Queue<ITouchFunction>();
            }

            var prototype = GetPrototype(functionName);
            if (prototype != null)
            {
                for (int i = 0; i < count; i++)
                {
                    ITouchFunction instance = prototype.Clone();
                    _pool[functionName].Enqueue(instance);
                    _availableFunctions.Add(instance);
                }
                Debug.Log($"[TouchFunctionPool] Expanded {functionName} by {count}");
            }
        }

        private ITouchFunction CreateNew(string functionName)
        {
            ITouchFunction function = functionName switch
            {
                "Bonus Click" => new BonusTouchFunction(),
                "Speed Boost" => new SpeedBoostFunction(),
                "Critical Click" => new CriticalTouchFunction(),
                _ => new BonusTouchFunction()
            };

            _activeFunctions[functionName] = function;
            return function;
        }

        private ITouchFunction GetPrototype(string functionName)
        {
            return functionName switch
            {
                "Bonus Click" => new BonusTouchFunction(),
                "Speed Boost" => new SpeedBoostFunction(),
                "Critical Click" => new CriticalTouchFunction(),
                _ => null
            };
        }

        public void LogStatus()
        {
            Debug.Log("=== TouchFunctionPool Status ===");
            foreach (var kvp in _pool)
            {
                Debug.Log($"  {kvp.Key}: {kvp.Value.Count} available");
            }
            Debug.Log($"  Active: {_activeFunctions.Count}");
            Debug.Log("================================");
        }
    }
}
