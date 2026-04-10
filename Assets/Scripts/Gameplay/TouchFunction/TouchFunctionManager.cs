using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using ClickerGame.Gameplay.TouchFunction;

namespace ClickerGame.Gameplay
{
    public class TouchFunctionManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TouchFunctionListSO _functionDataList;
        [SerializeField] private int _initialListCapacity = 16;

        [Header("Events")]
        public UnityEvent<string> OnFunctionActivated;
        public UnityEvent<string> OnFunctionExpired;
        public UnityEvent<int> OnListExpanded;

        private List<ITouchFunction> _functions = new();
        private readonly List<ITouchFunction> _reusableFunctions = new();
        private readonly List<ITouchFunction> _temporaryFunctions = new();
        private readonly List<TouchFunctionDataModel> _registeredData = new();
        private float _currentMultiplier = 1f;
        private float _lastActivateTime = -999f;

        public float CurrentMultiplier => _currentMultiplier;
        public int ActiveFunctionCount => _functions.Count;
        public int ReusableFunctionCount => _reusableFunctions.Count;
        public int MaxCapacity => _functions.Capacity;
        public bool CanExpand => true;

        private void Awake()
        {
            if (OnFunctionActivated == null)
                OnFunctionActivated = new UnityEvent<string>();

            if (OnFunctionExpired == null)
                OnFunctionExpired = new UnityEvent<string>();

            InitializeFromData();
        }

        private void InitializeFromData()
        {
            _functions = new List<ITouchFunction>(_initialListCapacity);
            _reusableFunctions.Clear();
            _temporaryFunctions.Clear();
            _registeredData.Clear();

            if (_functionDataList == null || _functionDataList.Functions == null)
            {
                Debug.LogWarning("[TouchFunctionManager] No data found. Creating default functions.");
                CreateDefaultFunctions();
                return;
            }

            foreach (var data in _functionDataList.Functions)
            {
                _registeredData.Add(data);
            }

            var functions = TouchFunction.TouchFunctionFactory.CreateList(_functionDataList.Functions);

            foreach (var function in functions)
            {
                AddFunction(function);
            }

            Debug.Log($"[TouchFunctionManager] Initialized {_functions.Count} functions from data (Capacity: {_functions.Capacity})");
        }

        private void CreateDefaultFunctions()
        {
            var defaultData = new List<TouchFunctionDataModel>
            {
                new TouchFunctionDataModel
                {
                    ID = "TOUCH_001",
                    FunctionName = "Bonus Click",
                    FunctionType = "BonusClick",
                    TriggerCount = 50,
                    Multiplier = 1.33f,
                    Duration = 0f,
                    Cooldown = 0f,
                    CriticalChance = 0f,
                    IsActive = false,  // ← 수정: 기본적으로 비활성화
                    IsReusable = true
                },
                new TouchFunctionDataModel
                {
                    ID = "TOUCH_002",
                    FunctionName = "Speed Boost",
                    FunctionType = "SpeedBoost",
                    TriggerCount = 0,
                    Multiplier = 2f,
                    Duration = 60f,
                    Cooldown = 300f,
                    CriticalChance = 0f,
                    IsActive = false,  // ← 수정: 기본적으로 비활성화
                    IsReusable = true
                }
            };

            var functions = TouchFunction.TouchFunctionFactory.CreateList(defaultData);

            foreach (var function in functions)
            {
                AddFunction(function);
            }
        }

        public void AddFunction(ITouchFunction function)
        {
            if (!_functions.Contains(function))
            {
                _functions.Add(function);

                if (function.IsReusable)
                {
                    if (!_reusableFunctions.Contains(function))
                        _reusableFunctions.Add(function);
                }
                else
                {
                    if (!_temporaryFunctions.Contains(function))
                        _temporaryFunctions.Add(function);
                }

                Debug.Log($"[TouchFunctionManager] Added: {function.FunctionName} (Reusable: {function.IsReusable}, Total: {_functions.Count})");
            }
        }

        public void AddFunctionFromData(TouchFunctionDataModel data)
        {
            if (data == null)
            {
                Debug.LogWarning("[TouchFunctionManager] Cannot add null data");
                return;
            }

            if (_registeredData.Exists(d => d.ID == data.ID))
            {
                Debug.Log($"[TouchFunctionManager] Function {data.ID} already registered");
                return;
            }

            _registeredData.Add(data);
            var function = TouchFunction.TouchFunctionFactory.Create(data);
            AddFunction(function);
        }

