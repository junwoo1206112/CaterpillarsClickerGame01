using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Gameplay;
using ClickerGame.Core;

namespace ClickerGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text touchCountText;
        [SerializeField] private Text stageText;
        [SerializeField] private Text multiplierText;
        [SerializeField] private Text clickScoreText;

        private TouchCounter _touchCounter;
        private CharacterEvolution _evolution;
        private TouchFunctionManager _touchFunctionManager;

        private void Awake()
        {
            FindReferences();
            SetupEvents();
            UpdateUI();
        }

        private void FindReferences()
        {
            _touchCounter = FindFirstObjectByType<TouchCounter>();
            _evolution = FindFirstObjectByType<CharacterEvolution>();
            _touchFunctionManager = FindFirstObjectByType<TouchFunctionManager>();
        }

        private void SetupEvents()
        {
            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.AddListener(UpdateTouchCount);
            }
            else
            {
                Debug.LogError("[GameUI] TouchCounter not found! Cannot connect event!");
            }

            if (_evolution != null)
            {
                // 이름 이벤트만 연결 (숫자 이벤트는 사용하지 않음)
                _evolution.OnEvolutionNameChanged.AddListener(UpdateFormName);
            }
            else
            {
                Debug.LogWarning("[GameUI] Evolution not found!");
            }

            if (_touchFunctionManager != null)
            {
                _touchFunctionManager.OnFunctionActivated.AddListener(OnTouchFunctionActivated);
            }
        }

        private void Update()
        {
            UpdateBonusStatus();
        }

        private void UpdateUI()
        {
            if (_touchCounter != null)
            {
                UpdateTouchCount(_touchCounter.TotalTouchCount);
            }
        }

        public void UpdateBonusStatus()
        {
            if (multiplierText != null && _touchCounter != null)
            {
                int touchCount = _touchCounter.TotalTouchCount;
                bool isBonusEnabled = _touchCounter.IsBonusEnabled;

                if (isBonusEnabled)
                {
                    multiplierText.text = "Bonus: ON!";
                    multiplierText.color = Color.green;
                }
                else
                {
                    int remaining = 50 - touchCount;
                    if (remaining > 0)
                    {
                        multiplierText.text = $"Bonus: {remaining} left";
                        multiplierText.color = Color.white;
                    }
                    else
                    {
                        multiplierText.text = "Bonus: Ready!";
                        multiplierText.color = Color.yellow;
                    }
                }
            }
        }

        private int _lastTouchCount = 0;

        public void UpdateTouchCount(int count)
        {
            if (touchCountText == null)
            {
                Debug.LogError("[GameUI] touchCountText is NULL! Cannot update UI!");
                return;
            }
            
            touchCountText.text = $"Touches: {count}";
            
            // 증가분 표시
            int gain = count - _lastTouchCount;
            if (gain > 0 && clickScoreText != null)
            {
                clickScoreText.text = $"+{gain}";
                clickScoreText.color = Color.green;
            }
            
            _lastTouchCount = count;
        }

        public void UpdateFormName(string formName)
        {
            Debug.Log($"[GameUI] UpdateFormName called: {formName}");
            
            if (stageText != null)
            {
                stageText.text = $"Form: {formName}";
                Debug.Log($"[GameUI] UI updated: Form: {formName}");
            }
            else
            {
                Debug.LogError("[GameUI] stageText is NULL!");
            }
        }

        private void OnTouchFunctionActivated(string functionName)
        {
            // 터치 함수 발동 시 클릭 점수 표시
            if (clickScoreText != null)
            {
                clickScoreText.text = functionName;
                clickScoreText.color = Color.yellow;
            }
        }

        private void OnDestroy()
        {
            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.RemoveListener(UpdateTouchCount);
            }

            if (_evolution != null)
            {
                _evolution.OnEvolutionNameChanged.RemoveListener(UpdateFormName);
            }

            if (_touchFunctionManager != null)
            {
                _touchFunctionManager.OnFunctionActivated.RemoveListener(OnTouchFunctionActivated);
            }
        }
    }
}
