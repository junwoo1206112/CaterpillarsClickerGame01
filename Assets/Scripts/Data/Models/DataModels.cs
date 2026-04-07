using System;

namespace ClickerGame.Data.Models
{
    [Serializable]
    public class EvolutionStageData
    {
        public string ID;
        public string Name;
        public int TouchRequired;
        public string SpritePath;
    }

    [Serializable]
    public class StageData
    {
        public string ID;
        public int UnlockScore;
        public string BackgroundPath;
        public float Difficulty;
    }

    [Serializable]
    public class ItemData
    {
        public string ID;
        public string Name;
        public string Effect;
        public int Value;
        public float Duration;
        public string IconPath;
    }

    [Serializable]
    public class TouchFunctionData
    {
        public string ID;
        public int TriggerCount;
        public string EffectType;
        public float Multiplier;
        public float Duration;
        public float Cooldown;
    }

    [Serializable]
    public class ConfigData
    {
        public string Key;
        public string Value;
        public string Description;
    }
}