        public void RemoveFunction(string functionName)
        {
            var function = _functions.Find(f => f.FunctionName == functionName);
            if (function != null)
            {
                _functions.Remove(function);
                _reusableFunctions.Remove(function);
                _temporaryFunctions.Remove(function);
                Debug.Log($"[TouchFunctionManager] Removed: {functionName}");
            }
        }

        public void RemoveFunctionById(string functionId)
        {
            var data = _registeredData.Find(d => d.ID == functionId);
            if (data != null)
            {
                RemoveFunction(data.FunctionName);
                _registeredData.Remove(data);
                Debug.Log($"[TouchFunctionManager] Removed by ID: {functionId}");
            }
        }

        public void LoadFromData(TouchFunctionListSO dataList)
        {
            _functionDataList = dataList;
            InitializeFromData();
        }

        public void LoadFromDataAsync(string excelPath)
        {
            StartCoroutine(LoadDataCoroutine(excelPath));
        }

        private System.Collections.IEnumerator LoadDataCoroutine(string excelPath)
        {
            var dataService = new ExcelDataService<TouchFunctionDataModel>();
            var task = dataService.LoadAsync(excelPath);

            while (!task.IsCompleted)
            {
                yield return null;
            }

            var dataList = task.Result;

            if (dataList != null && dataList.Count > 0)
            {
                _functionDataList = ScriptableObject.CreateInstance<TouchFunctionListSO>();
                _functionDataList.Functions = dataList;
                InitializeFromData();
            }
        }

        public TouchEffect ProcessTouch(int touchCount)
        {
            TouchEffect totalEffect = new TouchEffect(0, 1f, 0f, "");
            float currentTime = Time.time;

            for (int i = 0; i < _functions.Count; i++)
            {
                var function = _functions[i];

                if (function.CanActivate(touchCount, _lastActivateTime))
                {
                    TouchEffect effect = function.Activate();

                    if (!string.IsNullOrEmpty(effect.EffectName))
                    {
                        totalEffect.BonusClicks += effect.BonusClicks;
                        totalEffect.ScoreMultiplier *= effect.ScoreMultiplier;
                        totalEffect.EffectName = effect.EffectName;

                        OnFunctionActivated?.Invoke(effect.EffectName);
                        Debug.Log($"[TouchFunction] {effect.EffectName} activated! (List: {_functions.Count})");
                    }
                }
            }

            UpdateFunctions(currentTime);

            ReuseTemporaryFunctions();

            return totalEffect;
        }

        private void UpdateFunctions(float currentTime)
        {
            _currentMultiplier = 1f;

            foreach (var function in _functions)
            {
                if (function is TouchFunction.SpeedBoostFunction speedBoost)
                {
                    speedBoost.Update(Time.deltaTime);

                    if (speedBoost.IsActive)
                    {
                        _currentMultiplier = speedBoost.Multiplier;
                    }
                }
                else if (function is TouchFunction.CriticalTouchFunction critical)
                {
                    critical.Update(Time.deltaTime);
                }
            }
        }

        private void ReuseTemporaryFunctions()
        {
            for (int i = _temporaryFunctions.Count - 1; i >= 0; i--)
            {
                var function = _temporaryFunctions[i];
                function.Reset();

                if (!_functions.Contains(function))
                {
                    _functions.Add(function);
                }
            }
        }

        public void ExpandList(int count)
        {
            int newCapacity = _functions.Count + count;
            _functions.Capacity = newCapacity;
            OnListExpanded?.Invoke(newCapacity);
            Debug.Log($"[TouchFunctionManager] List expanded to capacity: {newCapacity}");
        }

        public void EnsureCapacity(int minCapacity)
        {
            if (_functions.Capacity < minCapacity)
            {
                ExpandList(minCapacity - _functions.Capacity);
            }
        }

        public void ActivateSpeedBoost()
        {
            float currentTime = Time.time;

            var speedBoost = _functions.Find(f => f is TouchFunction.SpeedBoostFunction) as TouchFunction.SpeedBoostFunction;

            if (speedBoost != null && speedBoost.CanActivate(0, currentTime))
            {
                speedBoost.Activate();
                OnFunctionActivated?.Invoke("Speed Boost");
                Debug.Log($"[SpeedBoost] Activated! 2x points for 20 seconds!");
            }
            else if (speedBoost != null)
            {
                float cooldownRemaining = speedBoost.GetCooldownRemaining(currentTime);
                Debug.Log($"[SpeedBoost] On cooldown. {cooldownRemaining:F0}s remaining");
            }
            else
            {
                Debug.LogWarning("[SpeedBoost] SpeedBoost function not found!");
            }
        }

