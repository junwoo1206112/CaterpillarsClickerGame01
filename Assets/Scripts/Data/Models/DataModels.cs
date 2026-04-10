using System;
using UnityEngine;

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
        public string Name;
        public string Description;
        public int BaseCost;
        public int Cost;
        public int Level;
        public int MaxLevel;
        public string Effect;
        public bool IsActive;
        public int TriggerCount;
        public string EffectType;
        public float BaseValue;
        public float Multiplier;
        public float Duration;
        public float Cooldown;
        public float CostMultiplier = 1.5f;
        
        public int GetCurrentCost()
        {
            return (int)(BaseCost * Mathf.Pow(CostMultiplier, Level - 1));
        }
        
        public float GetCurrentValue()
        {
            return BaseValue * Level;
        }
        
        public bool CanLevelUp()
        {
            return Level < MaxLevel;
        }
        
        public TouchFunctionData Clone()
        {
            return new TouchFunctionData
            {
                ID = this.ID,
                Name = this.Name,
                Description = this.Description,
                BaseCost = this.BaseCost,
                Cost = this.Cost,
                Level = this.Level,
                MaxLevel = this.MaxLevel,
                Effect = this.Effect,
                IsActive = this.IsActive,
                TriggerCount = this.TriggerCount,
                EffectType = this.EffectType,
                BaseValue = this.BaseValue,
                Multiplier = this.Multiplier,
                Duration = this.Duration,
                Cooldown = this.Cooldown,
                CostMultiplier = this.CostMultiplier
            };
        }
    }

    [Serializable]
    public class ConfigData
    {
        public string Key;
        public string Value;
        public string Description;
    }
}
