using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class SpeedBoostFunction : ITouchFunction
    {
        public string FunctionName => "Speed Boost";
        public int TriggerCount => 0;
        public float Multiplier => 2f;
        public float Duration => 20f;
        public float Cooldown => 30f;
        public bool IsReusable => true;

        private float _lastActivateTime = -999f;
        private bool _isActive = false;

        public bool IsActive => _isActive;
        public float RemainingTime { get; private set; }

        public bool CanActivate(int currentCount, float currentTime)
        {
            return currentTime - _lastActivateTime >= Cooldown;
        }

        public TouchEffect Activate()
        {
            _lastActivateTime = Time.time;
            _isActive = true;
            RemainingTime = Duration;

            Debug.Log($"[SpeedBoost] Activated for {Duration}s!");

            return new TouchEffect(0, Multiplier, Duration, FunctionName);
        }

        public void Update(float deltaTime)
        {
            if (_isActive)
            {
                RemainingTime -= deltaTime;

                if (RemainingTime <= 0)
                {
                    _isActive = false;
                    Debug.Log("[SpeedBoost] Deactivated");
                }
            }
        }

        public void Reset()
        {
            _isActive = false;
            _lastActivateTime = -999f;
            RemainingTime = 0f;
        }

        public float GetCooldownRemaining(float currentTime)
        {
            float elapsed = currentTime - _lastActivateTime;
            return Mathf.Max(0, Cooldown - elapsed);
        }

        public ITouchFunction Clone()
        {
            return new SpeedBoostFunction();
        }
    }
}
