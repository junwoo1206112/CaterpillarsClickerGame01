using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Data.Models;

namespace ClickerGame.UI
{
    public class TouchFunctionListItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private Text levelText;
        [SerializeField] private Button addButton;
        
        private TouchFunctionData _data;
        private bool _isActive = false;
        
        private void Awake()
        {
            SetupButtons();
            
            // Image 의 Raycast Target 끄기 (버튼 클릭 통과)
            var image = GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = false;
            }
        }
        
        public void Initialize(TouchFunctionData data)
        {
            _data = data;
            UpdateUI();
        }
        
        public void SetActiveState(bool isActive)
        {
            _isActive = isActive;
            UpdateUI();
        }
        
        private void SetupButtons()
        {
            if (addButton != null)
            {
                addButton.onClick.AddListener(OnAddClicked);
            }
        }
        
        private void OnAddClicked()
        {
            Debug.Log($"[TouchFunctionListItem] Add clicked: {_data?.Name} (ID: {_data?.ID}, Cost: {_data?.Cost})");
            
            if (TouchFunctionListManager.Instance != null && _data != null)
            {
                Debug.Log($"[TouchFunctionListItem] Points: {TouchFunctionListManager.Instance.TouchPoints}, Cost: {_data.Cost}");
                TouchFunctionListManager.Instance.AddFunction(_data.ID);
            }
            else
            {
                if (TouchFunctionListManager.Instance == null)
                    Debug.LogError("[TouchFunctionListItem] TouchFunctionListManager.Instance is null!");
                if (_data == null)
                    Debug.LogError("[TouchFunctionListItem] Data is null!");
            }
        }
        
        private void UpdateUI()
        {
            if (_data == null) return;
            
            if (nameText != null)
                nameText.text = _data.Name;
            
            if (descriptionText != null)
                descriptionText.text = _data.Description;
            
            if (costText != null)
                costText.text = $"{_data.Cost} pts";
            
            if (levelText != null)
                levelText.text = $"Lv. {_data.Level}";
            
            // 활성화되면 버튼 숨기기
            if (addButton != null)
                addButton.gameObject.SetActive(!_isActive);
        }
        
        public void Refresh()
        {
            UpdateUI();
        }
    }
}