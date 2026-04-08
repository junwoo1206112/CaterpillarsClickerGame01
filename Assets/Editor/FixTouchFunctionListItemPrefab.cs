using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace ClickerGame.EditorTools
{
    public class FixTouchFunctionListItemPrefab
    {
        [MenuItem("Tools/Game/🔧 Fix TouchFunctionListItem Prefab")]
        public static void FixPrefab()
        {
            string prefabPath = "Assets/Prefabs/TouchFunctionListItem.prefab";
            
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefab == null)
            {
                Debug.LogError("[FixPrefab] Prefab not found!");
                return;
            }
            
            // Root RectTransform
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(600, 100);
            
            // Image Raycast Target 끄기
            var image = prefab.GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = false;
            }
            
            // AddButton 위치 수정
            var addButton = prefab.transform.Find("AddButton");
            if (addButton != null)
            {
                var addRect = addButton.GetComponent<RectTransform>();
                addRect.anchorMin = new Vector2(1, 0.5f);
                addRect.anchorMax = new Vector2(1, 0.5f);
                addRect.pivot = new Vector2(1, 0.5f);
                addRect.sizeDelta = new Vector2(40, 40);
                addRect.anchoredPosition = new Vector2(-50, 0);
                
                // Button Image Raycast Target 끄기
                var addImage = addButton.GetComponent<Image>();
                if (addImage != null)
                {
                    addImage.raycastTarget = false;
                }
            }
            
            // RemoveButton 위치 수정
            var removeButton = prefab.transform.Find("RemoveButton");
            if (removeButton != null)
            {
                var removeRect = removeButton.GetComponent<RectTransform>();
                removeRect.anchorMin = new Vector2(1, 0.5f);
                removeRect.anchorMax = new Vector2(1, 0.5f);
                removeRect.pivot = new Vector2(1, 0.5f);
                removeRect.sizeDelta = new Vector2(40, 40);
                removeRect.anchoredPosition = new Vector2(-50, 0);
                
                var removeImage = removeButton.GetComponent<Image>();
                if (removeImage != null)
                {
                    removeImage.raycastTarget = false;
                }
            }
            
            // NameText 위치 수정
            var nameText = prefab.transform.Find("NameText");
            if (nameText != null)
            {
                var nameRect = nameText.GetComponent<RectTransform>();
                nameRect.anchorMin = new Vector2(0, 1);
                nameRect.anchorMax = new Vector2(0, 1);
                nameRect.pivot = new Vector2(0, 1);
                nameRect.sizeDelta = new Vector2(250, 30);
                nameRect.anchoredPosition = new Vector2(10, -10);
            }
            
            // CostText 위치 수정
            var costText = prefab.transform.Find("CostText");
            if (costText != null)
            {
                var costRect = costText.GetComponent<RectTransform>();
                costRect.anchorMin = new Vector2(1, 1);
                costRect.anchorMax = new Vector2(1, 1);
                costRect.pivot = new Vector2(1, 1);
                costRect.sizeDelta = new Vector2(100, 30);
                costRect.anchoredPosition = new Vector2(-10, -10);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[FixPrefab] ✅ TouchFunctionListItem prefab fixed!");
            EditorUtility.DisplayDialog("완료", 
                "프리팹 UI 가 수정되었습니다!\n\n" +
                "1. Play 버튼을 누르세요\n" +
                "2. 오른쪽 패널에서 [+] 버튼 확인\n" +
                "3. 클릭 테스트", 
                "OK");
        }
    }
}