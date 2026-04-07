using UnityEngine;
using ClickerGame.Gameplay;

namespace ClickerGame.Tests
{
    public class TouchFunctionTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private int _testClickCount = 100;
        [SerializeField] private float _testCriticalChance = 0.2f;

        [Header("References")]
        private TouchFunctionManager _manager;

        private void Start()
        {
            _manager = FindFirstObjectByType<TouchFunctionManager>();
        }

        [ContextMenu("Test: Add Bonus Function")]
        public void TestAddBonusFunction()
        {
            if (_manager == null) return;

            var data = new Data.Models.TouchFunctionDataModel
            {
                ID = $"TEST_BONUS_{Random.Range(0, 9999)}",
                FunctionName = "Test Bonus",
                FunctionType = "BonusClick",
                TriggerCount = 10,
                Multiplier = 2f,
                Duration = 0f,
                Cooldown = 0f,
                CriticalChance = 0f,
                IsActive = true,
                IsReusable = true
            };

            _manager.AddFunctionFromData(data);
            Debug.Log("[Test] Added bonus function");
        }

        [ContextMenu("Test: Add Speed Boost")]
        public void TestAddSpeedBoost()
        {
            if (_manager == null) return;

            var data = new Data.Models.TouchFunctionDataModel
            {
                ID = $"TEST_SPEED_{Random.Range(0, 9999)}",
                FunctionName = "Test Speed Boost",
                FunctionType = "SpeedBoost",
                TriggerCount = 0,
                Multiplier = 3f,
                Duration = 30f,
                Cooldown = 60f,
                CriticalChance = 0f,
                IsActive = true,
                IsReusable = true
            };

            _manager.AddFunctionFromData(data);
            Debug.Log("[Test] Added speed boost function");
        }

        [ContextMenu("Test: Activate Critical Mode")]
        public void TestActivateCriticalMode()
        {
            if (_manager == null) return;

            _manager.ActivateCriticalMode(_testCriticalChance);
            Debug.Log($"[Test] Activated critical mode with {_testCriticalChance * 100}% chance");
        }

        [ContextMenu("Test: Simulate Clicks")]
        public void TestSimulateClicks()
        {
            if (_manager == null) return;

            Debug.Log($"[Test] Simulating {_testClickCount} clicks...");

            for (int i = 1; i <= _testClickCount; i++)
            {
                var effect = _manager.ProcessTouch(i);

                if (!string.IsNullOrEmpty(effect.EffectName))
                {
                    Debug.Log($"[Test] Click {i}: {effect.EffectName} (Multiplier: {effect.ScoreMultiplier}x)");
                }
            }

            Debug.Log("[Test] Simulation complete");
        }

        [ContextMenu("Test: Expand List")]
        public void TestExpandList()
        {
            if (_manager == null) return;

            _manager.ExpandList(50);
            Debug.Log("[Test] Expanded list by 50");
        }

        [ContextMenu("Test: Log Status")]
        public void TestLogStatus()
        {
            if (_manager == null) return;

            _manager.LogStatus();
        }

        [ContextMenu("Test: Reset All")]
        public void TestResetAll()
        {
            if (_manager == null) return;

            _manager.ResetAll();
            Debug.Log("[Test] Reset all functions");
        }

        [ContextMenu("Test: Clear All")]
        public void TestClearAll()
        {
            if (_manager == null) return;

            _manager.ClearAll();
            Debug.Log("[Test] Cleared all functions");
        }

        private void OnGUI()
        {
            if (_manager == null) return;

            GUILayout.BeginArea(new Rect(Screen.width - 220, 10, 210, 400));
            GUILayout.BeginVertical("box");

            GUILayout.Label($"Functions: {_manager.ActiveFunctionCount}");
            GUILayout.Label($"Reusable: {_manager.ReusableFunctionCount}");
            GUILayout.Label($"Capacity: {_manager.MaxCapacity}");
            GUILayout.Label($"Multiplier: {_manager.CurrentMultiplier}x");

            GUILayout.Space(10);

            if (GUILayout.Button("Add Bonus"))
                TestAddBonusFunction();

            if (GUILayout.Button("Add Speed Boost"))
                TestAddSpeedBoost();

            if (GUILayout.Button("Critical Mode"))
                TestActivateCriticalMode();

            if (GUILayout.Button("Simulate 100 Clicks"))
                TestSimulateClicks();

            if (GUILayout.Button("Expand List (+50)"))
                TestExpandList();

            if (GUILayout.Button("Log Status"))
                TestLogStatus();

            GUILayout.Space(10);

            if (GUILayout.Button("Reset"))
                TestResetAll();

            if (GUILayout.Button("Clear All"))
                TestClearAll();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
