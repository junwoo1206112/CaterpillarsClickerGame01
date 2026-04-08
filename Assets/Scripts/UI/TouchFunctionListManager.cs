using UnityEngine;
using System.Collections.Generic;
using ClickerGame.Data.Models;
using ClickerGame.Gameplay;

namespace ClickerGame.UI
{
    public class TouchFunctionListManager : MonoBehaviour
    {
        public static TouchFunctionListManager Instance { get; private set; }
        
        [Header("References")]
        [SerializeField] private TouchFunctionManager _touchFunctionManager;
        
        [Header("Data")]
        public List<TouchFunctionData> allFunctions = new();
        public List<TouchFunctionData> activeFunctions = new();
        
        [Header("Events")]
        public System.Action<string> OnFunctionAdded;
        public System.Action<string> OnFunctionRemoved;
        
        private int _touchPoints = 0;
        
        public int TouchPoints => _touchPoints;
        
        public int PointsPerClick => CalculatePointsPerClick();
        
private int CalculatePointsPerClick()
        {
            int basePoints = 1;
            float multiplier = 1f;
            
            Debug.Log($"[PointsPerClick] Checking {activeFunctions.Count} active functions");
            
            foreach (var function in activeFunctions)
            {
                Debug.Log($"[PointsPerClick] Function: {function.Name}, Effect: {function.Effect}");
                
                switch (function.Effect)
                {
                    case "DoubleClick":
                        multiplier *= 2f;
                        Debug.Log($"[PointsPerClick] DoubleClick applied! Multiplier: {multiplier}");
                        break;
                    case "Critical":
                    case "SuperCritical":
                        break;
                    case "SpeedBoost":
                        break;
                    default:
                        Debug.LogWarning($"[PointsPerClick] Unknown effect: {function.Effect}");
                        break;
                }
            }
            
            int result = Mathf.RoundToInt(basePoints * multiplier);
            Debug.Log($"[PointsPerClick] Final: {result} (base: {basePoints}, multiplier: {multiplier})");
            return result;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            if (_touchFunctionManager == null)
            {
                _touchFunctionManager = FindFirstObjectByType<TouchFunctionManager>();
            }
            
            LoadFromExcel();
        }
        
        public void AddTouchPoint(int amount = 1)
        {
            _touchPoints += amount;
            Debug.Log($"[TouchFunctionList] Touch Points: {_touchPoints}");
        }
        
        public bool CanAfford(int cost)
        {
            return _touchPoints >= cost;
        }
        
        public void SpendPoints(int cost)
        {
            if (CanAfford(cost))
            {
                _touchPoints -= cost;
            }
        }
        
        public void AddFunction(string functionId)
        {
            var function = allFunctions.Find(f => f.ID == functionId);
            if (function == null)
            {
                Debug.LogError($"[TouchFunctionList] Function {functionId} not found!");
                Debug.LogError($"[TouchFunctionList] Available IDs: {string.Join(", ", allFunctions.ConvertAll(f => f.ID))}");
                return;
            }
            
            if (!CanAfford(function.Cost))
            {
                Debug.LogWarning($"[TouchFunctionList] Not enough points! Need {function.Cost}, have {_touchPoints}");
                return;
            }
            
            if (activeFunctions.Find(f => f.ID == functionId) != null)
            {
                Debug.LogWarning($"[TouchFunctionList] Function {functionId} already active!");
                return;
            }
            
            SpendPoints(function.Cost);
            var newFunction = function.Clone();
            newFunction.IsActive = true;
            activeFunctions.Add(newFunction);
            
            Debug.Log($"[TouchFunctionList] Added {function.Name} (ID: {function.ID}, Effect: {function.Effect})");
            Debug.Log($"[TouchFunctionList] Active functions: {activeFunctions.Count}");
            foreach (var af in activeFunctions)
            {
                Debug.Log($"  - {af.Name} (Effect: {af.Effect})");
            }
            
            ApplyFunctionEffect(newFunction);
            OnFunctionAdded?.Invoke(functionId);
            SaveToExcel();
        }
        
