using UnityEngine;
using UnityEditor;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.EditorTools
{
    public class CreateEvolutionStageList
    {
        [MenuItem("Tools/Data/Create EvolutionStageList Manually")]
        public static void CreateEvolutionStageListAsset()
        {
            string path = "Assets/Resources/EvolutionStageList.asset";
            
            var asset = ScriptableObject.CreateInstance<EvolutionStageListSO>();
            
            asset.Stages = new List<EvolutionStageDataModel>
            {
                new EvolutionStageDataModel
                {
                    ID = "1",
                    Name = "애벌레",
                    TouchRequired = 0,
                    SpritePath = ""
                },
                new EvolutionStageDataModel
                {
                    ID = "2",
                    Name = "번데기",
                    TouchRequired = 1000,
                    SpritePath = ""
                },
                new EvolutionStageDataModel
                {
                    ID = "3",
                    Name = "나비",
                    TouchRequired = 3000,
                    SpritePath = ""
                }
            };
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"[CreateEvolutionStageList] Created {path}");
        }
    }
}