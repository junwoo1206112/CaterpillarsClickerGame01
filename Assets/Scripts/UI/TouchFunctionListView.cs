using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ClickerGame.Data.Models;

namespace ClickerGame.UI
{
    public class TouchFunctionListView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private TouchFunctionListItem itemPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button closeButton;
        [SerializeField] private Text pointsText;
        
        private List<TouchFunctionListItem> _items = new();
        
        private void Awake()
        {
            SetupButtons();
            RefreshList();
        }
        
        private void Update()
        {
            UpdatePointsDisplay();
        }
        
        private void UpdatePointsDisplay()
        {
            if (pointsText != null && TouchFunctionListManager.Instance != null)
            {
                int perClick = TouchFunctionListManager.Instance.PointsPerClick;
                pointsText.text = $"+{perClick}/클릭";
            }
        }
        
        private void SetupButtons()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseWindow);
            
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnFunctionAdded += (id) => RefreshList();
                TouchFunctionListManager.Instance.OnFunctionRemoved += (id) => RefreshList();
            }
        }
        
        public void RefreshList()
        {
            ClearList();
            
            if (TouchFunctionListManager.Instance == null)
            {
                Debug.LogError("[TouchFunctionListView] TouchFunctionListManager not found!");
                return;
            }
            
            foreach (var function in TouchFunctionListManager.Instance.allFunctions)
            {
                CreateItem(function);
            }
        }
        
        private void ClearList()
        {
            foreach (var item in _items)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            _items.Clear();
        }
        
        private void CreateItem(TouchFunctionData data)
        {
            if (itemPrefab == null || contentParent == null)
            {
                Debug.LogError("[TouchFunctionListView] Prefab or Content is null!");
                return;
            }
            
            var newItem = Instantiate(itemPrefab, contentParent);
            var isActive = TouchFunctionListManager.Instance.IsFunctionActive(data.ID);
            newItem.Initialize(data);
            newItem.SetActiveState(isActive);
            
            _items.Add(newItem);
        }
        
        private void CloseWindow()
        {
            var uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.CloseAllWindows();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveListener(CloseWindow);
            
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnFunctionAdded -= (id) => RefreshList();
                TouchFunctionListManager.Instance.OnFunctionRemoved -= (id) => RefreshList();
            }
        }
    }
}