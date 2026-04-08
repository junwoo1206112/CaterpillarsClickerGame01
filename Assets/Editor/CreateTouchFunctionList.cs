using UnityEngine;
using UnityEditor;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.EditorTools
{
    public class CreateTouchFunctionList
    {
        [MenuItem("Tools/Data/Create TouchFunctionList Manually")]
        public static void CreateTouchFunctionListAsset()
        {
            string path = "Assets/Resources/TouchFunctionList.asset";
            
            var asset = ScriptableObject.CreateInstance<TouchFunctionListSO>();
            
            asset.Functions = new List<TouchFunctionDataModel>
            {
                new TouchFunctionDataModel
                {
                    ID = "critical",
                    FunctionName = "크리티컬",
                    FunctionType = "Critical",
                    TriggerCount = 50,
                    Multiplier = 2f,
                    Duration = 0f,
                    Cooldown = 0f,
                    CriticalChance = 0.1f,
                    IsActive = false,
                    IsReusable = true
                },
                new TouchFunctionDataModel
                {
                    ID = "doubleclick",
                    FunctionName = "더블클릭",
                    FunctionType = "DoubleClick",
                    TriggerCount = 100,
                    Multiplier = 2f,
                    Duration = 0f,
                    Cooldown = 0f,
                    CriticalChance = 0f,
                    IsActive = false,
                    IsReusable = true
                },
                new TouchFunctionDataModel
                {
                    ID = "speedboost",
                    FunctionName = "스피드 부스트",
                    FunctionType = "SpeedBoost",
                    TriggerCount = 200,
                    Multiplier = 1f,
                    Duration = 60f,
                    Cooldown = 0f,
                    CriticalChance = 0f,
                    IsActive = false,
                    IsReusable = true
                }
            };
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"[CreateTouchFunctionList] Created {path} with {asset.Functions.Count} functions");
        }
    }
}