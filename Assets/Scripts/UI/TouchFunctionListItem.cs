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
        [SerializeField] private Text pointsText;
        [SerializeField] private Text costText;
        [SerializeField] private Text levelText;
        [SerializeField] private Button addButton;
        
        private TouchFunctionData _data;
        private bool _isActive = false;
        
        public string DataID => _data != null ? _data.ID : "";
        
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
        
        private void OnEnable()
        {
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnPointsChanged += UpdatePointsText;
            }
        }
        
        private void OnDisable()
        {
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnPointsChanged -= UpdatePointsText;
            }
        }
        
        private void UpdatePointsText(int points)
        {
            if (pointsText != null)
            {
                pointsText.text = $"현재 포인트: {points} pts";
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
            if (TouchFunctionListManager.Instance != null && _data != null)
            {
                TouchFunctionListManager.Instance.AddFunction(_data.ID);
            }
        }
        
        private void UpdateUI()
        {
            if (_data == null)
            {
                Debug.LogWarning("[TouchFunctionListItem] _data is null!");
                return;
            }
            
            if (nameText != null)
                nameText.text = _data.Name;
            
            if (descriptionText != null)
                descriptionText.text = _data.Description;
            
            if (pointsText != null)
            {
                int currentPoints = 0;
                if (TouchFunctionListManager.Instance != null)
                {
                    currentPoints = TouchFunctionListManager.Instance.TouchPoints;
                }
                pointsText.text = $"현재 포인트: {currentPoints} pts";
            }
            
            if (levelText != null)
            {
                if (_isActive)
                {
                    levelText.text = $"Lv.{_data.Level}/{_data.MaxLevel}";
                }
                else
                {
                    levelText.text = "Lv.1";
                }
            }
            
            if (costText != null)
            {
                if (_isActive)
                {
                    if (_data.CanLevelUp())
                    {
                        int nextCost = _data.GetCurrentCost();
                        costText.text = $"강화: {nextCost} pts";
                    }
                    else
                    {
                        costText.text = "MAX";
                    }
                }
                else
                {
                    costText.text = $"구매: {_data.BaseCost} pts";
                }
            }
            
            if (addButton != null)
            {
                if (_isActive && !_data.CanLevelUp())
                {
                    addButton.gameObject.SetActive(false);
                }
                else
                {
                    addButton.gameObject.SetActive(true);
                    var buttonText = addButton.GetComponentInChildren<UnityEngine.UI.Text>();
                    if (buttonText != null)
                    {
                        buttonText.text = _isActive ? "+" : "Buy";
                    }
                }
            }
        }
        
        public void Refresh()
        {
            UpdateUI();
        }
    }
}