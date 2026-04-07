using UnityEngine;
using UnityEngine.Events;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.Gameplay
{
    public class ItemManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<ItemDataModel> _itemData;

        [Header("Events")]
        public UnityEvent<string> OnItemUsed;
        public UnityEvent<string, int> OnItemCountChanged;

        private Dictionary<string, int> _itemCounts = new Dictionary<string, int>();

        private void Awake()
        {
            if (OnItemUsed == null)
                OnItemUsed = new UnityEvent<string>();

            if (OnItemCountChanged == null)
                OnItemCountChanged = new UnityEvent<string, int>();
        }

        public void Initialize(List<ItemDataModel> itemData)
        {
            _itemData = itemData;

            if (_itemData != null)
            {
                foreach (var item in _itemData)
                {
                    _itemCounts[item.ID] = 10;
                }
            }
        }

        public bool UseItem(string itemId)
        {
            if (!_itemCounts.ContainsKey(itemId) || _itemCounts[itemId] <= 0)
            {
                Debug.Log($"[ItemManager] Cannot use item {itemId}. Count: {_itemCounts.GetValueOrDefault(itemId, 0)}");
                return false;
            }

            _itemCounts[itemId]--;
            OnItemCountChanged?.Invoke(itemId, _itemCounts[itemId]);

            var item = FindItem(itemId);
            if (item != null)
            {
                ApplyItemEffect(item);
                OnItemUsed?.Invoke(item.Name);
                Debug.Log($"[ItemManager] Used item: {item.Name}");
                return true;
            }

            return false;
        }

        private void ApplyItemEffect(ItemDataModel item)
        {
            switch (item.Effect)
            {
                case "AddScore":
                    var scoreManager = FindFirstObjectByType<ScoreManager>();
                    if (scoreManager != null)
                    {
                        scoreManager.AddScore(item.Value);
                    }
                    break;

                case "SpeedBoost":
                    var touchFunctionManager = FindFirstObjectByType<TouchFunctionManager>();
                    if (touchFunctionManager != null)
                    {
                        touchFunctionManager.ActivateSpeedBoost();
                    }
                    break;

                default:
                    Debug.Log($"[ItemManager] Unknown effect: {item.Effect}");
                    break;
            }
        }

        public int GetItemCount(string itemId)
        {
            return _itemCounts.GetValueOrDefault(itemId, 0);
        }

        public void AddItem(string itemId, int amount)
        {
            if (!_itemCounts.ContainsKey(itemId))
            {
                _itemCounts[itemId] = 0;
            }

            _itemCounts[itemId] += amount;
            OnItemCountChanged?.Invoke(itemId, _itemCounts[itemId]);
        }

        public ItemDataModel FindItem(string itemId)
        {
            if (_itemData == null)
                return null;

            foreach (var item in _itemData)
            {
                if (item.ID == itemId)
                    return item;
            }

            return null;
        }

        public void ResetItems()
        {
            _itemCounts.Clear();
            Initialize(_itemData);
        }
    }
}
