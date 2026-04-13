using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using ClickerGame.UI;

namespace ClickerGame.Editor
{
    public class BackgroundPanelSetup
    {
        [MenuItem("Tools/Game/Setup Background Panel")]
        public static void Setup()
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[BackgroundPanelSetup] Canvas가 씬에 없습니다.");
                return;
            }

            var existingObj = GameObject.Find("BackgroundPanel");
            if (existingObj != null)
            {
                Object.DestroyImmediate(existingObj);
            }

            var snowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BackgroundParticle/Snow.prefab");
            var rainPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BackgroundParticle/Rain.prefab");

            if (snowPrefab == null)
            {
                var snowGuids = AssetDatabase.FindAssets("Snow t:Prefab", new[] { "Assets/Prefabs" });
                if (snowGuids.Length > 0)
                    snowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(snowGuids[0]));
            }
            if (rainPrefab == null)
            {
                var rainGuids = AssetDatabase.FindAssets("Rain t:Prefab", new[] { "Assets/Prefabs" });
                if (rainGuids.Length > 0)
                    rainPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(rainGuids[0]));
            }

            Debug.Log($"[BackgroundPanelSetup] Snow prefab: {(snowPrefab != null ? snowPrefab.name : "NULL")}");
            Debug.Log($"[BackgroundPanelSetup] Rain prefab: {(rainPrefab != null ? rainPrefab.name : "NULL")}");

            GameObject panel = CreatePanel(canvas.transform);
            var bgManager = panel.AddComponent<BackgroundManager>();

            var content = panel.transform.Find("Scroll View/Viewport/Content");

            var serializedObj = new SerializedObject(bgManager);
            serializedObj.Update();

            var backgroundsProp = serializedObj.FindProperty("backgrounds");
            int count = 0;
            if (snowPrefab != null) count++;
            if (rainPrefab != null) count++;
            backgroundsProp.arraySize = count;

            int index = 0;
            if (snowPrefab != null)
            {
                var snowItem = backgroundsProp.GetArrayElementAtIndex(index);
                snowItem.FindPropertyRelative("name").stringValue = "Snow";
                snowItem.FindPropertyRelative("particlePrefab").objectReferenceValue = snowPrefab;
                index++;
            }

            if (rainPrefab != null)
            {
                var rainItem = backgroundsProp.GetArrayElementAtIndex(index);
                rainItem.FindPropertyRelative("name").stringValue = "Rain";
                rainItem.FindPropertyRelative("particlePrefab").objectReferenceValue = rainPrefab;
            }

            serializedObj.FindProperty("backgroundsGrid").objectReferenceValue = content;
            serializedObj.FindProperty("closeButton").objectReferenceValue = panel.transform.Find("CloseButton").GetComponent<Button>();

            serializedObj.ApplyModifiedProperties();

            Selection.activeGameObject = panel;

            Debug.Log("[BackgroundPanelSetup] 완료! BackgroundPanel이 씬에 추가되었습니다.");
            Debug.Log("[BackgroundPanelSetup] 씬을 저장하세요 (Ctrl+S)");
        }

        private static GameObject CreatePanel(Transform parent)
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject panel = new GameObject("BackgroundPanel");
            panel.transform.SetParent(parent, false);

            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            Image panelBg = panel.AddComponent<Image>();
            panelBg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(panel.transform, false);
            RectTransform titleRect = titleObj.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0, -20);
            titleRect.sizeDelta = new Vector2(400, 60);
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "날씨 선택";
            titleText.fontSize = 30;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;
            if (font != null) titleText.font = font;

            GameObject closeButtonObj = new GameObject("CloseButton");
            closeButtonObj.transform.SetParent(panel.transform, false);
            RectTransform closeRect = closeButtonObj.AddComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(1f, 1f);
            closeRect.anchorMax = new Vector2(1f, 1f);
            closeRect.pivot = new Vector2(1f, 1f);
            closeRect.anchoredPosition = new Vector2(-10, -10);
            closeRect.sizeDelta = new Vector2(50, 50);
            Image closeBg = closeButtonObj.AddComponent<Image>();
            closeBg.color = new Color(0.8f, 0.2f, 0.2f, 1f);

            GameObject closeTextObj = new GameObject("Text");
            closeTextObj.transform.SetParent(closeButtonObj.transform, false);
            RectTransform closeTextRect = closeTextObj.AddComponent<RectTransform>();
            closeTextRect.anchorMin = Vector2.zero;
            closeTextRect.anchorMax = Vector2.one;
            closeTextRect.offsetMin = Vector2.zero;
            closeTextRect.offsetMax = Vector2.zero;
            Text closeText = closeTextObj.AddComponent<Text>();
            closeText.text = "X";
            closeText.fontSize = 24;
            closeText.color = Color.white;
            closeText.alignment = TextAnchor.MiddleCenter;
            if (font != null) closeText.font = font;
            Button closeBtn = closeButtonObj.AddComponent<Button>();

            GameObject scrollView = new GameObject("Scroll View");
            scrollView.transform.SetParent(panel.transform, false);
            RectTransform scrollRectTrans = scrollView.AddComponent<RectTransform>();
            scrollRectTrans.anchorMin = new Vector2(0, 0);
            scrollRectTrans.anchorMax = new Vector2(1, 1);
            scrollRectTrans.offsetMin = new Vector2(20, 20);
            scrollRectTrans.offsetMax = new Vector2(-20, -90);
            Image scrollBg = scrollView.AddComponent<Image>();
            scrollBg.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            ScrollRect sr = scrollView.AddComponent<ScrollRect>();
            scrollView.AddComponent<RectMask2D>();

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollView.transform, false);
            RectTransform vpRect = viewport.AddComponent<RectTransform>();
            vpRect.anchorMin = Vector2.zero;
            vpRect.anchorMax = Vector2.one;
            vpRect.offsetMin = Vector2.zero;
            vpRect.offsetMax = Vector2.zero;
            viewport.AddComponent<RectMask2D>();

            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0, 200);
            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(10, 10, 10, 10);
            vlg.spacing = 10;
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            sr.content = contentRect;
            sr.viewport = vpRect;
            sr.horizontal = false;
            sr.vertical = true;

            panel.SetActive(true);

            // 비활성화 상태에서도 참조를 유지하기 위해 UIManager에 자동 연결
            var uiManager = Object.FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                var uiSo = new SerializedObject(uiManager);
                uiSo.Update();
                uiSo.FindProperty("backgroundPanel").objectReferenceValue = panel;
                uiSo.ApplyModifiedProperties();
                Debug.Log("[BackgroundPanelSetup] UIManager.backgroundPanel 연결 완료");
            }

            // 씬 저장 후 비활성화는 UIManager가 담당
            Debug.Log("[BackgroundPanelSetup] 완료! BackgroundPanel이 씬에 추가되었습니다.");
            Debug.Log("[BackgroundPanelSetup] 씬을 저장하세요 (Ctrl+S)");

            return panel;
        }
    }
}