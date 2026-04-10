using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace ClickerGame.EditorTools
{
    public class FixSceneBrokenReferences : EditorWindow
    {
        [MenuItem("Tools/Game/Fix Broken Scene References")]
        public static void FixBrokenReferences()
        {
            if (!EditorUtility.DisplayDialog("Fix Broken References",
                "Scene 파일의 깨진 참조 (Broken PPtr) 를 복구합니다.\n\nGamePlayScene.unity 의 깨진 Text 참조를 제거합니다.\n\n계속하시겠습니까?",
                "실행", "취소"))
                return;

            Debug.Log("[FixSceneBrokenReferences] Starting to fix broken references...");

            FixGamePlayScene();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[FixSceneBrokenReferences] Fix complete!");
            EditorUtility.DisplayDialog("Fix Complete", "깨진 참조 복구 완료!\n\nUnity 를 재시작하세요.", "확인");
        }

        private static void FixGamePlayScene()
        {
            string scenePath = "Assets/Scenes/GamePlayScene.unity";
            
            if (!File.Exists(scenePath))
            {
                Debug.LogError($"[FixSceneBrokenReferences] Scene file not found: {scenePath}");
                return;
            }

            string content = File.ReadAllText(scenePath);
            int originalLength = content.Length;

            content = RemoveBrokenTextPtr(content);

            if (content.Length != originalLength)
            {
                File.WriteAllText(scenePath, content);
                Debug.Log($"[FixSceneBrokenReferences] Fixed broken PPtr references in {scenePath}");
            }
            else
            {
                Debug.Log($"[FixSceneBrokenReferences] No broken references found in {scenePath}");
            }
        }

        private static string RemoveBrokenTextPtr(string content)
        {
            string[] lines = content.Split('\n');
            System.Collections.Generic.List<string> validLines = new System.Collections.Generic.List<string>();

            foreach (string line in lines)
            {
                if (!line.Trim().StartsWith("- {fileID: 300000000") && 
                    !line.Trim().StartsWith("- {fileID: 300000001"))
                {
                    validLines.Add(line);
                }
            }

            return string.Join("\n", validLines);
        }

        [MenuItem("Tools/Game/Recreate Setting Panel Prefab")]
        public static void RecreateSettingPanel()
        {
            string prefabPath = "Assets/Prefabs/SettingPanel.prefab";

            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) != null)
            {
                AssetDatabase.DeleteAsset(prefabPath);
                Debug.Log($"[FixSceneBrokenReferences] Deleted existing SettingPanel prefab");
            }

            CreateSettingPanel.CreatePrefab();

            Debug.Log($"[FixSceneBrokenReferences] SettingPanel prefab recreated: {prefabPath}");
        }

        [MenuItem("Tools/Game/Clear Library Cache")]
        public static void ClearLibraryCache()
        {
            if (!EditorUtility.DisplayDialog("Clear Library Cache",
                "Library 폴더를 삭제하여 Unity 캐시를 초기화합니다.\n\nUnity 재시작이 필요합니다.\n\n계속하시겠습니까?",
                "실행", "취소"))
                return;

            string libraryPath = Path.Combine(Application.dataPath, "..", "Library");

            if (Directory.Exists(libraryPath))
            {
                Directory.Delete(libraryPath, true);
                Debug.Log($"[FixSceneBrokenReferences] Library folder deleted: {libraryPath}");
                Debug.Log("[FixSceneBrokenReferences] Please restart Unity Editor");
            }
            else
            {
                Debug.Log($"[FixSceneBrokenReferences] Library folder not found: {libraryPath}");
            }
        }
    }
}
