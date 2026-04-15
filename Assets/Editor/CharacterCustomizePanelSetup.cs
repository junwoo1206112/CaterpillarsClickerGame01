using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using ClickerGame.UI;

namespace ClickerGame.Editor
{
    public class CharacterCustomizePanelSetup
    {
        [MenuItem("Tools/Game/Setup Character Customize Panel")]
        public static void Setup()
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[CharacterCustomizePanelSetup] Canvas가 씬에 없습니다.");
                return;
            }

            var existingObj = GameObject.Find("CharacterCustomizePanel");
            if (existingObj != null)
            {
                Object.DestroyImmediate(existingObj);
            }

            var sprite1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Graphics/Custmising/sprite_e5ilre2co.png");
            var sprite2 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Graphics/Custmising/sprite_llabpmlo9.png");

            if (sprite1 == null)
            {
                var guids1 = AssetDatabase.FindAssets("sprite_e5ilre2co t:Sprite", new[] { "Assets/Graphics/Custmising" });
                if (guids1.Length > 0)
                    sprite1 = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guids1[0]));
            }
            if (sprite2 == null)
            {
                var guids2 = AssetDatabase.FindAssets("sprite_llabpmlo9 t:Sprite", new[] { "Assets/Graphics/Custmising" });
                if (guids2.Length > 0)
                    sprite2 = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guids2[0]));
            }

            Debug.Log($"[CharacterCustomizePanelSetup] Sprite1: {(sprite1 != null ? sprite1.name : "NULL")}");
            Debug.Log($"[CharacterCustomizePanelSetup] Sprite2: {(sprite2 != null ? sprite2.name : "NULL")}");

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject panel = new GameObject("CharacterCustomizePanel");
            panel.transform.SetParent(canvas.transform, false);

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
            titleText.text = "캐릭터 꾸미기";
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

            GameObject content = new GameObject("Content");
            content.transform.SetParent(panel.transform, false);
            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0.1f, 0);
            contentRect.anchorMax = new Vector2(0.9f, 1);
            contentRect.offsetMin = new Vector2(0, 20);
            contentRect.offsetMax = new Vector2(0, -90);
            VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 15;
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var customizeManager = panel.AddComponent<CharacterCustomizeManager>();

            var serializedObj = new SerializedObject(customizeManager);
            serializedObj.Update();

            var itemsProp = serializedObj.FindProperty("items");
            itemsProp.arraySize = 2;

            var item1 = itemsProp.GetArrayElementAtIndex(0);
            item1.FindPropertyRelative("name").stringValue = "왕관";
            item1.FindPropertyRelative("sprite").objectReferenceValue = sprite1;
            item1.FindPropertyRelative("caterpillarOffset").vector2Value = new Vector2(1.1f, 0.8f);
            item1.FindPropertyRelative("butterflyOffset").vector2Value = new Vector2(0f, 1.0f);

            var item2 = itemsProp.GetArrayElementAtIndex(1);
            item2.FindPropertyRelative("name").stringValue = "모자";
            item2.FindPropertyRelative("sprite").objectReferenceValue = sprite2;
            item2.FindPropertyRelative("caterpillarOffset").vector2Value = new Vector2(1.1f, 0.7f);
            item2.FindPropertyRelative("butterflyOffset").vector2Value = new Vector2(0f, 0.9f);

            serializedObj.FindProperty("itemsGrid").objectReferenceValue = content;
            serializedObj.FindProperty("closeButton").objectReferenceValue = closeBtn;

            serializedObj.ApplyModifiedProperties();

            var uiManager = Object.FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                var uiSo = new SerializedObject(uiManager);
                uiSo.Update();
                uiSo.FindProperty("characterCustomizePanel").objectReferenceValue = panel;
                uiSo.ApplyModifiedProperties();
                Debug.Log("[CharacterCustomizePanelSetup] UIManager.characterCustomizePanel 연결 완료");
            }

            panel.SetActive(false);

            Selection.activeGameObject = panel;

            Debug.Log("[CharacterCustomizePanelSetup] 완료! 캐릭터 꾸미기 패널이 생성되었습니다.");
            Debug.Log("[CharacterCustomizePanelSetup] Inspector에서 CharacterCustomizeManager의 Target Renderer에 캐릭터 SpriteRenderer를 드래그하세요.");
            Debug.Log("[CharacterCustomizePanelSetup] 씬을 저장하세요 (Ctrl+S)");
        }
    }
}