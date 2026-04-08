using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class LeftButtonPanelSetup
    {
        [MenuItem("Tools/Game/Create Left Button Panel")]
        public static void CreateLeftButtonPanel()
        {
            Debug.Log("[LeftButtonPanel] Creating left button panel...");

            // 1. Canvas 찾기
            var canvas = Object.FindFirstObjectByType<Canvas>();
            
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                Debug.Log("[LeftButtonPanel] Created Canvas");
            }

            // 2. LeftButtonPanel 생성
            GameObject panelObj = GameObject.Find("LeftButtonPanel");
            
            if (panelObj == null)
            {
                panelObj = new GameObject("LeftButtonPanel");
                panelObj.transform.SetParent(canvas.transform, false);
                
                RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 0.5f);
                rectTransform.sizeDelta = new Vector2(200, 0);
                rectTransform.localPosition = new Vector3(10, 0, 0);
                
                Image image = panelObj.AddComponent<Image>();
                image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                
                VerticalLayoutGroup layoutGroup = panelObj.AddComponent<VerticalLayoutGroup>();
                layoutGroup.padding = new RectOffset(10, 10, 20, 20);
                layoutGroup.spacing = 10;
                layoutGroup.childAlignment = TextAnchor.UpperCenter;
                layoutGroup.childControlWidth = true;
                layoutGroup.childControlHeight = false;
                
                Debug.Log("[LeftButtonPanel] Created LeftButtonPanel");
            }
            else
            {
                Debug.Log("[LeftButtonPanel] LeftButtonPanel already exists");
            }

            // 3. 버튼 생성
            CreateButton(panelObj.transform, "SettingButton", "설정");
            CreateButton(panelObj.transform, "CharacterButton", "캐릭터 치장");
            CreateButton(panelObj.transform, "BackgroundButton", "배경");
            CreateButton(panelObj.transform, "SpeedButton", "터치 2 배속");
            CreateButton(panelObj.transform, "ItemButton", "점수 +100~500");
            CreateButton(panelObj.transform, "ResetButton", "게임 초기화");

            // 4. UIManager 찾기 또는 생성
            var uiManager = Object.FindFirstObjectByType<UIManager>();
            
            if (uiManager == null)
            {
                GameObject managerObj = new GameObject("UIManager");
                uiManager = managerObj.AddComponent<UIManager>();
                Debug.Log("[LeftButtonPanel] Created UIManager");
            }

            // 5. UIManager 에 버튼 연결
            ConnectButtons(uiManager, panelObj.transform);

            // 6. Scene 저장
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            Debug.Log("[LeftButtonPanel] Left button panel created successfully!");
            EditorUtility.DisplayDialog("왼쪽 버튼 패널 완료", 
                "왼쪽에 6 개 버튼이 생성되었습니다!\n\n" +
                "- 설정\n" +
                "- 캐릭터 치장\n" +
                "- 배경\n" +
                "- 터치 2 배속\n" +
                "- 점수 +100~500\n" +
                "- 게임 초기화\n\n" +
                "Play 버튼을 눌러 테스트하세요.", 
                "OK");
        }

        private static void CreateButton(Transform parent, string name, string text)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(180, 50);

            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            Button button = buttonObj.AddComponent<Button>();
            button.colors = new ColorBlock
            {
                normalColor = new Color(0.3f, 0.3f, 0.3f, 1f),
                highlightedColor = new Color(0.4f, 0.4f, 0.4f, 1f),
                pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f),
                selectedColor = Color.gray,
                disabledColor = new Color(0.2f, 0.2f, 0.2f, 0.5f),
                colorMultiplier = 1f,
                fadeDuration = 0.1f
            };

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 16;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }

        private static void ConnectButtons(UIManager uiManager, Transform parent)
        {
            var so = new SerializedObject(uiManager);

            ConnectButton(so, "SettingButton", parent, "settingButton");
            ConnectButton(so, "CharacterButton", parent, "characterCustomizeButton");
            ConnectButton(so, "BackgroundButton", parent, "backgroundButton");
            ConnectButton(so, "SpeedButton", parent, "speedBoostButton");
            ConnectButton(so, "ItemButton", parent, "itemButton");
            ConnectButton(so, "ResetButton", parent, "resetButton");

            so.ApplyModifiedProperties();

            Debug.Log("[LeftButtonPanel] Buttons connected to UIManager");
        }

        private static void ConnectButton(SerializedObject so, string buttonName, Transform parent, string fieldName)
        {
            Transform buttonTransform = parent.Find(buttonName);
            
            if (buttonTransform != null)
            {
                var button = buttonTransform.GetComponent<Button>();
                if (button != null)
                {
                    so.FindProperty(fieldName).objectReferenceValue = button;
                    Debug.Log($"[LeftButtonPanel] Connected {fieldName} to {buttonName}");
                }
            }
            else
            {
                Debug.LogWarning($"[LeftButtonPanel] Button {buttonName} not found");
            }
        }
    }
}