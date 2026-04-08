using UnityEngine;
using UnityEditor;
using ClickerGame.Core;
using ClickerGame.Data.Models;

namespace ClickerGame.EditorTools
{
    public class SetupDataInitializer
    {
        [MenuItem("Tools/Game/Setup DataInitializer")]
        public static void SetupDataInitializerInScene()
        {
            var existing = Object.FindFirstObjectByType<DataInitializer>();
            
            if (existing != null)
            {
                Debug.Log("[SetupDataInitializer] DataInitializer already exists");
                Selection.activeGameObject = existing.gameObject;
                return;
            }
            
            GameObject obj = new GameObject("DataInitializer");
            DataInitializer dataInitializer = obj.AddComponent<DataInitializer>();
            
            var evolutionList = Resources.Load<EvolutionStageListSO>("EvolutionStageList");
            var backgroundList = Resources.Load<BackgroundListSO>("BackgroundList");
            
            var so = new SerializedObject(dataInitializer);
            
            if (evolutionList != null)
            {
                so.FindProperty("_characterListSO").objectReferenceValue = evolutionList;
                Debug.Log("[SetupDataInitializer] Connected EvolutionStageList");
            }
            else
            {
                Debug.LogWarning("[SetupDataInitializer] EvolutionStageList not found in Resources!");
            }
            
            if (backgroundList != null)
            {
                so.FindProperty("_backgroundListSO").objectReferenceValue = backgroundList;
                Debug.Log("[SetupDataInitializer] Connected BackgroundList");
            }
            
            so.ApplyModifiedProperties();
            
            Debug.Log("[SetupDataInitializer] DataInitializer setup complete!");
        }
    }
}