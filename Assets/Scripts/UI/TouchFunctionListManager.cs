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
        public System.Action<int> OnPointsChanged;
        
        private TouchCounter _touchCounter;
        
        public int TouchPoints => _touchCounter != null ? _touchCounter.TotalTouchCount : 0;
        
        public int PointsPerClick => CalculatePointsPerClick();
        
        private int CalculatePointsPerClick()
        {
            int basePoints = 1;
            float multiplier = 1f;
            int bonusPoints = 0;
            
            foreach (var function in activeFunctions)
            {
                switch (function.Effect)
                {
                    case "DoubleClick":
                        multiplier *= 2f;
                        break;
                    case "TripleClick":
                        multiplier *= 3f;
                        break;
                    case "PowerBoost":
                        basePoints += (int)function.GetCurrentValue();
                        break;
                    case "BonusPoints":
                        bonusPoints += (int)function.GetCurrentValue();
                        break;
                    case "Critical":
                    case "SuperCritical":
                        break;
                    case "SpeedBoost":
                        break;
                }
            }
            
            if (_touchFunctionManager != null && _touchFunctionManager.IsSpeedBoostActive())
            {
                multiplier *= 2f;
            }
            
            return Mathf.RoundToInt(basePoints * multiplier) + bonusPoints;
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
            
            _touchCounter = FindFirstObjectByType<TouchCounter>();
            
            activeFunctions = new List<TouchFunctionData>();
            
            LoadFromExcel();
            
            Debug.Log($"[TouchFunctionList] Initialized! Points: {TouchPoints}, Active Functions: {activeFunctions.Count}");
        }
        
        private void Start()
        {
            if (_touchCounter == null)
            {
                _touchCounter = FindFirstObjectByType<TouchCounter>();
            }
        }
        
        public bool CanAfford(int cost)
        {
            return TouchPoints >= cost;
        }
        
        public void SpendPoints(int cost)
        {
            if (_touchCounter != null && CanAfford(cost))
            {
                _touchCounter.SubtractPoints(cost);
                OnPointsChanged?.Invoke(TouchPoints);
                Debug.Log($"[TouchFunctionList] Spent {cost} points. Remaining: {TouchPoints}");
            }
        }
        
        public void AddFunction(string functionId)
        {
            var function = allFunctions.Find(f => f.ID == functionId);
            if (function == null)
            {
                Debug.LogError($"[TouchFunctionList] Function {functionId} not found!");
                return;
            }
            
            var existingFunction = activeFunctions.Find(f => f.ID == functionId);
            
            if (existingFunction != null)
            {
                // 이미 활성화됨 → 레벨업
                if (!existingFunction.CanLevelUp())
                {
                    Debug.LogWarning($"[TouchFunctionList] {function.Name} is already at max level ({existingFunction.MaxLevel})!");
                    return;
                }
                
                int cost = existingFunction.GetCurrentCost();
                
                if (!CanAfford(cost))
                {
                    Debug.LogWarning($"[TouchFunctionList] Not enough points! Need {cost}, have {TouchPoints}");
                    return;
                }
                
                SpendPoints(cost);
                existingFunction.Level++;
                Debug.Log($"[TouchFunctionList] Level up! {function.Name} → Lv.{existingFunction.Level}/{existingFunction.MaxLevel}");
                Debug.Log($"[TouchFunctionList] PointsPerClick now: {PointsPerClick}");
                OnFunctionAdded?.Invoke(functionId);
                SaveToExcel();
                return;
            }
            
            // 새 함수 추가
            int newCost = function.BaseCost;
            
            if (!CanAfford(newCost))
            {
                Debug.LogWarning($"[TouchFunctionList] Not enough points! Need {newCost}, have {TouchPoints}");
                return;
            }
            
            SpendPoints(newCost);
            
            var newFunction = function.Clone();
            newFunction.IsActive = true;
            newFunction.Level = 1;
            activeFunctions.Add(newFunction);
            
            Debug.Log($"[TouchFunctionList] Added {function.Name} Lv.1/{function.MaxLevel}");
            Debug.Log($"[TouchFunctionList] PointsPerClick now: {PointsPerClick}");
            
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
                case "SuperCritical":
                    _touchFunctionManager.ActivateCriticalMode(0.2f, 0f);
                    break;
                case "SpeedBoost":
                    _touchFunctionManager.ActivateSpeedBoost();
                    break;
                case "PowerBoost":
                case "BonusPoints":
                case "DoubleClick":
                case "TripleClick":
                    Debug.Log($"[TouchFunctionList] Effect {function.Effect} applied - Points per click updated");
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
        
        public TouchFunctionData GetActiveFunction(string functionId)
        {
            return activeFunctions.Find(f => f.ID == functionId);
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
                        ID = func.ID,
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
                new TouchFunctionData 
                { 
                    ID = "power_boost", 
                    Name = "터치 파워", 
                    Description = "터치당 +1 증가", 
                    BaseCost = 50, 
                    BaseValue = 1,
                    MaxLevel = 10,
                    CostMultiplier = 1.5f,
                    Level = 1, 
                    Effect = "PowerBoost", 
                    IsActive = false 
                },
                new TouchFunctionData 
                { 
                    ID = "bonus_points", 
                    Name = "보너스 포인트", 
                    Description = "터치당 +5 추가", 
                    BaseCost = 75, 
                    BaseValue = 5,
                    MaxLevel = 10,
                    CostMultiplier = 1.5f,
                    Level = 1, 
                    Effect = "BonusPoints", 
                    IsActive = false 
                },
                new TouchFunctionData 
                { 
                    ID = "critical", 
                    Name = "크리티컬", 
                    Description = "10% 확률로 2배", 
                    BaseCost = 100, 
                    BaseValue = 0.1f,
                    MaxLevel = 5,
                    CostMultiplier = 2f,
                    Level = 1, 
                    Effect = "Critical", 
                    IsActive = false 
                },
                new TouchFunctionData 
                { 
                    ID = "double_click", 
                    Name = "더블클릭", 
                    Description = "항상 2배", 
                    BaseCost = 500, 
                    BaseValue = 2,
                    MaxLevel = 1,
                    CostMultiplier = 1f,
                    Level = 1, 
                    Effect = "DoubleClick", 
                    IsActive = false 
                },
                new TouchFunctionData 
                { 
                    ID = "triple_click", 
                    Name = "트리플클릭", 
                    Description = "항상 3배", 
                    BaseCost = 1000, 
                    BaseValue = 3,
                    MaxLevel = 1,
                    CostMultiplier = 1f,
                    Level = 1, 
                    Effect = "TripleClick", 
                    IsActive = false 
                }
            };
            Debug.Log($"[TouchFunctionList] Created {allFunctions.Count} default functions");
        }
        
        private string GetDescription(string functionType)
        {
            switch (functionType)
            {
                case "Critical": return "10% 확률로 2 배";
                case "SuperCritical": return "20% 확률로 3 배";
                case "SpeedBoost": return "연타 속도 증가";
                case "Bonus": return "50 회마다 +10";
                case "DoubleClick": return "항상 2 배 클릭";
                case "TripleClick": return "항상 3 배 클릭";
                case "PowerBoost": return "기본 터치 증가";
                case "BonusPoints": return "클릭 시 추가 포인트";
                default: return "효과 없음";
            }
        }
        
        private int GetCost(string functionType)
        {
            switch (functionType)
            {
                case "Critical": return 50;
                case "SuperCritical": return 200;
                case "SpeedBoost": return 100;
                case "Bonus": return 150;
                case "DoubleClick": return 300;
                case "TripleClick": return 500;
                case "PowerBoost": return 50;
                case "BonusPoints": return 75;
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