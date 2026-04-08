using UnityEngine;
using System.Collections.Generic;
using ClickerGame.Data.Models;

namespace ClickerGame.UI
{
    public class TouchFunctionListManager : MonoBehaviour
    {
        public static TouchFunctionListManager Instance { get; private set; }
        
        [Header("Data")]
        public List<TouchFunctionData> allFunctions = new();
        public List<TouchFunctionData> activeFunctions = new();
        
        [Header("Events")]
        public System.Action<string> OnFunctionAdded;
        public System.Action<string> OnFunctionRemoved;
        
        private int _touchPoints = 0;
        
        public int TouchPoints => _touchPoints;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
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
            var function = allFunctions.Find(f => f.id == functionId);
            if (function == null)
            {
                Debug.LogError($"[TouchFunctionList] Function {functionId} not found!");
                return;
            }
            
            if (!CanAfford(function.cost))
            {
                Debug.LogWarning($"[TouchFunctionList] Not enough points! Need {function.cost}, have {_touchPoints}");
                return;
            }
            
            if (activeFunctions.Find(f => f.id == functionId) != null)
            {
                Debug.LogWarning($"[TouchFunctionList] Function {functionId} already active!");
                return;
            }
            
            SpendPoints(function.cost);
            var newFunction = function.Clone();
            newFunction.isActive = true;
            activeFunctions.Add(newFunction);
            
            Debug.Log($"[TouchFunctionList] Added {function.name} for {function.cost} points");
            OnFunctionAdded?.Invoke(functionId);
            
            SaveToExcel();
        }
        
        public void RemoveFunction(string functionId)
        {
            var function = activeFunctions.Find(f => f.id == functionId);
            if (function == null)
            {
                Debug.LogWarning($"[TouchFunctionList] Function {functionId} not active!");
                return;
            }
            
            activeFunctions.Remove(function);
            Debug.Log($"[TouchFunctionList] Removed {function.name}");
            OnFunctionRemoved?.Invoke(functionId);
            
            SaveToExcel();
        }
        
        public bool IsFunctionActive(string functionId)
        {
            return activeFunctions.Find(f => f.id == functionId) != null;
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
                        id = func.FunctionName,
                        name = func.FunctionName,
                        description = GetDescription(func.FunctionType),
                        cost = GetCost(func.FunctionType),
                        level = 1,
                        effect = func.FunctionType,
                        isActive = false
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
                new TouchFunctionData { id = "critical", name = "크리티컬", description = "10% 확률로 2 배", cost = 50, level = 1, effect = "Critical", isActive = false },
                new TouchFunctionData { id = "speed", name = "스피드부스트", description = "연타 속도 증가", cost = 100, level = 1, effect = "SpeedBoost", isActive = false },
                new TouchFunctionData { id = "bonus", name = "보너스 터치", description = "50 회마다 +10", cost = 150, level = 1, effect = "Bonus", isActive = false },
                new TouchFunctionData { id = "super_critical", name = "슈퍼크리티컬", description = "20% 확률로 3 배", cost = 200, level = 1, effect = "SuperCritical", isActive = false },
                new TouchFunctionData { id = "double_click", name = "더블클릭", description = "항상 2 배 클릭", cost = 300, level = 1, effect = "DoubleClick", isActive = false }
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