        public void ActivateCriticalMode(float chance = 0.1f, float duration = 0f)
        {
            var critical = _functions.Find(f => f is TouchFunction.CriticalTouchFunction) as TouchFunction.CriticalTouchFunction;

            if (critical == null)
            {
                var criticalData = new TouchFunctionDataModel
                {
                    ID = $"TOUCH_CRITICAL_{System.Guid.NewGuid()}",
                    FunctionName = "Critical Click",
                    FunctionType = "CriticalClick",
                    TriggerCount = 0,
                    Multiplier = 5f,
                    Duration = duration,
                    Cooldown = 0f,
                    CriticalChance = chance,
                    IsActive = true,
                    IsReusable = true
                };

                AddFunctionFromData(criticalData);
                Debug.Log($"[TouchFunctionManager] Critical mode created with {chance * 100}% chance");
            }
            else
            {
                critical.SetCriticalChance(chance);
                Debug.Log($"[TouchFunctionManager] Critical chance updated to {chance * 100}%");
            }

            OnFunctionActivated?.Invoke("Critical Mode");
        }

        public bool IsSpeedBoostActive()
        {
            var speedBoost = _functions.Find(f => f is TouchFunction.SpeedBoostFunction) as TouchFunction.SpeedBoostFunction;
            return speedBoost?.IsActive ?? false;
        }

        public float GetSpeedBoostCooldown()
        {
            var speedBoost = _functions.Find(f => f is TouchFunction.SpeedBoostFunction) as TouchFunction.SpeedBoostFunction;
            return speedBoost?.GetCooldownRemaining(Time.time) ?? 0f;
        }

        public List<string> GetActiveFunctionNames()
        {
            var names = new List<string>();
            foreach (var function in _functions)
            {
                names.Add(function.FunctionName);
            }
            return names;
        }

        public List<TouchFunctionDataModel> GetRegisteredData()
        {
            return new List<TouchFunctionDataModel>(_registeredData);
        }

        public ITouchFunction GetFunction(string functionName)
        {
            return _functions.Find(f => f.FunctionName == functionName);
        }

        public ITouchFunction GetFunctionById(string functionId)
        {
            var data = _registeredData.Find(d => d.ID == functionId);
            if (data != null)
            {
                return _functions.Find(f => f.FunctionName == data.FunctionName);
            }
            return null;
        }

        public bool ContainsFunction(string functionName)
        {
            return _functions.Exists(f => f.FunctionName == functionName);
        }

        public bool ContainsFunctionById(string functionId)
        {
            return _registeredData.Exists(d => d.ID == functionId);
        }

        public void ResetAll()
        {
            foreach (var function in _reusableFunctions)
            {
                function.Reset();
            }

            _temporaryFunctions.Clear();
            _currentMultiplier = 1f;
            _lastActivateTime = -999f;

            Debug.Log("[TouchFunctionManager] Reset all functions (kept reusable)");
        }

        public void ClearAll()
        {
            _functions.Clear();
            _reusableFunctions.Clear();
            _temporaryFunctions.Clear();
            _registeredData.Clear();
            _currentMultiplier = 1f;
            _lastActivateTime = -999f;

            Debug.Log("[TouchFunctionManager] Cleared all functions");
        }

        public void LogStatus()
        {
            Debug.Log("=== TouchFunctionManager Status ===");
            Debug.Log($"  Total Functions: {_functions.Count}");
            Debug.Log($"  Capacity: {_functions.Capacity} (Infinite: {_functions.Capacity >= int.MaxValue - 1000})");
            Debug.Log($"  Reusable: {_reusableFunctions.Count}");
            Debug.Log($"  Temporary: {_temporaryFunctions.Count}");
            Debug.Log($"  Registered Data: {_registeredData.Count}");
            Debug.Log($"  Current Multiplier: {_currentMultiplier}x");
            Debug.Log("");
            Debug.Log("  [Functions]");

            foreach (var function in _functions)
            {
                Debug.Log($"    - {function.FunctionName} (Reusable: {function.IsReusable})");
            }

            Debug.Log("");
            Debug.Log("  [Registered Data]");

            foreach (var data in _registeredData)
            {
                Debug.Log($"    - {data.ID}: {data.FunctionName} ({data.FunctionType})");
            }

            Debug.Log("=====================================");
        }

        private void Update()
        {
            UpdateFunctions(Time.time);
        }

        private void OnDestroy()
        {
            ClearAll();
        }
    }
}
