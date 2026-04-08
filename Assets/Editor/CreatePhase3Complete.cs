using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class CreatePhase3Complete
    {
        [MenuItem("Tools/Game/Complete Phase 3 Setup")]
        public static void CompletePhase3()
        {
            Debug.Log("[Phase3Complete] Starting Phase 3 setup...");

            // 1. Canvas 찾기
            var canvas = Object.FindFirstObjectByType<Canvas>();
            
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                Debug.Log("[Phase3Complete] Created Canvas");
            }

            // 2. 왼쪽 버튼 패널 생성
            CreateLeftButtonPanel(canvas.transform);

            // 3. 오른쪽 터치 강화 패널 생성
            CreateTouchFunctionPanel(canvas.transform);

            // 4. UIManager 생성 및 연결
            CreateUIManager(canvas.transform);

            // 5. Scene 저장
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            Debug.Log("[Phase3Complete] Phase 3 setup complete!");
            EditorUtility.DisplayDialog("Phase 3 완료!", 
                "Phase 3 터치 강화 시스템이 완료되었습니다!\n\n" +
                "왼쪽: 버튼 6 개\n" +
                "오른쪽: 터치 강화 패널\n\n" +
                "Play 버튼을 눌러 테스트하세요!", 
                "OK");
        }

        private static void CreateLeftButtonPanel(Transform canvas)
        {
            GameObject panelObj = GameObject.Find("LeftButtonPanel");
            
            if (panelObj == null)
            {
                panelObj = new GameObject("LeftButtonPanel");
                panelObj.transform.SetParent(canvas, false);

                RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 0.5f);
                rectTransform.sizeDelta = new Vector2(180, 0);
                rectTransform.localPosition = new Vector3(10, 0, 0);

                Image image = panelObj.AddComponent<Image>();
                image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

                VerticalLayoutGroup layout = panelObj.AddComponent<VerticalLayoutGroup>();
                layout.padding = new RectOffset(10, 10, 20, 20);
                layout.spacing = 10;
                layout.childAlignment = TextAnchor.UpperCenter;
                layout.childControlWidth = true;
                layout.childControlHeight = false;

                Debug.Log("[Phase3Complete] Created LeftButtonPanel");
            }

            // 버튼 생성
            CreateButton(panelObj.transform, "SettingButton", "설정", 50);
            CreateButton(panelObj.transform, "CharacterButton", "캐릭터 치장", 50);
            CreateButton(panelObj.transform, "BackgroundButton", "배경", 50);
            CreateButton(panelObj.transform, "SpeedButton", "터치 2 배속", 50);
            CreateButton(panelObj.transform, "ItemButton", "점수 +100~500", 50);
            CreateButton(panelObj.transform, "ResetButton", "게임 초기화", 50);
        }

        private static void CreateTouchFunctionPanel(Transform canvas)
        {
            GameObject panelObj = GameObject.Find("TouchFunctionPanel");
            
            if (panelObj == null)
            {
                panelObj = new GameObject("TouchFunctionPanel");
                panelObj.transform.SetParent(canvas, false);

                RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(1, 0.5f);
                rectTransform.sizeDelta = new Vector2(500, 0);
                rectTransform.localPosition = new Vector3(-10, 0, 0);

                Image image = panelObj.AddComponent<Image>();
                image.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);

                // Title
                GameObject titleObj = new GameObject("Title");
                titleObj.transform.SetParent(panelObj.transform, false);
                RectTransform titleRect = titleObj.AddComponent<RectTransform>();
                titleRect.anchorMin = new Vector2(0, 1);
                titleRect.anchorMax = new Vector2(1, 1);
                titleRect.pivot = new Vector2(0.5f, 1);
                titleRect.sizeDelta = new Vector2(-20, 60);
                titleRect.localPosition = new Vector3(0, -10, 0);

                Text titleText = titleObj.AddComponent<Text>();
                titleText.text = "터치 강화";
                titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                titleText.fontSize = 40;
                titleText.color = Color.white;
                titleText.alignment = TextAnchor.UpperCenter;

                // PointsText
                GameObject pointsObj = new GameObject("PointsText");
                pointsObj.transform.SetParent(panelObj.transform, false);
                RectTransform pointsRect = pointsObj.AddComponent<RectTransform>();
                pointsRect.anchorMin = new Vector2(0, 1);
                pointsRect.anchorMax = new Vector2(1, 1);
                pointsRect.pivot = new Vector2(0.5f, 1);
                pointsRect.sizeDelta = new Vector2(-20, 40);
                pointsRect.localPosition = new Vector3(0, -70, 0);

                Text pointsText = pointsObj.AddComponent<Text>();
                pointsText.text = "+1/클릭";
                pointsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                pointsText.fontSize = 28;
                pointsText.color = Color.yellow;
                pointsText.alignment = TextAnchor.UpperCenter;

                // ScrollView
                GameObject scrollObj = new GameObject("ScrollView");
                scrollObj.transform.SetParent(panelObj.transform, false);
                RectTransform scrollRect = scrollObj.AddComponent<RectTransform>();
                scrollRect.anchorMin = new Vector2(0, 0);
                scrollRect.anchorMax = new Vector2(1, 0);
                scrollRect.pivot = new Vector2(0.5f, 0);
                scrollRect.sizeDelta = new Vector2(-20, -120);
                scrollRect.localPosition = new Vector3(0, 10, 0);

                GameObject viewportObj = new GameObject("Viewport");
                viewportObj.transform.SetParent(scrollObj.transform, false);
                RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
                viewportRect.anchorMin = Vector2.zero;
                viewportRect.anchorMax = Vector2.one;
                viewportRect.sizeDelta = new Vector2(-20, 0);
                viewportObj.AddComponent<RectMask2D>();

                Image viewportImage = viewportObj.AddComponent<Image>();
                viewportImage.color = new Color(0, 0, 0, 0);

GameObject contentObj = new GameObject("Content");
                contentObj.transform.SetParent(viewportObj.transform, false);
                RectTransform contentRect = contentObj.AddComponent<RectTransform>();
                contentRect.anchorMin = new Vector2(0, 1);
                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.pivot = new Vector2(0.5f, 1);
                contentRect.sizeDelta = new Vector2(-20, 100);
                contentRect.localPosition = new Vector3(0, 0, 0);

                VerticalLayoutGroup contentLayout = contentObj.AddComponent<VerticalLayoutGroup>();
                contentLayout.padding = new RectOffset(10, 10, 10, 10);
                contentLayout.spacing = 5;
                contentLayout.childAlignment = TextAnchor.UpperCenter;
                contentLayout.childControlWidth = true;
                contentLayout.childControlHeight = false;

                contentObj.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                // Scrollbar
                GameObject scrollbarObj = new GameObject("Scrollbar");
                scrollbarObj.transform.SetParent(scrollObj.transform, false);
                Scrollbar scrollbar = scrollbarObj.AddComponent<Scrollbar>();
                scrollbarObj.AddComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);

                ScrollRect scrollComponent = scrollObj.AddComponent<ScrollRect>();
                scrollComponent.content = contentRect;
                scrollComponent.viewport = viewportRect;
                scrollComponent.verticalScrollbar = scrollbar;

                Debug.Log("[Phase3Complete] Created TouchFunctionPanel");
            }

            // 컴포넌트 추가
            var listView = panelObj.GetComponent<TouchFunctionListView>();
            if (listView == null)
                listView = panelObj.AddComponent<TouchFunctionListView>();

            var listManager = panelObj.GetComponent<TouchFunctionListManager>();
            if (listManager == null)
                listManager = panelObj.AddComponent<TouchFunctionListManager>();

            // UI 연결
            ConnectTouchFunctionPanel(panelObj);
        }

        private static void ConnectTouchFunctionPanel(GameObject panelObj)
        {
            var listView = panelObj.GetComponent<TouchFunctionListView>();
            var so = new SerializedObject(listView);

            Transform scrollView = panelObj.transform.Find("ScrollView");
            if (scrollView != null)
            {
                Transform viewport = scrollView.Find("Viewport");
                if (viewport != null)
                {
                    Transform content = viewport.Find("Content");
                    if (content != null)
                    {
                        so.FindProperty("contentParent").objectReferenceValue = content;
                        so.FindProperty("scrollRect").objectReferenceValue = scrollView.GetComponent<ScrollRect>();
                        so.FindProperty("pointsText").objectReferenceValue = panelObj.transform.Find("PointsText").GetComponent<Text>();
                    }
                }
            }

            // Prefab 찾기
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TouchFunctionListItem.prefab");
            if (prefab != null)
            {
                var listItem = prefab.GetComponent<TouchFunctionListItem>();
                so.FindProperty("itemPrefab").objectReferenceValue = listItem;
            }

            so.ApplyModifiedProperties();
            Debug.Log("[Phase3Complete] Connected TouchFunctionPanel UI");
        }

        private static void CreateUIManager(Transform canvas)
        {
            var uiManagerObj = GameObject.Find("UIManager");
            
            if (uiManagerObj == null)
            {
                uiManagerObj = new GameObject("UIManager");
                uiManagerObj.AddComponent<UIManager>();
                Debug.Log("[Phase3Complete] Created UIManager");
            }

            var uiManager = uiManagerObj.GetComponent<UIManager>();
            var so = new SerializedObject(uiManager);

            // 버튼 연결
            ConnectButton(so, "LeftButtonPanel/SettingButton", canvas, "settingButton");
            ConnectButton(so, "LeftButtonPanel/CharacterButton", canvas, "characterCustomizeButton");
            ConnectButton(so, "LeftButtonPanel/BackgroundButton", canvas, "backgroundButton");
            ConnectButton(so, "LeftButtonPanel/SpeedButton", canvas, "speedBoostButton");
            ConnectButton(so, "LeftButtonPanel/ItemButton", canvas, "itemButton");
            ConnectButton(so, "LeftButtonPanel/ResetButton", canvas, "resetButton");

            so.ApplyModifiedProperties();
            Debug.Log("[Phase3Complete] Connected UIManager buttons");
        }

        private static void CreateButton(Transform parent, string name, string text, int height)
        {
            GameObject buttonObj = GameObject.Find(name);
            
            if (buttonObj == null)
            {
                buttonObj = new GameObject(name);
                buttonObj.transform.SetParent(parent, false);

                RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(160, height);

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
        }

        private static void ConnectButton(SerializedObject so, string buttonPath, Transform canvas, string fieldName)
        {
            Transform buttonTransform = canvas.Find(buttonPath);
            
            if (buttonTransform != null)
            {
                var button = buttonTransform.GetComponent<Button>();
                if (button != null)
                {
                    so.FindProperty(fieldName).objectReferenceValue = button;
                }
            }
        }
    }
}