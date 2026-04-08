using UnityEngine;
using UnityEditor;
using ClickerGame.Data.Models;
using System.Collections.Generic;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class ForceRecreateScriptableObjects
    {
        [MenuItem("Tools/Game/🔧 Force Recreate All ScriptableObjects")]
        public static void ForceRecreate()
        {
            // 1. Delete old files
            DeleteFile("Assets/Resources/EvolutionStageList.asset");
            DeleteFile("Assets/Resources/TouchFunctionList.asset");
            
            AssetDatabase.Refresh();
            
            // 2. Create new EvolutionStageList
            var evolutionList = ScriptableObject.CreateInstance<EvolutionStageListSO>();
            evolutionList.Stages = new List<EvolutionStageDataModel>
            {
                new EvolutionStageDataModel { ID = "1", Name = "애벌레", TouchRequired = 0, SpritePath = "" },
                new EvolutionStageDataModel { ID = "2", Name = "번데기", TouchRequired = 1000, SpritePath = "" },
                new EvolutionStageDataModel { ID = "3", Name = "나비", TouchRequired = 3000, SpritePath = "" }
            };
            AssetDatabase.CreateAsset(evolutionList, "Assets/Resources/EvolutionStageList.asset");
            Debug.Log("[ForceRecreate] Created EvolutionStageList.asset");
            
            // 3. Create new TouchFunctionList
            var touchFunctionList = ScriptableObject.CreateInstance<TouchFunctionListSO>();
            touchFunctionList.Functions = new List<TouchFunctionDataModel>
            {
                new TouchFunctionDataModel { ID = "critical", FunctionName = "크리티컬", FunctionType = "Critical", TriggerCount = 50, Multiplier = 2f },
                new TouchFunctionDataModel { ID = "doubleclick", FunctionName = "더블클릭", FunctionType = "DoubleClick", TriggerCount = 100, Multiplier = 2f },
                new TouchFunctionDataModel { ID = "speedboost", FunctionName = "스피드부스트", FunctionType = "SpeedBoost", TriggerCount = 200, Duration = 60f }
            };
            AssetDatabase.CreateAsset(touchFunctionList, "Assets/Resources/TouchFunctionList.asset");
            Debug.Log("[ForceRecreate] Created TouchFunctionList.asset");
            
            // 4. Save
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[ForceRecreate] ✅ All ScriptableObjects recreated successfully!");
            EditorUtility.DisplayDialog("완료", 
                "ScriptableObject 가 복구되었습니다!\n\n" +
                "1. Play 버튼을 누르세요\n" +
                "2. Console 에서 [TouchFunctionList] Loaded 3 functions 확인\n" +
                "3. 오른쪽 패널에서 리스트 확인", 
                "OK");
        }
        
        private static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[ForceRecreate] Deleted {path}");
            }
            if (File.Exists(path + ".meta"))
            {
                File.Delete(path + ".meta");
                Debug.Log($"[ForceRecreate] Deleted {path}.meta");
            }
        }
    }
}
