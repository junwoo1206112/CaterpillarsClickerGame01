using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class CreateTouchFunctionPanel : EditorWindow
    {
        [MenuItem("Tools/Game/Create Touch Function Panel")]
        public static void CreatePanel()
        {
            Debug.Log("[TouchFunctionPanel] Creating panel...");
            
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[TouchFunctionPanel] Canvas not found!");
                return;
            }
            
            CreateRightPanel(canvas.transform);
            CreateItemPrefab();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[TouchFunctionPanel] Panel creation complete!");
        }
        
        private static void CreateRightPanel(Transform canvas)
        {
            // 오른쪽 패널
            GameObject panelObj = new GameObject("TouchFunctionPanel");
            panelObj.transform.SetParent(canvas, false);
            
            RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.localPosition = new Vector3(0, 0, 0);
            
            // 배경
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0.15f, 0.15f, 0.15f, 0.95f);
            
            // 제목
            CreateTitle(panelObj.transform);
            
            // 포인트 표시
            CreatePointsText(panelObj.transform);
            
            // 스크롤 뷰
            CreateScrollView(panelObj.transform);
            
            Debug.Log("[TouchFunctionPanel] Right panel created");
        }
        
        private static void CreateTitle(Transform parent)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(parent, false);
            
            RectTransform titleRect = titleObj.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = new Vector2(-20, 50);
            titleRect.localPosition = new Vector3(0, -25, 0);
            
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "터치 강화";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;
        }
        
        private static void CreatePointsText(Transform parent)
        {
            GameObject pointsObj = new GameObject("PointsText");
            pointsObj.transform.SetParent(parent, false);
            
            RectTransform pointsRect = pointsObj.AddComponent<RectTransform>();
            pointsRect.anchorMin = new Vector2(0, 1);
            pointsRect.anchorMax = new Vector2(1, 1);
            pointsRect.sizeDelta = new Vector2(-20, 40);
            pointsRect.localPosition = new Vector3(0, -70, 0);
            
            Text pointsText = pointsObj.AddComponent<Text>();
            pointsText.text = "Points: 0";
            pointsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            pointsText.fontSize = 20;
            pointsText.color = Color.yellow;
            pointsText.alignment = TextAnchor.MiddleCenter;
        }
        
        private static void CreateScrollView(Transform parent)
        {
            GameObject scrollViewObj = new GameObject("ScrollView");
            scrollViewObj.transform.SetParent(parent, false);
            
            RectTransform scrollRect = scrollViewObj.AddComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 1);
            scrollRect.sizeDelta = new Vector2(-10, -130);
            scrollRect.localPosition = new Vector3(0, 10, 0);
            
            ScrollRect scrollRectComponent = scrollViewObj.AddComponent<ScrollRect>();
            scrollRectComponent.horizontal = false;
            scrollRectComponent.vertical = true;
            scrollRectComponent.movementType = ScrollRect.MovementType.Clamped;
            
            // Viewport
            GameObject viewportObj = new GameObject("Viewport");
            viewportObj.transform.SetParent(scrollViewObj.transform, false);
            
            RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
            viewportRect.anchorMin = new Vector2(0, 0);
            viewportRect.anchorMax = new Vector2(1, 1);
            viewportRect.sizeDelta = new Vector2(-17, 0);
            viewportRect.localPosition = new Vector3(0, 0, 0);
            
            Image viewportImage = viewportObj.AddComponent<Image>();
            viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 1);
            
            Mask viewportMask = viewportObj.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;
            
            // Content
            GameObject contentObj = new GameObject("Content");
            contentObj.transform.SetParent(viewportObj.transform, false);
            
            RectTransform contentRect = contentObj.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.sizeDelta = new Vector2(-20, 300);
            contentRect.localPosition = new Vector3(0, 0, 0);
            
            // Vertical Layout Group
            VerticalLayoutGroup layoutGroup = contentObj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            layoutGroup.spacing = 10;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;
            
            // Content Size Fitter
            ContentSizeFitter sizeFitter = contentObj.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            scrollRectComponent.content = contentRect;
            scrollRectComponent.viewport = viewportRect;
            
            Debug.Log("[TouchFunctionPanel] ScrollView created");
        }
        
        private static void CreateItemPrefab()
        {
            string path = "Assets/Prefabs/TouchFunctionListItem.prefab";
            
            GameObject itemObj = new GameObject("TouchFunctionListItem");
            
            RectTransform rectTransform = itemObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(260, 100);
            
            // 배경
            Image background = itemObj.AddComponent<Image>();
            background.color = new Color(0.3f, 0.3f, 0.3f, 1);
            
            // 이름
            CreateLabel(itemObj.transform, "NameText", "함수명", new Vector2(-120, 35), 18, Color.white);
            
            // 설명
            CreateLabel(itemObj.transform, "DescriptionText", "설명", new Vector2(-120, 10), 14, new Color(0.8f, 0.8f, 0.8f));
            
            // 비용
            CreateLabel(itemObj.transform, "CostText", "50 pts", new Vector2(-120, -15), 16, Color.yellow);
            
            // 레벨
            CreateLabel(itemObj.transform, "LevelText", "Lv. 1", new Vector2(-120, -35), 14, Color.gray);
            
            // 포인트
            CreateLabel(itemObj.transform, "PointsText", "Points: 0", new Vector2(0, 40), 12, Color.green);
            
            // 추가 버튼
            CreateButton(itemObj.transform, "AddButton", "+", new Vector2(100, 0), 40, 40, Color.green);
            
            // 삭제 버튼
            CreateButton(itemObj.transform, "RemoveButton", "-", new Vector2(100, 0), 40, 40, Color.red);
            
            // 컴포넌트 추가
            itemObj.AddComponent<TouchFunctionListItem>();
            
            // 폴더 생성
            System.IO.Directory.CreateDirectory("Assets/Prefabs");
            
            // 프리팹으로 저장
            PrefabUtility.SaveAsPrefabAsset(itemObj, path);
            
            Debug.Log($"[TouchFunctionPanel] Prefab created: {path}");
            
            Object.DestroyImmediate(itemObj);
        }
        
        private static void CreateLabel(Transform parent, string name, string text, Vector2 position, int fontSize, Color color)
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
        
        private static void CreateButton(Transform parent, string name, string text, Vector2 position, int width, int height, Color color)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.localPosition = position;
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = color;
            
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
            uiText.fontSize = 24;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;
        }
    }
}
