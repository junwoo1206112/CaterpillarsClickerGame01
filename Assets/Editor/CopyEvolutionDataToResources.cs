using UnityEngine;
using UnityEditor;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class CopyEvolutionDataToResources
    {
        [MenuItem("Tools/Data/Copy Evolution Data to Resources")]
        public static void CopyToResources()
        {
            string sourcePath = "Assets/ScriptableObjects/Character/EvolutionStageList.asset";
            string resourcesPath = "Assets/Resources";
            string destPath = "Assets/Resources/EvolutionStageList.asset";

            // Resources 폴더 생성 (없으면)
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                Debug.Log("[CopyToResources] Created Resources folder");
            }

            // 파일 복사
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destPath, true);
                Debug.Log($"[CopyToResources] Copied EvolutionStageList.asset to Resources");
                
                // Meta 파일도 복사
                string sourceMeta = sourcePath + ".meta";
                string destMeta = destPath + ".meta";
                if (File.Exists(sourceMeta))
                {
                    File.Copy(sourceMeta, destMeta, true);
                }
                
                AssetDatabase.Refresh();
                Debug.Log("[CopyToResources] Done!");
            }
            else
            {
                Debug.LogError("[CopyToResources] EvolutionStageList.asset not found!");
                Debug.Log("Run: Tools > Data > Convert Excel to ScriptableObject first");
            }
        }
    }
}
