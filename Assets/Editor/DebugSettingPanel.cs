using UnityEngine;
using UnityEditor;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class DebugSettingPanel
    {
        [MenuItem("Tools/Game/DEBUG Check SettingPanel")]
        public static void DebugCheck()
        {
            Debug.Log("=== SettingPanel Debug Check ===");
            
            // 1. Check if SettingPanel exists in scene
            GameObject panel = GameObject.Find("SettingPanel");
            Debug.Log($"1. SettingPanel found in scene: {panel != null}");
            
            if (panel == null)
            {
                Debug.LogError("SettingPanel does not exist in scene!");
                return;
            }
            
            // 2. Check if SettingManager component exists
            SettingManager manager = panel.GetComponent<SettingManager>();
            Debug.Log($"2. SettingManager component: {manager != null}");
            
            if (manager == null)
            {
                Debug.LogError("SettingManager component is MISSING!");
                Debug.Log("   → Try: Remove SettingManager and re-add it");
                return;
            }
            
            // 3. Check if component is enabled
            Debug.Log($"3. SettingManager enabled: {manager.enabled}");
            
            // 4. Check UI references
            Debug.Log("4. UI References:");
            Debug.Log($"   - bgmSlider: {manager != null}");
            
            // 5. Check active state
            Debug.Log($"5. SettingPanel active: {panel.activeSelf}");
            
            // 6. Try FindFirstObjectByType
            SettingManager found = Object.FindFirstObjectByType<SettingManager>();
            Debug.Log($"6. FindFirstObjectByType result: {found != null}");
            
            Debug.Log("=== Debug Check Complete ===");
        }
        
        [MenuItem("Tools/Game/FIX Add SettingManager Component")]
        public static void FixAddComponent()
        {
            GameObject panel = GameObject.Find("SettingPanel");
            
            if (panel == null)
            {
                Debug.LogError("SettingPanel not found!");
                return;
            }
            
            SettingManager manager = panel.GetComponent<SettingManager>();
            if (manager != null)
            {
                Debug.Log("SettingManager already exists!");
                return;
            }
            
            manager = panel.AddComponent<SettingManager>();
            Debug.Log("SettingManager component ADDED!");
            Debug.Log("NOW: Connect UI references in Inspector manually!");
        }
    }
}
