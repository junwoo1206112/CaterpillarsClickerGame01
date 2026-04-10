using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class CreateSettingPanel
    {
        private const string PREFAB_PATH = "Assets/Prefabs/SettingPanel.prefab";
        
        [MenuItem("Tools/Game/Setup Setting Panel (Complete)")]
        public static void SetupSettingPanelComplete()
        {
            if (!EditorUtility.DisplayDialog("Setting Panel Setup",
                "Setting Panel 전체 설정을 진행합니다:\n\n1. Prefab 생성\n2. Scene 에 추가\n3. UI 연결\n\n계속하시겠습니까?",
                "실행", "취소"))
                return;

            Debug.Log("[CreateSettingPanel] Starting Setting Panel setup...");

            CreatePrefab();
            ConnectSceneUI();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[CreateSettingPanel] Setting Panel setup complete!");
            EditorUtility.DisplayDialog("Setup Complete", "Setting Panel 설정 완료!\n\n게임을 실행하여 테스트하세요.", "확인");
        }

        [MenuItem("Tools/Game/Create Setting Panel Prefab")]
        public static void CreatePrefab()
        {
            if (File.Exists(PREFAB_PATH))
            {
                AssetDatabase.DeleteAsset(PREFAB_PATH);
                Debug.Log("[CreateSettingPanel] Deleted existing prefab");
            }
            
            GameObject prefab = new GameObject("SettingPanel");
            RectTransform rectTransform = prefab.AddComponent<RectTransform>();
            prefab.AddComponent<CanvasRenderer>();
            
            rectTransform.sizeDelta = new Vector2(400, 300);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            
            Image image = prefab.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            CreateUIElements(prefab);
            AddSettingManager(prefab);
            
            Directory.CreateDirectory("Assets/Prefabs");
            PrefabUtility.SaveAsPrefabAsset(prefab, PREFAB_PATH);
            
            Object.DestroyImmediate(prefab);
            
            Debug.Log($"[CreateSettingPanel] Created prefab at {PREFAB_PATH}");
        }
        
        [MenuItem("Tools/Game/Connect Setting Panel UI")]
        public static void ConnectSceneUI()
        {
            GameObject panelObj = GameObject.Find("SettingPanel");
            
            if (panelObj == null)
            {
                Debug.Log("[CreateSettingPanel] SettingPanel not found in scene. Trying to instantiate from prefab...");
                
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
                if (prefab == null)
                {
                    Debug.LogError("[CreateSettingPanel] Prefab not found! Run 'Create Prefab' first.");
                    return;
                }
                
                panelObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                panelObj.name = "SettingPanel";
                
                Canvas canvas = Object.FindFirstObjectByType<Canvas>();
                if (canvas == null)
                {
                    GameObject canvasObj = new GameObject("Canvas");
                    canvas = canvasObj.AddComponent<Canvas>();
                    canvasObj.AddComponent<CanvasRenderer>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                
                panelObj.transform.SetParent(canvas.transform);
                panelObj.SetActive(false);
                
                Debug.Log("[CreateSettingPanel] SettingPanel added to scene");
            }
            
            RectTransform rect = panelObj.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(400, 300);
            }
            
            Debug.Log("[CreateSettingPanel] Scene UI connected");
        }
        
        private static void AddSettingManager(GameObject prefab)
        {
            SettingManager manager = prefab.AddComponent<SettingManager>();
            
            SerializedObject serializedObject = new SerializedObject(manager);
            
            SerializedProperty bgmSliderProp = serializedObject.FindProperty("bgmSlider");
            SerializedProperty sfxSliderProp = serializedObject.FindProperty("sfxSlider");
            SerializedProperty closeButtonProp = serializedObject.FindProperty("closeButton");
            SerializedProperty testBgmButtonProp = serializedObject.FindProperty("testBgmButton");
            SerializedProperty testSfxButtonProp = serializedObject.FindProperty("testSfxButton");
            
            bgmSliderProp.objectReferenceValue = prefab.transform.Find("BGM Slider")?.GetComponent<Slider>();
            sfxSliderProp.objectReferenceValue = prefab.transform.Find("SFX Slider")?.GetComponent<Slider>();
            closeButtonProp.objectReferenceValue = prefab.transform.Find("Close Button")?.GetComponent<Button>();
            testBgmButtonProp.objectReferenceValue = prefab.transform.Find("Test BGM Button")?.GetComponent<Button>();
            testSfxButtonProp.objectReferenceValue = prefab.transform.Find("Test SFX Button")?.GetComponent<Button>();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private static void CreateUIElements(GameObject parent)
        {
            float startY = 120f;
            float spacing = 50f;
            
            CreateSlider(parent, "BGM Slider", "BGM Volume", startY);
            CreateTestButton(parent, "Test BGM Button", "BGM Test", startY, 250f);
            
            CreateSlider(parent, "SFX Slider", "SFX Volume", startY - spacing);
            CreateTestButton(parent, "Test SFX Button", "SFX Test", startY - spacing, 250f);
            
            CreateCloseButton(parent, "Close Button", "Close", startY - spacing * 3);
        }
        
        private static void CreateSlider(GameObject parent, string name, string label, float y)
        {
            GameObject sliderObj = new GameObject(name);
            sliderObj.transform.SetParent(parent.transform, false);
            
            RectTransform rectTransform = sliderObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 20);
            rectTransform.anchoredPosition = new Vector2(0, y);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            sliderObj.AddComponent<CanvasRenderer>();
            Image bgImage = sliderObj.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 1f);
            
            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObj.transform, false);
            RectTransform fillRect = fillArea.AddComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0.25f);
            fillRect.anchorMax = new Vector2(1, 0.75f);
            fillRect.sizeDelta = new Vector2(-20, 0);
            fillRect.anchoredPosition = new Vector2(5, 0);
            fillArea.AddComponent<CanvasRenderer>();
            Image fillImage = fillArea.AddComponent<Image>();
            fillImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            RectTransform fillRect2 = fill.AddComponent<RectTransform>();
            fillRect2.anchorMin = Vector2.zero;
            fillRect2.anchorMax = Vector2.one;
            fillRect2.sizeDelta = Vector2.zero;
            fillRect2.anchoredPosition = Vector2.zero;
            fill.AddComponent<CanvasRenderer>();
            Image fillImg = fill.AddComponent<Image>();
            fillImg.color = new Color(0.2f, 0.6f, 1f, 1f);
            
            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(sliderObj.transform, false);
            RectTransform handleRect = handle.AddComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 20);
            handleRect.anchoredPosition = Vector2.zero;
            handle.AddComponent<CanvasRenderer>();
            Image handleImage = handle.AddComponent<Image>();
            handleImage.color = Color.white;
            
            Slider slider = sliderObj.AddComponent<Slider>();
            
            SerializedObject so = new SerializedObject(slider);
            so.FindProperty("m_FillRect").objectReferenceValue = fillRect2;
            so.FindProperty("m_HandleRect").objectReferenceValue = handleRect;
            so.FindProperty("m_MinValue").floatValue = 0f;
            so.FindProperty("m_MaxValue").floatValue = 1f;
            so.FindProperty("m_Value").floatValue = 0.5f;
            so.ApplyModifiedProperties();
            
            CreateLabel(parent, label, y + 15);
        }
        
        private static void CreateTestButton(GameObject parent, string name, string text, float y, float x)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent.transform, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 30);
            rectTransform.anchoredPosition = new Vector2(x, y);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            buttonObj.AddComponent<CanvasRenderer>();
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.6f, 1f, 1f);
            
            Button button = buttonObj.AddComponent<Button>();
            button.colors = CreateColorBlock();
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            textObj.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Text uiText = textObj.AddComponent<UnityEngine.UI.Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 14;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }
        
        private static void CreateCloseButton(GameObject parent, string name, string text, float y)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent.transform, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 40);
            rectTransform.anchoredPosition = new Vector2(0, y);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            buttonObj.AddComponent<CanvasRenderer>();
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.8f, 0.2f, 0.2f, 1f);
            
            Button button = buttonObj.AddComponent<Button>();
            button.colors = CreateColorBlock();
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            textObj.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Text uiText = textObj.AddComponent<UnityEngine.UI.Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 18;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }
        
        private static void CreateLabel(GameObject parent, string text, float y)
        {
            GameObject labelObj = new GameObject(text + " Label");
            labelObj.transform.SetParent(parent.transform, false);
            
            RectTransform rectTransform = labelObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 20);
            rectTransform.anchoredPosition = new Vector2(0, y);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            labelObj.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Text uiText = labelObj.AddComponent<UnityEngine.UI.Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 16;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }
        
        private static ColorBlock CreateColorBlock()
        {
            ColorBlock colors = new ColorBlock
            {
                normalColor = new Color(0.2f, 0.6f, 1f, 1f),
                highlightedColor = new Color(0.3f, 0.7f, 1f, 1f),
                pressedColor = new Color(0.1f, 0.5f, 0.9f, 1f),
                selectedColor = new Color(0.2f, 0.6f, 1f, 1f),
                disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f),
                colorMultiplier = 1f,
                fadeDuration = 0.1f
            };
            return colors;
        }
    }
}
