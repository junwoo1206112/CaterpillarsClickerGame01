using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class Phase3Setup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Phase 3 (Complete)")]
        public static void SetupPhase3Complete()
        {
            if (!EditorUtility.DisplayDialog("Phase 3 Setup",
                "Phase 3 전체 설정을 진행합니다:\n\n1. Prefab 생성\n2. Scene UI 연결\n\n계속하시겠습니까?",
                "실행", "취소"))
                return;

            Debug.Log("[Phase3Setup] Starting Phase 3 setup...");

            CreatePrefab();
            ConnectSceneUI();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[Phase3Setup] Phase 3 setup complete!");
            EditorUtility.DisplayDialog("Phase 3 Setup", "Phase 3 설정 완료!\n\n게임을 실행하여 테스트하세요.", "확인");
        }

        [MenuItem("Tools/Game/Create TouchFunctionListItem Prefab")]
        public static void CreatePrefab()
        {
            string prefabPath = "Assets/Prefabs/TouchFunctionListItem.prefab";

            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
            {
                Debug.Log($"[Phase3Setup] Prefab already exists: {prefabPath}");
                return;
            }

            GameObject itemObj = new GameObject("TouchFunctionListItem");

            RectTransform rectTransform = itemObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(600, 100);

            Image background = itemObj.AddComponent<Image>();
            background.color = new Color(0.25f, 0.25f, 0.25f, 1f);

            CreateText(itemObj.transform, "NameText", "함수명", new Vector2(20, -25), 250, 30, 24, Color.white);
            CreateText(itemObj.transform, "DescriptionText", "설명", new Vector2(20, -55), 400, 25, 16, new Color(0.7f, 0.7f, 0.7f));
            CreateText(itemObj.transform, "CostText", "50 pts", new Vector2(450, -25), 100, 30, 18, Color.yellow);
            CreateText(itemObj.transform, "LevelText", "Lv. 1", new Vector2(450, -55), 100, 25, 14, Color.gray);
            CreateText(itemObj.transform, "PointsText", "Points: 0", new Vector2(20, -85), 200, 20, 14, Color.green);

            CreateButton(itemObj.transform, "AddButton", "+", new Vector2(550, -30), 40, 40, Color.green);
            CreateButton(itemObj.transform, "RemoveButton", "-", new Vector2(550, -70), 40, 40, Color.red);

            var listItem = itemObj.AddComponent<TouchFunctionListItem>();

            var so = new SerializedObject(listItem);
            so.FindProperty("nameText").objectReferenceValue = itemObj.transform.Find("NameText").GetComponent<Text>();
            so.FindProperty("descriptionText").objectReferenceValue = itemObj.transform.Find("DescriptionText").GetComponent<Text>();
            so.FindProperty("costText").objectReferenceValue = itemObj.transform.Find("CostText").GetComponent<Text>();
            so.FindProperty("levelText").objectReferenceValue = itemObj.transform.Find("LevelText").GetComponent<Text>();
            so.FindProperty("pointsText").objectReferenceValue = itemObj.transform.Find("PointsText").GetComponent<Text>();
            so.FindProperty("addButton").objectReferenceValue = itemObj.transform.Find("AddButton").GetComponent<Button>();
            so.FindProperty("removeButton").objectReferenceValue = itemObj.transform.Find("RemoveButton").GetComponent<Button>();
            so.ApplyModifiedProperties();

            System.IO.Directory.CreateDirectory("Assets/Prefabs");
            PrefabUtility.SaveAsPrefabAsset(itemObj, prefabPath);

            Debug.Log($"[Phase3Setup] Prefab created: {prefabPath}");

            Object.DestroyImmediate(itemObj);
        }

        [MenuItem("Tools/Game/Connect Scene UI")]
        public static void ConnectSceneUI()
        {
            GameObject panelObj = GameObject.Find("TouchFunctionPanel");

            if (panelObj == null)
            {
                Debug.LogError("[Phase3Setup] TouchFunctionPanel not found in scene!");
                return;
            }

            TouchFunctionListView listView = panelObj.GetComponent<TouchFunctionListView>();
            if (listView == null)
            {
                listView = panelObj.AddComponent<TouchFunctionListView>();
            }

            var so = new SerializedObject(listView);

            Transform scrollTransform = panelObj.transform.Find("Scroll View");
            if (scrollTransform != null)
            {
                so.FindProperty("scrollRect").objectReferenceValue = scrollTransform.GetComponent<ScrollRect>();

                Transform viewportTransform = scrollTransform.Find("Viewport");
                if (viewportTransform != null)
                {
                    Transform contentTransform = viewportTransform.Find("Content");
                    if (contentTransform != null)
                    {
                        so.FindProperty("contentParent").objectReferenceValue = contentTransform;

                        VerticalLayoutGroup layoutGroup = contentTransform.GetComponent<VerticalLayoutGroup>();
                        if (layoutGroup == null)
                        {
                            layoutGroup = contentTransform.gameObject.AddComponent<VerticalLayoutGroup>();
                            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
                            layoutGroup.spacing = 5;
                            layoutGroup.childAlignment = TextAnchor.UpperCenter;
                            layoutGroup.childControlWidth = true;
                            layoutGroup.childControlHeight = false;
                        }

                        ContentSizeFitter sizeFitter = contentTransform.GetComponent<ContentSizeFitter>();
                        if (sizeFitter == null)
                        {
                            sizeFitter = contentTransform.gameObject.AddComponent<ContentSizeFitter>();
                            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                        }
                    }
                }
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TouchFunctionListItem.prefab");
            if (prefab != null)
            {
                so.FindProperty("itemPrefab").objectReferenceValue = prefab.GetComponent<TouchFunctionListItem>();
            }
            else
            {
                Debug.LogWarning("[Phase3Setup] Prefab not found! Run 'Create Prefab' first.");
            }

            so.ApplyModifiedProperties();

            TouchFunctionListManager listManager = panelObj.GetComponent<TouchFunctionListManager>();
            if (listManager == null)
            {
                listManager = panelObj.AddComponent<TouchFunctionListManager>();
            }

            GameObject upgradeListItem = GameObject.Find("UpgradeListItem");
            if (upgradeListItem != null)
            {
                Object.DestroyImmediate(upgradeListItem);
                Debug.Log("[Phase3Setup] Removed 'UpgradeListItem' from scene");
            }

            GameObject gameObjectObj = GameObject.Find("GameObject");
            if (gameObjectObj != null)
            {
                Object.DestroyImmediate(gameObjectObj);
                Debug.Log("[Phase3Setup] Removed 'GameObject' from scene");
            }

            Debug.Log("[Phase3Setup] Scene UI connected");
        }

        private static void CreateText(Transform parent, string name, string text, Vector2 position, int width, int height, int fontSize, Color color)
        {
            GameObject textObj = new GameObject(name);
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
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.localPosition = position;

            Image image = buttonObj.AddComponent<Image>();
            image.color = color;

            Button button = buttonObj.AddComponent<Button>();

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