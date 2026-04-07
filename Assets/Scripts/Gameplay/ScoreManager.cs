using UnityEngine;
using UnityEngine.Events;

namespace ClickerGame.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int startingScore = 0;

        [Header("Events")]
        public UnityEvent<int> OnScoreChanged;
        public UnityEvent<int, int> OnScoreReached;

        private int _currentScore;
        private float _scoreMultiplier = 1f;

        public int CurrentScore => _currentScore;
        public float ScoreMultiplier => _scoreMultiplier;

        private void Awake()
        {
            _currentScore = startingScore;

            if (OnScoreChanged == null)
                OnScoreChanged = new UnityEvent<int>();

            if (OnScoreReached == null)
                OnScoreReached = new UnityEvent<int, int>();
        }

        public void AddScore(int amount)
        {
            int actualAmount = Mathf.RoundToInt(amount * _scoreMultiplier);
            _currentScore += actualAmount;

            OnScoreChanged?.Invoke(_currentScore);

            Debug.Log($"Score: {_currentScore} (+{actualAmount})");
        }

        public void SetScoreMultiplier(float multiplier)
        {
            _scoreMultiplier = multiplier;
            Debug.Log($"Score Multiplier: {_scoreMultiplier}x");
        }

        public void ResetMultiplier()
        {
            _scoreMultiplier = 1f;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public bool CheckScoreReached(int targetScore)
        {
            if (_currentScore >= targetScore)
            {
                OnScoreReached?.Invoke(_currentScore, targetScore);
                return true;
            }
            return false;
        }

        public void ResetScore()
        {
            _currentScore = startingScore;
            _scoreMultiplier = 1f;
            OnScoreChanged?.Invoke(_currentScore);
        }
    }
}
