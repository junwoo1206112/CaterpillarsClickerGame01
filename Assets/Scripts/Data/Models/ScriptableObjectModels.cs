using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClickerGame.Data.Models
{
    [Serializable]
    public class EvolutionStageDataModel
    {
        public string ID;
        public string Name;
        public int TouchRequired;
        public string SpritePath;
    }

    [Serializable]
    public class EvolutionStageListSO : ScriptableObject
    {
        public List<EvolutionStageDataModel> Stages = new();
    }

    [Serializable]
    public class BackgroundDataModel
    {
        public string ID;
        public string Name;
        public string SpritePath;
        public int UnlockScore;
    }

    [Serializable]
    public class BackgroundListSO : ScriptableObject
    {
        public List<BackgroundDataModel> Backgrounds = new();
    }

    [Serializable]
    public class ItemDataModel
    {
        public string ID;
        public string Name;
        public string Effect;
        public int Value;
        public float Duration;
        public string IconPath;
    }

    [Serializable]
    public class ItemListSO : ScriptableObject
    {
        public List<ItemDataModel> Items = new();
    }

    [Serializable]
    public class ConfigDataModel
    {
        public string Key;
        public string Value;
        public string Description;
    }

    [Serializable]
    public class ConfigListSO : ScriptableObject
    {
        public List<ConfigDataModel> Configs = new();
    }

    [Serializable]
    public class TouchFunctionDataModel
    {
        public string ID;
        public string FunctionName;
        public string FunctionType;
        public int TriggerCount;
        public float Multiplier;
        public float Duration;
        public float Cooldown;
        public float CriticalChance;
        public bool IsActive;
        public bool IsReusable;
    }

    [Serializable]
    public class TouchFunctionListSO : ScriptableObject
    {
        public List<TouchFunctionDataModel> Functions = new();
    }
}
