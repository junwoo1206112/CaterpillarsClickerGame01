using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Data.Models;

namespace ClickerGame.UI
{
    public class TouchFunctionListItem : MonoBehaviour
    {
        [Header("UI References")]
        public Text nameText;
        public Text descriptionText;
        public Text costText;
        public Text levelText;
        public Text pointsText;
        public Button addButton;
        public Button removeButton;
        
        private TouchFunctionData _data;
        private bool _isActive = false;
        
        public void Initialize(TouchFunctionData data)
        {
            _data = data;
            UpdateUI();
            SetupButtons();
        }
        
        public void SetActiveState(bool isActive)
        {
            _isActive = isActive;
            UpdateUI();
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
            
            if (pointsText != null)
            {
                var listManager = TouchFunctionListManager.Instance;
                if (listManager != null)
                    pointsText.text = $"Points: {listManager.TouchPoints}";
            }
            
            if (addButton != null)
                addButton.gameObject.SetActive(!_isActive);
            
            if (removeButton != null)
                removeButton.gameObject.SetActive(_isActive);
        }
        
        private void SetupButtons()
        {
            if (addButton != null)
            {
                addButton.onClick.RemoveAllListeners();
                addButton.onClick.AddListener(() =>
                {
                    if (TouchFunctionListManager.Instance != null)
                    {
                        TouchFunctionListManager.Instance.AddFunction(_data.ID);
                    }
                });
            }
            
            if (removeButton != null)
            {
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(() =>
                {
                    if (TouchFunctionListManager.Instance != null)
                    {
                        TouchFunctionListManager.Instance.RemoveFunction(_data.ID);
                    }
                });
            }
        }
        
        public void Refresh()
        {
            UpdateUI();
        }
    }
}
