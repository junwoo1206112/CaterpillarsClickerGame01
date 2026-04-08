using UnityEngine;
using UnityEditor;
using ClickerGame.Data.Models;

namespace ClickerGame.EditorTools
{
    public class FixScriptableObjectReferences
    {
        [MenuItem("Tools/Game/Fix EvolutionStageList Reference")]
        public static void FixEvolutionStageList()
        {
            string path = "Assets/Resources/EvolutionStageList.asset";
            var asset = AssetDatabase.LoadAssetAtPath<EvolutionStageListSO>(path);
            
            if (asset != null)
            {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[FixScriptableObject] EvolutionStageList reference fixed!");
            }
            else
            {
                Debug.LogError("[FixScriptableObject] EvolutionStageList not found!");
            }
        }
        
        [MenuItem("Tools/Game/Fix TouchFunctionList Reference")]
        public static void FixTouchFunctionList()
        {
            string path = "Assets/Resources/TouchFunctionList.asset";
            var asset = AssetDatabase.LoadAssetAtPath<TouchFunctionListSO>(path);
            
            if (asset != null)
            {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[FixScriptableObject] TouchFunctionList reference fixed!");
            }
            else
            {
                Debug.LogError("[FixScriptableObject] TouchFunctionList not found!");
            }
        }
    }
}