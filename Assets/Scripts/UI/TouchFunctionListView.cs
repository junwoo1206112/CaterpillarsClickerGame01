using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using ClickerGame.Data.Models;
using ClickerGame.Gameplay;

namespace ClickerGame.UI
{
    public class TouchFunctionListView : MonoBehaviour, IScrollHandler
    {
        [Header("UI References")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private TouchFunctionListItem itemPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Text pointsText;
        
        private List<TouchFunctionListItem> _items = new();
        private TouchCounter _touchCounter;
        
        public void OnScroll(PointerEventData eventData)
        {
            if (scrollRect != null)
            {
                scrollRect.OnScroll(eventData);
            }
        }
        
        private void Awake()
        {
            _touchCounter = FindFirstObjectByType<TouchCounter>();
            SetupEvents();
            RefreshList();
        }
        
        private void SetupEvents()
        {
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnFunctionAdded += (id) => RefreshList();
                TouchFunctionListManager.Instance.OnFunctionRemoved += (id) => RefreshList();
            }
            
            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.AddListener(OnTouchCountChanged);
            }
        }
        
        private void OnTouchCountChanged(int count)
        {
            UpdatePointsDisplay();
        }
        
        private void Update()
        {
            UpdatePointsDisplay();
        }
        
        private void UpdatePointsDisplay()
        {
            if (pointsText != null && TouchFunctionListManager.Instance != null)
            {
                int points = TouchFunctionListManager.Instance.TouchPoints;
                int perClick = TouchFunctionListManager.Instance.PointsPerClick;
                pointsText.text = $"Points: {points} (+{perClick}/click)";
            }
        }
        
        private void OnDestroy()
        {
            if (TouchFunctionListManager.Instance != null)
            {
                TouchFunctionListManager.Instance.OnFunctionAdded -= (id) => RefreshList();
                TouchFunctionListManager.Instance.OnFunctionRemoved -= (id) => RefreshList();
            }
            
            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.RemoveListener(OnTouchCountChanged);
            }
        }
        
        public void RefreshList()
        {
            if (TouchFunctionListManager.Instance == null)
            {
                Debug.LogError("[TouchFunctionListView] TouchFunctionListManager not found!");
                return;
            }
            
            if (_items.Count == 0)
            {
                foreach (var function in TouchFunctionListManager.Instance.allFunctions)
                {
                    CreateItem(function);
                }
            }
            else
            {
                foreach (var item in _items)
                {
                    if (item != null)
                    {
                        var data = TouchFunctionListManager.Instance.allFunctions.Find(f => f.ID == item.DataID);
                        if (data != null)
                        {
                            var activeFunc = TouchFunctionListManager.Instance.GetActiveFunction(data.ID);
                            if (activeFunc != null)
                            {
                                item.Initialize(activeFunc);
                                item.SetActiveState(true);
                            }
                            else
                            {
                                item.Initialize(data);
                                item.SetActiveState(false);
                            }
                        }
                    }
                }
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
            var activeFunc = TouchFunctionListManager.Instance.GetActiveFunction(data.ID);
            
            if (activeFunc != null)
            {
                newItem.Initialize(activeFunc);
                newItem.SetActiveState(true);
            }
            else
            {
                newItem.Initialize(data);
                newItem.SetActiveState(false);
            }
            
            _items.Add(newItem);
        }
    }
}
