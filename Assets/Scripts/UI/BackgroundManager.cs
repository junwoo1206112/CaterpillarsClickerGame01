using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class BackgroundManager : MonoBehaviour
    {
        [System.Serializable]
        public class BackgroundItem
        {
            public string name;
            public Sprite background;
        }
        
        [Header("Backgrounds")]
        [SerializeField] private BackgroundItem[] backgrounds;
        
        [Header("UI")]
        [SerializeField] private Transform backgroundsGrid;
        [SerializeField] private Button closeButton;
        
        [Header("Target")]
        [SerializeField] private Image targetBackground;
        
        private void RefreshBackgrounds()
        {
            // 그리드 초기화
            foreach (Transform child in backgroundsGrid)
            {
                Destroy(child.gameObject);
            }
            
            // 배경 생성
            foreach (var bg in backgrounds)
            {
                CreateBackgroundButton(bg);
            }
        }
        
        private void CreateBackgroundButton(BackgroundItem bg)
        {
            GameObject buttonObj = new GameObject("Background_" + bg.name);
            buttonObj.transform.SetParent(backgroundsGrid, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            
            text.text = bg.name;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => OnBackgroundSelected(bg));
        }
        
        public void OnBackgroundSelected(BackgroundItem bg)
        {
            Debug.Log($"[Background] Selected: {bg.name}");
            
            if (targetBackground != null && bg.background != null)
            {
                targetBackground.sprite = bg.background;
            }
            
            OnCloseClicked();
        }
        
        public void OnCloseClicked()
        {
            var uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.CloseAllWindows();
            }
        }
        
        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            RefreshBackgrounds();
        }
    }
}
