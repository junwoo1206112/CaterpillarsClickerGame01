using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using ClickerGame.UI;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class ForceAddSettingPanelToScene
    {
        [MenuItem("Tools/Game/FORCE Add SettingPanel to Scene")]
        public static void ForceAddToScene()
        {
            string prefabPath = "Assets/Prefabs/SettingPanel.prefab";
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError("[ForceAdd] Prefab not found! Create it first.");
                return;
            }
            
            GameObject existing = GameObject.Find("SettingPanel");
            if (existing != null)
            {
                Debug.Log("[ForceAdd] SettingPanel already exists in scene");
                return;
            }
            
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = "SettingPanel";
            
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasRenderer>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Debug.Log("[ForceAdd] Created Canvas");
            }
            
            instance.transform.SetParent(canvas.transform);
            
            RectTransform rect = instance.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(400, 300);
            
            instance.SetActive(false);
            
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
            
            Debug.Log("[ForceAdd] SettingPanel ADDED to scene successfully!");
            Debug.Log("[ForceAdd] NOW SAVE THE SCENE (Ctrl+S) and Play!");
        }
    }
}
