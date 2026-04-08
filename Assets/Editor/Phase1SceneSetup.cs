using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using ClickerGame.Core;
using ClickerGame.Tests;
using ClickerGame.Data.Models;

namespace ClickerGame.EditorTools
{
    public class Phase1SceneSetup
    {
        [MenuItem("Tools/Game/Setup Phase 1 Test Scene")]
        public static void SetupPhase1TestScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            
            GameObject dataManagerObj = new GameObject("DataManager");
            DataInitializer dataInitializer = dataManagerObj.AddComponent<DataInitializer>();
            
            var evolutionListSO = Resources.Load<EvolutionStageListSO>("EvolutionStageList");
            if (evolutionListSO != null)
            {
                var so = new SerializedObject(dataInitializer);
                so.FindProperty("_characterListSO").objectReferenceValue = evolutionListSO;
                so.ApplyModifiedProperties();
            }
            
            GameObject testObj = new GameObject("Phase1TestObject");
            testObj.AddComponent<Phase1Test>();
            
            string scenePath = "Assets/Scenes/Phase1TestScene.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            
            Debug.Log($"[Phase1SceneSetup] Created Phase 1 test scene at {scenePath}");
            Debug.Log($"[Phase1SceneSetup] Run the scene to test DI Container");
            
            EditorUtility.DisplayDialog("Phase 1 Test Scene", 
                "Phase 1 Test Scene created!\n\n" +
                "Scene: Assets/Scenes/Phase1TestScene.unity\n\n" +
                "Click Play to run the test.", 
                "OK");
        }
    }
}