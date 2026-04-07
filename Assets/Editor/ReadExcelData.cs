using UnityEngine;
using UnityEditor;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class ReadExcelData : EditorWindow
    {
        [MenuItem("Tools/Data/Print Excel Data")]
        public static void PrintExcelData()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";
            
            if (!File.Exists(excelPath))
            {
                Debug.LogError($"Excel file not found: {excelPath}");
                return;
            }

            Debug.Log("=== EXCEL DATA START ===");
            
            using (var file = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                
                for (int s = 0; s < workbook.NumberOfSheets; s++)
                {
                    string sheetName = workbook.GetSheetName(s);
                    Debug.Log($"\n=== SHEET: {sheetName} ===");
                    
                    ISheet sheet = workbook.GetSheetAt(s);
                    
                    // Header row
                    IRow headerRow = sheet.GetRow(0);
                    if (headerRow != null)
                    {
                        string[] headers = new string[headerRow.LastCellNum];
                        for (int i = 0; i < headerRow.LastCellNum; i++)
                        {
                            headers[i] = headerRow.GetCell(i)?.ToString() ?? "";
                        }
                        Debug.Log($"Headers: {string.Join(", ", headers)}");
                    }
                    
                    // Data rows (first 5 rows)
                    for (int i = 1; i <= Mathf.Min(5, sheet.LastRowNum); i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            string[] values = new string[row.LastCellNum];
                            for (int j = 0; j < row.LastCellNum; j++)
                            {
                                var cell = row.GetCell(j);
                                values[j] = cell?.ToString() ?? "";
                            }
                            Debug.Log($"Row {i+1}: {string.Join(", ", values)}");
                        }
                    }
                }
            }
            
            Debug.Log("\n=== EXCEL DATA END ===");
        }
    }
}
