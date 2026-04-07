using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Core;

namespace ClickerGame.Tests
{
    public class SimpleDataViewer : MonoBehaviour
    {
        [Header("UI References")]
        public Text statusText;
        public Text characterCountText;
        public Text backgroundCountText;
        public Text itemCountText;
        public Text configCountText;
        public Text dataDisplayText;

        private void Start()
        {
            if (statusText == null)
            {
                CreateUI();
            }

            Invoke(nameof(LoadAndDisplayData), 1f);
        }

        private void LoadAndDisplayData()
        {
            var dataManager = FindFirstObjectByType<DataManager>();
            
            if (dataManager == null)
            {
                GameObject managerObj = new GameObject("DataManager");
                dataManager = managerObj.AddComponent<DataManager>();
            }

            // DataManager initialization would happen via GameSetup
            Invoke(nameof(DisplayData), 0.5f);
        }

        private void DisplayData()
        {
            var dataManager = FindFirstObjectByType<DataManager>();

            if (dataManager == null)
            {
                statusText.text = "Status: FAIL (DataManager not found)";
                return;
            }

            statusText.text = "Status: SUCCESS ✓";

            int charCount = dataManager.Characters?.Count ?? 0;
            int bgCount = dataManager.Backgrounds?.Count ?? 0;
            int itemCount = dataManager.Items?.Count ?? 0;
            int configCount = dataManager.Configs?.Count ?? 0;

            characterCountText.text = $"Characters: {charCount}";
            backgroundCountText.text = $"Backgrounds: {bgCount}";
            itemCountText.text = $"Items: {itemCount}";
            configCountText.text = $"Configs: {configCount}";

            string dataInfo = "\n=== Data Details ===\n";

            if (charCount > 0)
            {
                dataInfo += "\n[Characters]\n";
                foreach (var c in dataManager.Characters)
                {
                    dataInfo += $"  - {c.ID}: {c.Name} ({c.TouchRequired} touches)\n";
                }
            }

            if (bgCount > 0)
            {
                dataInfo += "\n[Backgrounds]\n";
                foreach (var b in dataManager.Backgrounds)
                {
                    dataInfo += $"  - {b.ID}: {b.Name}\n";
                }
            }

            dataDisplayText.text = dataInfo;
        }

        private void CreateUI()
        {
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            statusText = CreateText(canvas.transform, "Status", new Vector2(20, -20));
            characterCountText = CreateText(canvas.transform, "Characters: 0", new Vector2(20, -50));
            backgroundCountText = CreateText(canvas.transform, "Backgrounds: 0", new Vector2(20, -80));
            itemCountText = CreateText(canvas.transform, "Items: 0", new Vector2(20, -110));
            configCountText = CreateText(canvas.transform, "Configs: 0", new Vector2(20, -140));
            dataDisplayText = CreateText(canvas.transform, "", new Vector2(20, -180), 600, 400);
        }

        private Text CreateText(Transform parent, string text, Vector2 position, int width = 400, int height = 30)
        {
            GameObject textObj = new GameObject("Text_" + text);
            textObj.transform.SetParent(parent, false);
            textObj.transform.localPosition = position;

            RectTransform rectTransform = textObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 16;
            uiText.color = Color.white;

            return uiText;
        }
    }
}
