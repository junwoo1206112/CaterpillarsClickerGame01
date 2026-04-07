using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class Phase3UISetup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Phase 3 UI")]
        public static void SetupPhase3UI()
        {
            if (EditorUtility.DisplayDialog("Setup Phase 3 UI",
                "This will create all UI elements for Phase 3.\n\nContinue?",
                "Yes", "No"))
            {
                CreatePhase3UI();
                Debug.Log("[Phase3UISetup] Phase 3 UI setup complete!");
            }
        }

        private static void CreatePhase3UI()
        {
            CreateMainCanvas();
            CreateTopBar();
            CreateWindows();
            CreateEventSystem();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateMainCanvas()
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj == null)
            {
                canvasObj = new GameObject("Canvas");
                canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                var canvas = canvasObj.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                var scaler = canvasObj.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
            }

            if (canvasObj.GetComponent<UIManager>() == null)
            {
                canvasObj.AddComponent<UIManager>();
            }

            if (canvasObj.GetComponent<GameUI>() == null)
            {
                canvasObj.AddComponent<GameUI>();
            }

            Debug.Log("[Phase3UISetup] Main Canvas created");
        }

        private static void CreateTopBar()
        {
            GameObject topBarObj = new GameObject("TopBar");
            topBarObj.transform.SetParent(GameObject.Find("Canvas").transform, false);

            RectTransform rectTransform = topBarObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(500, 60);
            rectTransform.localPosition = new Vector3(10, -10, 0);

            string[] buttonNames = new string[]
            {
                "SettingButton", "CharacterColorButton", "BackgroundButton",
                "SpeedBoostButton", "ItemButton"
            };

            string[] buttonTexts = new string[]
            {
                "⚙️", "🎨", "🖼️", "⚡", "🎒"
            };

            for (int i = 0; i < buttonNames.Length; i++)
            {
                CreateTopBarButton(topBarObj.transform, buttonNames[i], buttonTexts[i], i * 60);
            }

            Debug.Log("[Phase3UISetup] TopBar created with 5 buttons");
        }

        private static void CreateTopBarButton(Transform parent, string name, string text, int xPos)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.sizeDelta = new Vector2(50, 50);
            rectTransform.localPosition = new Vector3(xPos, -25, 0);

            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            Button button = buttonObj.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f);
            colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f);
            colors.pressedColor = new Color(0.5f, 0.5f, 0.5f);
            button.colors = colors;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            textObj.transform.localPosition = Vector3.zero;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 24;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }

        private static void CreateWindows()
        {
            CreateSettingWindow();
            CreateCharacterColorWindow();
            CreateBackgroundWindow();
            CreateItemWindow();

            Debug.Log("[Phase3UISetup] Windows created");
        }

        private static void CreateSettingWindow()
        {
            GameObject windowObj = CreateWindowBase("SettingWindow", new Vector2(400, 300));

            // Title
            CreateLabel(windowObj.transform, "Title", "Settings", new Vector2(0, 130), new Vector2(380, 30));

            // Toggles
            CreateToggle(windowObj.transform, "BGMToggle", "BGM: ON", new Vector2(-100, 80));
            CreateToggle(windowObj.transform, "SFXToggle", "SFX: ON", new Vector2(100, 80));

            // Sliders
            CreateSlider(windowObj.transform, "BGMSlider", "BGM Volume", new Vector2(-100, 40));
            CreateSlider(windowObj.transform, "SFXSlider", "SFX Volume", new Vector2(100, 40));

            // Buttons
            CreateButton(windowObj.transform, "SaveButton", "Save", new Vector2(-100, -120), 120, 40);
            CreateButton(windowObj.transform, "CloseButton", "Close", new Vector2(100, -120), 120, 40);

            // Add SettingManager
            if (windowObj.GetComponent<SettingManager>() == null)
            {
                windowObj.AddComponent<SettingManager>();
            }

            windowObj.SetActive(false);
        }

        private static void CreateCharacterColorWindow()
        {
            GameObject windowObj = CreateWindowBase("CharacterColorWindow", new Vector2(500, 400));

            // Title
            CreateLabel(windowObj.transform, "Title", "Character Color", new Vector2(0, 180), new Vector2(480, 30));

            // Color Grid Parent
            GameObject gridObj = new GameObject("ColorGrid");
            gridObj.transform.SetParent(windowObj.transform, false);
            RectTransform gridRect = gridObj.AddComponent<RectTransform>();
            gridRect.sizeDelta = new Vector2(400, 250);
            gridRect.localPosition = new Vector3(0, -20, 0);

            // Add CharacterColorManager
            if (windowObj.GetComponent<CharacterColorManager>() == null)
            {
                CharacterColorManager manager = windowObj.AddComponent<CharacterColorManager>();
                // Will be connected via Inspector
            }

            // Close Button
            CreateButton(windowObj.transform, "CloseButton", "Close", new Vector2(0, -180), 120, 40);

            windowObj.SetActive(false);
        }

        private static void CreateBackgroundWindow()
        {
            GameObject windowObj = CreateWindowBase("BackgroundWindow", new Vector2(500, 400));

            // Title
            CreateLabel(windowObj.transform, "Title", "Background", new Vector2(0, 180), new Vector2(480, 30));

            // Background Grid Parent
            GameObject gridObj = new GameObject("BackgroundGrid");
            gridObj.transform.SetParent(windowObj.transform, false);
            RectTransform gridRect = gridObj.AddComponent<RectTransform>();
            gridRect.sizeDelta = new Vector2(400, 250);
            gridRect.localPosition = new Vector3(0, -20, 0);

            // Close Button
            CreateButton(windowObj.transform, "CloseButton", "Close", new Vector2(0, -180), 120, 40);

            windowObj.SetActive(false);
        }

        private static void CreateItemWindow()
        {
            GameObject windowObj = CreateWindowBase("ItemWindow", new Vector2(600, 500));

            // Title
            CreateLabel(windowObj.transform, "Title", "Items", new Vector2(0, 230), new Vector2(580, 30));

            // Item Grid Parent
            GameObject gridObj = new GameObject("ItemGrid");
            gridObj.transform.SetParent(windowObj.transform, false);
            RectTransform gridRect = gridObj.AddComponent<RectTransform>();
            gridRect.sizeDelta = new Vector2(500, 350);
            gridRect.localPosition = new Vector3(0, -40, 0);

            // Close Button
            CreateButton(windowObj.transform, "CloseButton", "Close", new Vector2(0, -230), 120, 40);

            windowObj.SetActive(false);
        }

        private static GameObject CreateWindowBase(string name, Vector2 size)
        {
            GameObject windowObj = new GameObject(name);
            windowObj.transform.SetParent(GameObject.Find("Canvas").transform, false);

            RectTransform rectTransform = windowObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;
            rectTransform.localPosition = Vector3.zero;

            Image image = windowObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

            return windowObj;
        }

        private static void CreateLabel(Transform parent, string name, string text, Vector2 position, Vector2 size)
        {
            GameObject labelObj = new GameObject(name);
            labelObj.transform.SetParent(parent, false);

            RectTransform rectTransform = labelObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            rectTransform.localPosition = position;

            Text uiText = labelObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 24;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }

        private static void CreateToggle(Transform parent, string name, string text, Vector2 position)
        {
            GameObject toggleObj = new GameObject(name);
            toggleObj.transform.SetParent(parent, false);

            RectTransform rectTransform = toggleObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            rectTransform.localPosition = position;

            Toggle toggle = toggleObj.AddComponent<Toggle>();

            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(toggleObj.transform, false);
            Text labelText = labelObj.AddComponent<Text>();
            labelText.text = text;
            labelText.fontSize = 16;
            labelText.color = Color.white;
            labelText.alignment = TextAnchor.MiddleLeft;

            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = new Vector2(20, 0);
            labelRect.localPosition = new Vector3(25, 0, 0);
        }

        private static void CreateSlider(Transform parent, string name, string text, Vector2 position)
        {
            GameObject sliderObj = new GameObject(name);
            sliderObj.transform.SetParent(parent, false);

            RectTransform rectTransform = sliderObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 20);
            rectTransform.localPosition = position;

            Slider slider = sliderObj.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0.5f;
        }

        private static void CreateButton(Transform parent, string name, string text, Vector2 position, int width, int height)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.localPosition = position;

            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            Button button = buttonObj.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f);
            colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f);
            colors.pressedColor = new Color(0.5f, 0.5f, 0.5f);
            button.colors = colors;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            textObj.transform.localPosition = Vector3.zero;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 20;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem == null)
            {
                eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("[Phase3UISetup] EventSystem created");
        }
    }
}
