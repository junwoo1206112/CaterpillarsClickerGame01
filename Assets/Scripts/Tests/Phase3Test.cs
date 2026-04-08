using UnityEngine;
using UnityEngine.UI;
using ClickerGame.UI;
using ClickerGame.Gameplay;
using ClickerGame.Data.Models;

namespace ClickerGame.Tests
{
    public class Phase3Test : MonoBehaviour
    {
        [Header("Test UI")]
        public Text statusText;
        public Text pointsText;
        public Text logText;

        private System.Text.StringBuilder _logBuilder = new System.Text.StringBuilder();

        private void Start()
        {
            CreateTestUI();
            RunTests();
        }

        private void CreateTestUI()
        {
            GameObject canvasObj = new GameObject("TestCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            statusText = CreateText(canvas.transform, "Phase 3 Test", new Vector2(20, -20), 200, 30, Color.cyan);
            pointsText = CreateText(canvas.transform, "Points: 0", new Vector2(20, -60), 200, 30, Color.yellow);
            logText = CreateText(canvas.transform, "", new Vector2(20, -100), 600, 400);
            logText.alignment = TextAnchor.UpperLeft;
            logText.verticalOverflow = VerticalWrapMode.Overflow;
        }

        private void RunTests()
        {
            AppendLog("=== Phase 3 Test Started ===\n");

            Test_TouchFunctionListSO();
            Test_TouchFunctionListManager();
            Test_TouchCounterIntegration();
            Test_UIConnection();

            AppendLog("\n=== All Tests Completed ===");
        }

        private void Test_TouchFunctionListSO()
        {
            AppendLog("[TEST] TouchFunctionListSO...\n");

            var dataList = Resources.Load<TouchFunctionListSO>("TouchFunctionList");

            if (dataList != null)
            {
                AppendLog($"✓ TouchFunctionListSO loaded\n");
                AppendLog($"  Functions count: {dataList.Functions.Count}\n");

                foreach (var func in dataList.Functions)
                {
                    AppendLog($"  - {func.FunctionName} ({func.FunctionType})\n");
                }
            }
            else
            {
                AppendLog("✗ TouchFunctionListSO not found!\n");
                AppendLog("  → Run: Tools > Game > Convert Excel to TouchFunctionListSO\n");
            }

            AppendLog("\n");
        }

        private void Test_TouchFunctionListManager()
        {
            AppendLog("[TEST] TouchFunctionListManager...\n");

            var manager = TouchFunctionListManager.Instance;

            if (manager != null)
            {
                AppendLog($"✓ TouchFunctionListManager found\n");
                AppendLog($"  All functions: {manager.allFunctions.Count}\n");
                AppendLog($"  Active functions: {manager.activeFunctions.Count}\n");
                AppendLog($"  Touch points: {manager.TouchPoints}\n");
            }
            else
            {
                AppendLog("✗ TouchFunctionListManager not found!\n");
                AppendLog("  → Check if TouchFunctionPanel has TouchFunctionListManager\n");
            }

            AppendLog("\n");
        }

        private void Test_TouchCounterIntegration()
        {
            AppendLog("[TEST] TouchCounter Integration...\n");

            var touchCounter = FindFirstObjectByType<TouchCounter>();

            if (touchCounter != null)
            {
                AppendLog($"✓ TouchCounter found\n");
                AppendLog($"  Total touches: {touchCounter.TotalTouchCount}\n");

                if (TouchFunctionListManager.Instance != null)
                {
                    AppendLog($"✓ TouchCounter → TouchFunctionListManager connected\n");
                }
                else
                {
                    AppendLog("✗ TouchFunctionListManager not connected\n");
                }
            }
            else
            {
                AppendLog("✗ TouchCounter not found!\n");
            }

            AppendLog("\n");
        }

        private void Test_UIConnection()
        {
            AppendLog("[TEST] UI Connection...\n");

            var listView = FindFirstObjectByType<TouchFunctionListView>();

            if (listView != null)
            {
                AppendLog($"✓ TouchFunctionListView found\n");

                var contentParentField = typeof(TouchFunctionListView).GetField("contentParent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var contentParent = contentParentField?.GetValue(listView) as Transform;
                if (contentParent != null)
                {
                    AppendLog($"  ✓ Content parent connected\n");
                }
                else
                {
                    AppendLog("  ✗ Content parent NOT connected\n");
                }

                var itemPrefabField = typeof(TouchFunctionListView).GetField("itemPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var itemPrefab = itemPrefabField?.GetValue(listView) as TouchFunctionListItem;
                if (itemPrefab != null)
                {
                    AppendLog($"  ✓ Item prefab connected\n");
                }
                else
                {
                    AppendLog("  ✗ Item prefab NOT connected\n");
                    AppendLog("    → Run: Tools > Game > Create TouchFunctionListItem Prefab\n");
                }

                var scrollRectField = typeof(TouchFunctionListView).GetField("scrollRect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var scrollRect = scrollRectField?.GetValue(listView) as ScrollRect;
                if (scrollRect != null)
                {
                    AppendLog($"  ✓ ScrollRect connected\n");
                }
                else
                {
                    AppendLog("  ✗ ScrollRect NOT connected\n");
                }
            }
            else
            {
                AppendLog("✗ TouchFunctionListView not found!\n");
                AppendLog("  → Run: Tools > Game > Connect Scene UI\n");
            }

            AppendLog("\n");
        }

        private void Update()
        {
            UpdatePointsDisplay();
        }

        private void UpdatePointsDisplay()
        {
            if (pointsText != null && TouchFunctionListManager.Instance != null)
            {
                pointsText.text = $"Points: {TouchFunctionListManager.Instance.TouchPoints}";
            }
        }

        private void AppendLog(string message)
        {
            _logBuilder.AppendLine(message);

            if (_logBuilder.Length > 2000)
            {
                _logBuilder.Remove(0, _logBuilder.Length - 2000);
            }

            if (logText != null)
            {
                logText.text = _logBuilder.ToString();
            }

            Debug.Log($"[Phase3Test] {message}");
        }

        private Text CreateText(Transform parent, string text, Vector2 position, int width, int height, Color? color = null)
        {
            GameObject textObj = new GameObject("Text_" + text.Replace("\n", ""));
            textObj.transform.SetParent(parent, false);

            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.localPosition = position;

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 18;
            uiText.color = color ?? Color.white;
            uiText.alignment = TextAnchor.UpperLeft;

            return uiText;
        }
    }
}