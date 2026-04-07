using UnityEditor;
using UnityEngine;
using ClickerGame.Data;
using System.IO;

namespace ClickerGame.EditorTools
{
    public static class DataMenu
    {
        private const string ExcelDataPath = "Assets/ExcelData";
        private const string ExcelFileName = "GameData.xlsx";

        [MenuItem("Tools/Data/Create Excel Template")]
        private static void CreateExcelTemplate()
        {
            try
            {
                string fullPath = Path.Combine(ExcelDataPath, ExcelFileName);

                IExcelConverter converter = new ExcelConverter();
                converter.CreateTemplateExcel(fullPath);

                AssetDatabase.Refresh();

                Debug.Log($"Excel template created at: {fullPath}");
                EditorUtility.DisplayDialog(
                    "Excel Template Created",
                    $"Excel template has been created at:\n{fullPath}\n\nYou can now edit the data in Excel.",
                    "OK"
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create Excel template: {ex.Message}");
                EditorUtility.DisplayDialog(
                    "Error",
                    $"Failed to create Excel template:\n{ex.Message}",
                    "OK"
                );
            }
        }

        [MenuItem("Tools/Data/Convert Excel to ScriptableObject")]
        private static void ConvertExcelToScriptableObject()
        {
            try
            {
                string fullPath = Path.Combine(ExcelDataPath, ExcelFileName);

                if (!File.Exists(fullPath))
                {
                    EditorUtility.DisplayDialog(
                        "File Not Found",
                        $"Excel file not found at:\n{fullPath}\n\nPlease create the Excel template first.",
                        "OK"
                    );
                    return;
                }

                ExcelToScriptableObjectConverter converter = new ExcelToScriptableObjectConverter();
                converter.ConvertAll(fullPath);

                AssetDatabase.Refresh();

                Debug.Log("Conversion completed successfully!");
                EditorUtility.DisplayDialog(
                    "Conversion Complete",
                    "Excel data has been converted to ScriptableObjects.\nCheck Assets/ScriptableObjects/ folder.",
                    "OK"
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to convert Excel to ScriptableObject: {ex.Message}");
                EditorUtility.DisplayDialog(
                    "Error",
                    $"Failed to convert Excel to ScriptableObject:\n{ex.Message}",
                    "OK"
                );
            }
        }

        [MenuItem("Tools/Data/Validate Excel Data")]
        private static void ValidateExcelData()
        {
            try
            {
                string fullPath = Path.Combine(ExcelDataPath, ExcelFileName);

                if (!File.Exists(fullPath))
                {
                    EditorUtility.DisplayDialog(
                        "File Not Found",
                        $"Excel file not found at:\n{fullPath}",
                        "OK"
                    );
                    return;
                }

                ExcelValidator validator = new ExcelValidator();
                bool isValid = validator.ValidateAll(fullPath);

                if (isValid)
                {
                    EditorUtility.DisplayDialog(
                        "Validation Passed",
                        "All data validation checks passed!",
                        "OK"
                    );
                }
                else
                {
                    EditorUtility.DisplayDialog(
                        "Validation Failed",
                        "Data validation failed. Check Console for details.",
                        "OK"
                    );
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Validation error: {ex.Message}");
                EditorUtility.DisplayDialog(
                    "Error",
                    $"Validation error:\n{ex.Message}",
                    "OK"
                );
            }
        }
    }
}
