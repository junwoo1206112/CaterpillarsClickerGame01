using UnityEngine;
using UnityEditor;
using ClickerGame.Core;
using ClickerGame.Tests;

namespace ClickerGame.EditorTools
{
    public class Phase1TestSetup
    {
        [MenuItem("Tools/Game/Add Phase 1 Test to Current Scene")]
        public static void AddPhase1TestToCurrentScene()
        {
            var existingTest = Object.FindFirstObjectByType<Phase1Test>();
            
            if (existingTest != null)
            {
                Debug.Log("[Phase1TestSetup] Phase1Test already exists in scene");
                Selection.activeGameObject = existingTest.gameObject;
                return;
            }
            
            GameObject testObj = new GameObject("Phase1TestObject");
            testObj.AddComponent<Phase1Test>();
            
            var existingDataInitializer = Object.FindFirstObjectByType<DataInitializer>();
            if (existingDataInitializer == null)
            {
                GameObject dataManagerObj = new GameObject("DataManager");
                dataManagerObj.AddComponent<DataInitializer>();
                Debug.Log("[Phase1TestSetup] Created DataManager");
            }
            
            Selection.activeGameObject = testObj;
            
            Debug.Log("[Phase1TestSetup] Phase1Test added to current scene");
            Debug.Log("[Phase1TestSetup] Click Play to run the test");
            
            EditorUtility.DisplayDialog("Phase 1 Test", 
                "Phase1Test added to current scene!\n\n" +
                "Click Play to run the test.", 
                "OK");
        }
    }
}