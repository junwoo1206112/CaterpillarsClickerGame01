using UnityEngine;
using UnityEngine.Events;

namespace ClickerGame.Gameplay
{
    public class TouchCounter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int bonusTriggerCount = 50;
        [SerializeField] private int bonusInterval = 3;

        [Header("Events")]
        public UnityEvent<int> OnTouchCountChanged;
        public UnityEvent OnBonusActivated;

        private int _totalTouchCount = 0;
        private int _currentIntervalCount = 0;
        private bool _bonusEnabled = false;

        public int TotalTouchCount => _totalTouchCount;
        public bool IsBonusEnabled => _bonusEnabled;

        private void Awake()
        {
            if (OnTouchCountChanged == null)
                OnTouchCountChanged = new UnityEvent<int>();

            if (OnBonusActivated == null)
                OnBonusActivated = new UnityEvent();
        }

        public void AddTouch()
        {
            int pointsPerClick = GetPointsPerClick();
            
            _totalTouchCount += pointsPerClick;
            _currentIntervalCount++;

            OnTouchCountChanged?.Invoke(_totalTouchCount);

            CheckBonus();
        }
        
        public void SubtractPoints(int amount)
        {
            _totalTouchCount -= amount;
            if (_totalTouchCount < 0)
                _totalTouchCount = 0;
                
            OnTouchCountChanged?.Invoke(_totalTouchCount);
            Debug.Log($"[TouchCounter] Subtracted {amount}. Total: {_totalTouchCount}");
        }
        
        private int GetPointsPerClick()
        {
            if (ClickerGame.UI.TouchFunctionListManager.Instance != null)
            {
                return ClickerGame.UI.TouchFunctionListManager.Instance.PointsPerClick;
            }
            return 1;
        }

        private void CheckBonus()
        {
            if (_totalTouchCount >= bonusTriggerCount && !_bonusEnabled)
            {
                _bonusEnabled = true;
                Debug.Log($"Bonus Enabled! Total Touches: {_totalTouchCount}");
            }

            if (_bonusEnabled && _currentIntervalCount >= bonusInterval)
            {
                _currentIntervalCount = 0;
                OnBonusActivated?.Invoke();
                Debug.Log("Bonus Click Activated! (+1 extra click)");
            }
        }

        public bool ShouldActivateBonus()
        {
            return _bonusEnabled && _currentIntervalCount >= bonusInterval;
        }

        public void ResetCounter()
        {
            _totalTouchCount = 0;
            _currentIntervalCount = 0;
            _bonusEnabled = false;
            OnTouchCountChanged?.Invoke(_totalTouchCount);
        }

        public void SetBonusEnabled(bool enabled)
        {
            _bonusEnabled = enabled;
        }
    }
}