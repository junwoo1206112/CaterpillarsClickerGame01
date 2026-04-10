using UnityEngine;
using UnityEditor;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class InjectSettingPanelIntoScene
    {
        [MenuItem("Tools/Game/INJECT SettingPanel into Scene File")]
        public static void InjectIntoSceneFile()
        {
            string scenePath = "Assets/Scenes/GamePlayScene.unity";
            string prefabPath = "Assets/Prefabs/SettingPanel.prefab";
            
            if (!File.Exists(scenePath))
            {
                Debug.LogError("[Inject] Scene file not found!");
                return;
            }
            
            if (!File.Exists(prefabPath))
            {
                Debug.LogError("[Inject] Prefab file not found!");
                return;
            }
            
            string sceneContent = File.ReadAllText(scenePath);
            
            // Check if already exists
            if (sceneContent.Contains("ClickerGame.UI.SettingManager"))
            {
                Debug.Log("[Inject] SettingManager already exists in scene!");
                return;
            }
            
            string prefabContent = File.ReadAllText(prefabPath);
            
            // Find insertion point (before SceneRoots)
            string marker = "--- !u!1660057539 &9223372036854775807";
            int insertPosition = sceneContent.LastIndexOf(marker);
            
            if (insertPosition > 0)
            {
                string newContent = sceneContent.Substring(0, insertPosition) + "\n" + prefabContent + "\n" + marker;
                newContent += sceneContent.Substring(insertPosition + marker.Length);
                
                File.WriteAllText(scenePath, newContent);
                AssetDatabase.Refresh();
                
                Debug.Log("[Inject] ================================================");
                Debug.Log("[Inject] SettingPanel injected into scene file!");
                Debug.Log("[Inject] NOW: Close and reopen the scene!");
                Debug.Log("[Inject] Then Play and click Settings button!");
                Debug.Log("[Inject] ================================================");
            }
            else
            {
                Debug.LogError("[Inject] Could not find insertion point!");
            }
        }
    }
}
