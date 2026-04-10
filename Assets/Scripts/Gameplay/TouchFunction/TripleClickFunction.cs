using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public class TripleClickFunction : ITouchFunction
    {
        public string FunctionName => "Triple Click";
        public int TriggerCount => 0;
        public float Multiplier => 3f;
        public float Duration => 0f;
        public float Cooldown => 0f;
        public bool IsReusable => true;

        public bool CanActivate(int currentCount, float lastActivateTime)
        {
            return true;
        }

        public TouchEffect Activate()
        {
            return new TouchEffect(0, Multiplier, Duration, FunctionName);
        }

        public void Reset()
        {
        }

        public ITouchFunction Clone()
        {
            return new TripleClickFunction();
        }
    }
}