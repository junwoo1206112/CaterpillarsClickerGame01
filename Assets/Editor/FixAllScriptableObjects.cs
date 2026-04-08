using UnityEngine;
using UnityEditor;

namespace ClickerGame.EditorTools
{
    public class FixAllScriptableObjects
    {
        [MenuItem("Tools/Game/Fix All ScriptableObjects")]
        public static void FixAll()
        {
            // 1. Resources 폴더 정리
            string[] files = {
                "Assets/Resources/EvolutionStageList.asset",
                "Assets/Resources/EvolutionStageList.asset.meta",
                "Assets/Resources/TouchFunctionList.asset",
                "Assets/Resources/TouchFunctionList.asset.meta"
            };
            
            foreach (var file in files)
            {
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                    Debug.Log($"[FixAll] Deleted {file}");
                }
            }
            
            AssetDatabase.Refresh();
            
            // 2. 다시 생성
            CreateEvolutionStageList.CreateEvolutionStageListAsset();
            CreateTouchFunctionList.CreateTouchFunctionListAsset();
            
            Debug.Log("[FixAll] All ScriptableObjects fixed!");
            EditorUtility.DisplayDialog("완료", 
                "모든 ScriptableObject 가 복구되었습니다!\n\nPlay 하세요.", 
                "OK");
        }
    }
}