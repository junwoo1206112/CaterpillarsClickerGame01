using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class TouchFunctionPanelSetup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Touch Function Panel")]
        public static void SetupTouchFunctionPanel()
        {
            Debug.Log("[TouchFunctionPanelSetup] Starting setup...");

            CreateTouchFunctionPanel();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[TouchFunctionPanelSetup] Setup complete!");
        }

        private static void CreateTouchFunctionPanel()
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[TouchFunctionPanelSetup] Canvas not found!");
                return;
            }

            // 오른쪽 패널 생성
            GameObject panelObj = new GameObject("TouchFunctionPanel");
            panelObj.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.localPosition = new Vector3(0, 0, 0);

            // Image 컴포넌트 (배경)
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

            // 스크롤 뷰 생성
            CreateScrollView(panelObj.transform);

            // 제목
            CreateTitle(panelObj.transform);

            Debug.Log("[TouchFunctionPanelSetup] TouchFunctionPanel created");
        }

        private static void CreateScrollView(Transform parent)
        {
            GameObject scrollViewObj = new GameObject("ScrollView");
            scrollViewObj.transform.SetParent(parent, false);

            RectTransform scrollRect = scrollViewObj.AddComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 1);
            scrollRect.sizeDelta = new Vector2(-20, -60);
            scrollRect.localPosition = new Vector3(0, -30, 0);

            // ScrollRect 컴포넌트
            ScrollRect scrollRectComponent = scrollViewObj.AddComponent<ScrollRect>();
            scrollRectComponent.horizontal = false;
            scrollRectComponent.vertical = true;

            // Viewport
            GameObject viewportObj = new GameObject("Viewport");
            viewportObj.transform.SetParent(scrollViewObj.transform, false);

            RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
            viewportRect.anchorMin = new Vector2(0, 0);
            viewportRect.anchorMax = new Vector2(1, 1);
            viewportRect.sizeDelta = new Vector2(-17, 0);
            viewportRect.localPosition = new Vector3(0, 0, 0);

            Image viewportImage = viewportObj.AddComponent<Image>();
            viewportImage.color = new Color(0.3f, 0.3f, 0.3f, 1);

            Mask viewportMask = viewportObj.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;

            // Content
            GameObject contentObj = new GameObject("Content");
            contentObj.transform.SetParent(viewportObj.transform, false);

            RectTransform contentRect = contentObj.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.sizeDelta = new Vector2(0, 300);
            contentRect.localPosition = new Vector3(0, 0, 0);

            // Vertical Layout Group
            VerticalLayoutGroup layoutGroup = contentObj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            layoutGroup.spacing = 5;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;

            // Content Size Fitter
            ContentSizeFitter sizeFitter = contentObj.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRectComponent.content = contentRect;
            scrollRectComponent.viewport = viewportRect;

            Debug.Log("[TouchFunctionPanelSetup] ScrollView created");
        }

        private static void CreateTitle(Transform parent)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(parent, false);

            RectTransform titleRect = titleObj.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = new Vector2(-20, 40);
            titleRect.localPosition = new Vector3(0, -20, 0);

            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "터치 강화";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;

            Debug.Log("[TouchFunctionPanelSetup] Title created");
        }

        [MenuItem("Tools/Game/Create Touch Function Item Prefab")]
        public static void CreateTouchFunctionItemPrefab()
        {
            string path = "Assets/Prefabs/TouchFunctionListItem.prefab";
            
            GameObject itemObj = new GameObject("TouchFunctionListItem");
            
            RectTransform rectTransform = itemObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(260, 80);
            
            // 배경
            Image background = itemObj.AddComponent<Image>();
            background.color = new Color(0.4f, 0.4f, 0.4f, 1);
            
            // 함수명
            CreateLabelText(itemObj.transform, "FunctionName", "함수명", new Vector2(10, -15), 16, Color.white);
            
            // 설명
            CreateLabelText(itemObj.transform, "Description", "설명", new Vector2(10, -35), 12, new Color(0.8f, 0.8f, 0.8f));
            
            // 레벨
            CreateLabelText(itemObj.transform, "Level", "Lv. 1", new Vector2(10, -55), 12, Color.yellow);
            
            // 추가 버튼
            CreateButton(itemObj.transform, "AddButton", "+", new Vector2(220, -20), 30, 30);
            
            // 삭제 버튼
            CreateButton(itemObj.transform, "RemoveButton", "-", new Vector2(180, -20), 30, 30);
            
            // 컴포넌트 추가
            itemObj.AddComponent<TouchFunctionListItem>();
            
            // 프리팹으로 저장
            System.IO.Directory.CreateDirectory("Assets/Prefabs");
            PrefabUtility.SaveAsPrefabAsset(itemObj, path);
            
            Debug.Log($"[TouchFunctionPanelSetup] Prefab created: {path}");
            
            // 정리
            Object.DestroyImmediate(itemObj);
        }

        private static void CreateLabelText(Transform parent, string name, string text, Vector2 position, int fontSize, Color color)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
            
            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(200, 20);
            rectTransform.localPosition = position;
            
            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = fontSize;
            uiText.color = color;
            uiText.alignment = TextAnchor.MiddleLeft;
        }

        private static void CreateButton(Transform parent, string name, string text, Vector2 position, int width, int height)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.localPosition = position;
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.5f, 0.5f, 0.5f, 1);
            
            Button button = buttonObj.AddComponent<Button>();
            
            // 버튼 텍스트
            GameObject childText = new GameObject("Text");
            childText.transform.SetParent(buttonObj.transform, false);
            
            RectTransform textRect = childText.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Text uiText = childText.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 18;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }
    }
}
