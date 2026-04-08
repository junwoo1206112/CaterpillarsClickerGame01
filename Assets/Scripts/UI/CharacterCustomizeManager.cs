using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class CharacterCustomizeManager : MonoBehaviour
    {
        [System.Serializable]
        public class CustomizeItem
        {
            public string name;
            public SpriteRenderer accessory; // 이 액세서리 킬 거
        }
        
        [Header("Accessories")]
        [SerializeField] private CustomizeItem[] items;
        
        [Header("UI")]
        [SerializeField] private Transform itemsGrid;
        [SerializeField] private Button closeButton;
        
        private void RefreshItems()
        {
            // 그리드 초기화
            foreach (Transform child in itemsGrid)
            {
                Destroy(child.gameObject);
            }
            
            // 아이템 생성
            foreach (var item in items)
            {
                CreateItemButton(item);
            }
        }
        
        private void CreateItemButton(CustomizeItem item)
        {
            GameObject buttonObj = new GameObject("Item_" + item.name);
            buttonObj.transform.SetParent(itemsGrid, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            
            text.text = item.name;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => OnItemSelected(item));
        }
        
        public void OnItemSelected(CustomizeItem item)
        {
            Debug.Log($"[Customize] Selected: {item.name}");
            
            // 모든 액세서리 끄기
            foreach (var acc in items)
            {
                if (acc.accessory != null)
                    acc.accessory.enabled = false;
            }
            
            // 선택한 액세서리 켜기
            if (item.accessory != null)
            {
                item.accessory.enabled = true;
            }
            
            OnCloseClicked();
        }
        
        public void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            RefreshItems();
        }
    }
}
