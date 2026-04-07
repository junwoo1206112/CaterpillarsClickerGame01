using UnityEngine;
using System;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class CriticalTouchFunction : ITouchFunction
    {
        public string FunctionName => "Critical Click";
        public int TriggerCount => 0;
        public float Multiplier => 5f;
        public float Duration => 0f;
        public float Cooldown => 0f;
        public bool IsReusable => true;

        [Range(0f, 1f)]
        [SerializeField] private float _criticalChance = 0.1f;

        public float CriticalChance => _criticalChance;

        public bool CanActivate(int currentCount, float lastActivateTime)
        {
            return UnityEngine.Random.value < _criticalChance;
        }

        public TouchEffect Activate()
        {
            Debug.Log("[CriticalClick] Critical hit! 5x damage!");
            return new TouchEffect(0, Multiplier, Duration, FunctionName);
        }

        public void Reset()
        {
            // No state to reset for critical
        }

        public void Update(float deltaTime)
        {
            // No duration-based effect
        }

        public ITouchFunction Clone()
        {
            return new CriticalTouchFunction();
        }

        public void SetCriticalChance(float chance)
        {
            _criticalChance = Mathf.Clamp01(chance);
        }
    }
}
