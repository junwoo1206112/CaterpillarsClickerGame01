using UnityEngine;

namespace ClickerGame.Gameplay.TouchFunction
{
    public enum TouchFunctionType
    {
        None,
        BonusClick,
        SpeedBoost,
        CriticalClick,
        AutoClick,
        Custom
    }

    [System.Serializable]
    public class TouchFunctionConfig
    {
        public string FunctionName;
        public TouchFunctionType FunctionType;
        public int TriggerCount;
        public float Multiplier;
        public float Duration;
        public float Cooldown;
        public bool IsActive;
        public bool AutoActivate;
    }
}
