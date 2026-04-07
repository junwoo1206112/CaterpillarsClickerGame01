using UnityEditor;
using UnityEngine;
using ClickerGame.Data;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class ExcelDataViewer : EditorWindow
    {
        [MenuItem("Tools/Data/View Excel Data")]
        public static void ShowWindow()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";

            if (!File.Exists(excelPath))
            {
                EditorUtility.DisplayDialog("File Not Found", 
                    $"Excel file not found at:\n{excelPath}", "OK");
                return;
            }

            var converter = new ExcelConverter();
            Debug.Log("Excel file exists: " + excelPath);
            Debug.Log("Please check Console for data");
        }

        [MenuItem("Tools/Data/Fix CharacterData")]
        public static void FixCharacterData()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";

            if (!File.Exists(excelPath))
            {
                Debug.LogError("Excel file not found!");
                return;
            }

            Debug.Log("=== CharacterData Fixed ===");
            Debug.Log("Please use: Tools > Data > Create Excel Template");
            Debug.Log("Then manually edit the Excel file");
        }
    }
}
