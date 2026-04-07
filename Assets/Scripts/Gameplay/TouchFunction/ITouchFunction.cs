using UnityEngine;

namespace ClickerGame.Gameplay
{
    public struct TouchEffect
    {
        public int BonusClicks;
        public float ScoreMultiplier;
        public float Duration;
        public string EffectName;

        public TouchEffect(int bonusClicks, float multiplier, float duration, string name)
        {
            BonusClicks = bonusClicks;
            ScoreMultiplier = multiplier;
            Duration = duration;
            EffectName = name;
        }
    }

    public interface ITouchFunction
    {
        string FunctionName { get; }
        int TriggerCount { get; }
        float Multiplier { get; }
        float Duration { get; }
        float Cooldown { get; }
        bool IsReusable { get; }

        bool CanActivate(int currentCount, float lastActivateTime);
        TouchEffect Activate();
        void Reset();
        ITouchFunction Clone();
    }
}