        public void RemoveFunction(string functionId)
        {
            var function = activeFunctions.Find(f => f.ID == functionId);
            if (function == null)
            {
                Debug.LogWarning($"[TouchFunctionList] Function {functionId} not active!");
                return;
            }
            
            RemoveFunctionEffect(function);
            
            activeFunctions.Remove(function);
            Debug.Log($"[TouchFunctionList] Removed {function.Name}");
            OnFunctionRemoved?.Invoke(functionId);
            
            SaveToExcel();
        }
        
        private void ApplyFunctionEffect(TouchFunctionData function)
        {
            if (_touchFunctionManager == null)
            {
                Debug.LogWarning("[TouchFunctionList] TouchFunctionManager not found!");
                return;
            }
            
            switch (function.Effect)
            {
                case "Critical":
                    _touchFunctionManager.ActivateCriticalMode(0.1f, 0f);
                    break;
                case "SpeedBoost":
                    _touchFunctionManager.ActivateSpeedBoost();
                    break;
                case "SuperCritical":
                    _touchFunctionManager.ActivateCriticalMode(0.2f, 0f);
                    break;
                default:
                    Debug.Log($"[TouchFunctionList] Effect {function.Effect} applied");
                    break;
            }
        }
        
        private void RemoveFunctionEffect(TouchFunctionData function)
        {
            if (_touchFunctionManager == null) return;
            
            switch (function.Effect)
            {
                case "Critical":
                case "SuperCritical":
                    break;
                case "SpeedBoost":
                    break;
                default:
                    break;
            }
            
            Debug.Log($"[TouchFunctionList] Effect {function.Effect} removed");
        }
        
        public bool IsFunctionActive(string functionId)
        {
            return activeFunctions.Find(f => f.ID == functionId) != null;
        }
        
        private void LoadFromExcel()
        {
            var dataList = Resources.Load<TouchFunctionListSO>("TouchFunctionList");
            
            if (dataList != null && dataList.Functions != null)
            {
                allFunctions = new List<TouchFunctionData>();
                foreach (var func in dataList.Functions)
                {
                    allFunctions.Add(new TouchFunctionData
                    {
                        ID = func.ID,  // FunctionName 이 아닌 ID 사용!
                        Name = func.FunctionName,
                        Description = GetDescription(func.FunctionType),
                        Cost = GetCost(func.FunctionType),
                        Level = 1,
                        Effect = func.FunctionType,
                        IsActive = false
                    });
                }
                Debug.Log($"[TouchFunctionList] Loaded {allFunctions.Count} functions from Excel");
            }
            else
            {
                CreateDefaultFunctions();
            }
        }
        
        private void CreateDefaultFunctions()
        {
            allFunctions = new List<TouchFunctionData>
            {
                new TouchFunctionData { ID = "critical", Name = "크리티컬", Description = "10% 확률로 2 배", Cost = 50, Level = 1, Effect = "Critical", IsActive = false },
                new TouchFunctionData { ID = "speed", Name = "스피드부스트", Description = "연타 속도 증가", Cost = 100, Level = 1, Effect = "SpeedBoost", IsActive = false },
                new TouchFunctionData { ID = "bonus", Name = "보너스 터치", Description = "50 회마다 +10", Cost = 150, Level = 1, Effect = "Bonus", IsActive = false },
                new TouchFunctionData { ID = "super_critical", Name = "슈퍼크리티컬", Description = "20% 확률로 3 배", Cost = 200, Level = 1, Effect = "SuperCritical", IsActive = false },
                new TouchFunctionData { ID = "double_click", Name = "더블클릭", Description = "항상 2 배 클릭", Cost = 300, Level = 1, Effect = "DoubleClick", IsActive = false }
            };
            Debug.Log($"[TouchFunctionList] Created {allFunctions.Count} default functions");
        }
        
        private string GetDescription(string functionType)
        {
            switch (functionType)
            {
                case "Critical": return "10% 확률로 2 배";
                case "SpeedBoost": return "연타 속도 증가";
                case "Bonus": return "50 회마다 +10";
                default: return "효과 없음";
            }
        }
        
        private int GetCost(string functionType)
        {
            switch (functionType)
            {
                case "Critical": return 50;
                case "SpeedBoost": return 100;
                case "Bonus": return 150;
                default: return 100;
            }
        }
        
        private void SaveToExcel()
        {
            Debug.Log("[TouchFunctionList] Saved to Excel");
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
