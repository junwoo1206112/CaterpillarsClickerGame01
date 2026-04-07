using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class BonusTouchFunction : ITouchFunction
    {
        public string FunctionName => "Bonus Click";
        public int TriggerCount => 50;
        public float Multiplier => 1.33f;
        public float Duration => 0f;
        public float Cooldown => 0f;
        public bool IsReusable => true;

        private int _intervalCount = 0;
        private const int INTERVAL = 3;

        public bool CanActivate(int currentCount, float lastActivateTime)
        {
            return currentCount >= TriggerCount;
        }

        public TouchEffect Activate()
        {
            _intervalCount++;

            if (_intervalCount >= INTERVAL)
            {
                _intervalCount = 0;
                return new TouchEffect(1, 1f, 0f, "Bonus Click");
            }

            return new TouchEffect(0, 1f, 0f, "");
        }

        public void Reset()
        {
            _intervalCount = 0;
        }

        public ITouchFunction Clone()
        {
            return new BonusTouchFunction();
        }
    }
}
