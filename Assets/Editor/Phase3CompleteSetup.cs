using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class Phase3CompleteSetup
    {
        [MenuItem("Tools/Game/Complete Phase 3 Setup")]
        public static void CompletePhase3Setup()
        {
            Debug.Log("[Phase3CompleteSetup] Starting Phase 3 setup...");

            // 1. Prefab 확인
            string prefabPath = "Assets/Prefabs/TouchFunctionListItem.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefab == null)
            {
                Debug.LogError("[Phase3CompleteSetup] Prefab not found!");
                Debug.LogError("[Phase3CompleteSetup] Run: Tools > Game > Create TouchFunctionListItem Prefab");
                return;
            }
            else
            {
                Debug.Log($"[Phase3CompleteSetup] Prefab found: {prefabPath}");
            }

            // 2. TouchFunctionPanel 찾기 또는 생성
            GameObject panelObj = GameObject.Find("TouchFunctionPanel");
            
            if (panelObj == null)
            {
                Debug.Log("[Phase3CompleteSetup] Creating TouchFunctionPanel...");
                panelObj = CreateTouchFunctionPanel();
            }
            else
            {
                Debug.Log("[Phase3CompleteSetup] TouchFunctionPanel found");
            }

            // 3. 컴포넌트 추가
            TouchFunctionListView listView = panelObj.GetComponent<TouchFunctionListView>();
            if (listView == null)
            {
                listView = panelObj.AddComponent<TouchFunctionListView>();
                Debug.Log("[Phase3CompleteSetup] Added TouchFunctionListView");
            }

            TouchFunctionListManager listManager = panelObj.GetComponent<TouchFunctionListManager>();
            if (listManager == null)
            {
                listManager = panelObj.AddComponent<TouchFunctionListManager>();
                Debug.Log("[Phase3CompleteSetup] Added TouchFunctionListManager");
            }

            // 4. UI 연결
            ConnectUI(panelObj, prefab);

            // 5. Scene 저장
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();

            Debug.Log("[Phase3CompleteSetup] Phase 3 setup complete!");
            EditorUtility.DisplayDialog("Phase 3 Complete", 
                "Phase 3 터치 강화 시스템이 완료되었습니다!\n\n" +
                "Play 버튼을 눌러 테스트하세요.", 
                "OK");
        }

        private static GameObject CreateTouchFunctionPanel()
        {
            var canvas = Object.FindFirstObjectByType<Canvas>();
            
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                Debug.Log("[Phase3CompleteSetup] Created Canvas");
            }

            GameObject panelObj = new GameObject("TouchFunctionPanel");
            panelObj.transform.SetParent(canvas.transform, false);

            RectTransform rectTransform = panelObj.GetComponent<RectTransform>();
            if (rectTransform == null)
                rectTransform = panelObj.AddComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.sizeDelta = new Vector2(700, 0);
            rectTransform.localPosition = Vector3.zero;

            Image image = panelObj.AddComponent<Image>();
            image.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);

            CreateTitle(panelObj.transform);
            CreatePointsText(panelObj.transform);
            CreateScrollView(panelObj.transform);

            return panelObj;
        }

        private static void CreateTitle(Transform parent)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(parent, false);

            RectTransform rectTransform = titleObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.sizeDelta = new Vector2(-40, 80);
            rectTransform.localPosition = new Vector3(0, -20, 0);

            Text text = titleObj.AddComponent<Text>();
            text.text = "터치 강화";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 50;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperCenter;
        }

        private static void CreatePointsText(Transform parent)
        {
            GameObject pointsObj = new GameObject("PointsText");
            pointsObj.transform.SetParent(parent, false);

            RectTransform rectTransform = pointsObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.sizeDelta = new Vector2(-40, 50);
            rectTransform.localPosition = new Vector3(0, -100, 0);

            Text text = pointsObj.AddComponent<Text>();
            text.text = "+1/클릭";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 36;
            text.color = Color.yellow;
            text.alignment = TextAnchor.UpperCenter;
        }

        private static void CreateScrollView(Transform parent)
        {
            GameObject scrollObj = new GameObject("ScrollView");
            scrollObj.transform.SetParent(parent, false);
            scrollObj.AddComponent<RectTransform>();

            GameObject viewportObj = new GameObject("Viewport");
            viewportObj.transform.SetParent(scrollObj.transform, false);
            viewportObj.AddComponent<RectTransform>();
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

            VerticalLayoutGroup layoutGroup = contentObj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            layoutGroup.spacing = 5;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;

            contentObj.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            GameObject scrollbarObj = new GameObject("Scrollbar Vertical");
            scrollbarObj.transform.SetParent(scrollObj.transform, false);
            scrollbarObj.AddComponent<Scrollbar>();

            ScrollRect scrollRect = scrollObj.AddComponent<ScrollRect>();
            scrollRect.content = contentRect;
            scrollRect.viewport = viewportObj.GetComponent<RectTransform>();
            scrollRect.verticalScrollbar = scrollbarObj.GetComponent<Scrollbar>();
        }

        private static void ConnectUI(GameObject panelObj, GameObject prefab)
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
                    }
                }
            }

            if (prefab != null)
            {
                var listItem = prefab.GetComponent<TouchFunctionListItem>();
                so.FindProperty("itemPrefab").objectReferenceValue = listItem;
            }

            so.ApplyModifiedProperties();

            Debug.Log("[Phase3CompleteSetup] UI connected");
        }
    }
}