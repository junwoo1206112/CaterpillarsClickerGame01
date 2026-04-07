using UnityEditor;
using UnityEngine;
using ClickerGame.Data.Models;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class CheckExcelData : EditorWindow
    {
        [MenuItem("Tools/Data/Check Character Data")]
        public static void CheckData()
        {
            Debug.Log("=== EvolutionStageData 확인 ===");
            
            string soPath = "Assets/ScriptableObjects/Character/EvolutionStageList.asset";
            
            if (!File.Exists(soPath))
            {
                Debug.LogError("EvolutionStageList.asset 이 없습니다!");
                Debug.LogWarning("Tools > Data > Convert Excel to ScriptableObject 실행하세요!");
                return;
            }
            
            var evolutionStageList = AssetDatabase.LoadAssetAtPath<EvolutionStageListSO>(soPath);
            
            if (evolutionStageList == null)
            {
                Debug.LogError("EvolutionStageList 를 로드할 수 없습니다!");
                return;
            }
            
            Debug.Log($"총 {evolutionStageList.Stages.Count}개 진화 단계");
            
            foreach (var stage in evolutionStageList.Stages)
            {
                Debug.Log($"ID: {stage.ID} | Name: {stage.Name} | TouchRequired: {stage.TouchRequired}");
            }
            
            Debug.Log("=== 확인 완료 ===");
        }
        
        [MenuItem("Tools/Data/Open Excel File")]
        public static void OpenExcel()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";
            
            if (File.Exists(excelPath))
            {
                System.Diagnostics.Process.Start(excelPath);
                Debug.Log("Excel 파일을 열었습니다!");
            }
            else
            {
                Debug.LogError("Excel 파일을 찾을 수 없습니다!");
            }
        }
    }
}
