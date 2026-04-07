using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ClickerGame.Data.Models;

namespace ClickerGame.UI
{
    public class TouchFunctionListManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private TouchFunctionListItem listItemPrefab;

        [Header("Data")]
        [SerializeField] private List<TouchFunctionDataModel> allFunctions = new List<TouchFunctionDataModel>();
        [SerializeField] private List<string> activeFunctions = new List<string>();

        private List<TouchFunctionListItem> _items = new List<TouchFunctionListItem>();

        private void Awake()
        {
            LoadFromExcel();
        }

        private void Start()
        {
            RefreshList();
        }

        public void LoadFromExcel()
        {
            // Excel 데이터 로드 (나중에 DataManager 와 연동)
            // 현재는 더미 데이터 사용
            if (allFunctions.Count == 0)
            {
                allFunctions.Add(new TouchFunctionDataModel
                {
                    Name = "크리티컬",
                    Description = "확률로 2 배 데미지",
                    Level = 1
                });

                allFunctions.Add(new TouchFunctionDataModel
                {
                    Name = "스피드 부스트",
                    Description = "일정 시간 동안 연타 속도 증가",
                    Level = 1
                });

                allFunctions.Add(new TouchFunctionDataModel
                {
                    Name = "보너스 터치",
                    Description = "50 회마다 추가 터치",
                    Level = 1
                });
            }
        }

        public void RefreshList()
        {
            ClearList();

            foreach (var func in allFunctions)
            {
                CreateListItem(func);
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

        private void CreateListItem(TouchFunctionDataModel data)
        {
            if (listItemPrefab == null || contentParent == null)
            {
                Debug.LogError("[TouchFunctionListManager] Prefab or Content is null!");
                return;
            }

            TouchFunctionListItem newItem = Instantiate(listItemPrefab, contentParent);
            
            bool isActive = activeFunctions.Contains(data.Name);
            newItem.Initialize(data.Name, data.Description, data.Level);
            newItem.SetInteractable(true);

            newItem.OnAddClicked += AddFunction;
            newItem.OnRemoveClicked += RemoveFunction;

            _items.Add(newItem);
        }

        public void AddFunction(string functionName)
        {
            if (!activeFunctions.Contains(functionName))
            {
                activeFunctions.Add(functionName);
                Debug.Log($"[TouchFunction] {functionName} 활성화!");
                
                // 실제 게임에 적용 (TouchFunctionManager 와 연동)
                // TouchFunctionManager.Instance.ActivateFunction(functionName);
                
                RefreshList();
            }
        }

        public void RemoveFunction(string functionName)
        {
            if (activeFunctions.Contains(functionName))
            {
                activeFunctions.Remove(functionName);
                Debug.Log($"[TouchFunction] {functionName} 비활성화!");
                
                // 실제 게임에서 제거
                // TouchFunctionManager.Instance.DeactivateFunction(functionName);
                
                RefreshList();
            }
        }

        public void SaveToExcel()
        {
            // Excel 데이터 저장 (나중에 구현)
            Debug.Log("[TouchFunctionListManager] 데이터 저장됨");
        }
    }

    // Excel 데이터 모델 (임시)
    [System.Serializable]
    public class TouchFunctionDataModel
    {
        public string Name;
        public string Description;
        public int Level;
    }
}
