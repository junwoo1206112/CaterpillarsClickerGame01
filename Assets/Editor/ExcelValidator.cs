using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

namespace ClickerGame.EditorTools
{
    public class ExcelValidator
    {
        private readonly List<string> _errors = new();

        public bool ValidateAll(string excelPath)
        {
            _errors.Clear();

            using (var file = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);

                ValidateSheet(workbook, "CharacterData", ValidateCharacterRow);
                ValidateSheet(workbook, "BackgroundData", ValidateBackgroundRow);
                ValidateSheet(workbook, "ItemData", ValidateItemRow);
                ValidateSheet(workbook, "TouchFunctionData", ValidateTouchFunctionRow);
                ValidateSheet(workbook, "ConfigData", ValidateConfigRow);
            }

            if (_errors.Count > 0)
            {
                foreach (var error in _errors)
                {
                    Debug.LogError(error);
                }
                return false;
            }

            Debug.Log("All data validation passed!");
            return true;
        }

        private void ValidateSheet(IWorkbook workbook, string sheetName, Func<IRow, int, bool> validateRow)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                _errors.Add($"Sheet '{sheetName}' not found!");
                return;
            }

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                _errors.Add($"Sheet '{sheetName}' has no header row!");
                return;
            }

            var fieldMap = GetFieldMap(headerRow);
            var ids = new HashSet<string>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                if (!validateRow(row, i))
                {
                    continue;
                }

                string id = GetStringValue(row, fieldMap, "ID");
                if (!string.IsNullOrEmpty(id))
                {
                    if (!ids.Add(id))
                    {
                        _errors.Add($"[{sheetName}] Duplicate ID '{id}' at row {i + 1}");
                    }
                }
            }
        }

        private bool ValidateCharacterRow(IRow row, int rowNum)
        {
            var fieldMap = GetFieldMap(row.Sheet.GetRow(0));

            string id = GetStringValue(row, fieldMap, "ID");
            if (string.IsNullOrEmpty(id))
            {
                _errors.Add($"[CharacterData] Empty ID at row {rowNum + 1}");
                return false;
            }

            string name = GetStringValue(row, fieldMap, "Name");
            if (string.IsNullOrEmpty(name))
            {
                _errors.Add($"[CharacterData] Empty Name at row {rowNum + 1}");
                return false;
            }

            int stage = GetIntValue(row, fieldMap, "Stage");
            if (stage < 1 || stage > 3)
            {
                _errors.Add($"[CharacterData] Invalid Stage ({stage}) at row {rowNum + 1}. Must be 1-3.");
                return false;
            }

            return true;
        }

        private bool ValidateBackgroundRow(IRow row, int rowNum)
        {
            var fieldMap = GetFieldMap(row.Sheet.GetRow(0));

            string id = GetStringValue(row, fieldMap, "ID");
            if (string.IsNullOrEmpty(id))
            {
                _errors.Add($"[BackgroundData] Empty ID at row {rowNum + 1}");
                return false;
            }

            string name = GetStringValue(row, fieldMap, "Name");
            if (string.IsNullOrEmpty(name))
            {
                _errors.Add($"[BackgroundData] Empty Name at row {rowNum + 1}");
                return false;
            }

            return true;
        }

        private bool ValidateItemRow(IRow row, int rowNum)
        {
            var fieldMap = GetFieldMap(row.Sheet.GetRow(0));

            string id = GetStringValue(row, fieldMap, "ID");
            if (string.IsNullOrEmpty(id))
            {
                _errors.Add($"[ItemData] Empty ID at row {rowNum + 1}");
                return false;
            }

            string name = GetStringValue(row, fieldMap, "Name");
            if (string.IsNullOrEmpty(name))
            {
                _errors.Add($"[ItemData] Empty Name at row {rowNum + 1}");
                return false;
            }

            return true;
        }

        private bool ValidateTouchFunctionRow(IRow row, int rowNum)
        {
            var fieldMap = GetFieldMap(row.Sheet.GetRow(0));

            string id = GetStringValue(row, fieldMap, "ID");
            if (string.IsNullOrEmpty(id))
            {
                _errors.Add($"[TouchFunctionData] Empty ID at row {rowNum + 1}");
                return false;
            }

            string functionName = GetStringValue(row, fieldMap, "FunctionName");
            if (string.IsNullOrEmpty(functionName))
            {
                _errors.Add($"[TouchFunctionData] Empty FunctionName at row {rowNum + 1}");
                return false;
            }

            string functionType = GetStringValue(row, fieldMap, "FunctionType");
            if (string.IsNullOrEmpty(functionType))
            {
                _errors.Add($"[TouchFunctionData] Empty FunctionType at row {rowNum + 1}");
                return false;
            }

            int triggerCount = GetIntValue(row, fieldMap, "TriggerCount");
            if (triggerCount < 0)
            {
                _errors.Add($"[TouchFunctionData] Negative TriggerCount at row {rowNum + 1}");
                return false;
            }

            float multiplier = GetFloatValue(row, fieldMap, "Multiplier");
            if (multiplier < 0)
            {
                _errors.Add($"[TouchFunctionData] Negative Multiplier at row {rowNum + 1}");
                return false;
            }

            return true;
        }

        private bool ValidateConfigRow(IRow row, int rowNum)
        {
            var fieldMap = GetFieldMap(row.Sheet.GetRow(0));

            string key = GetStringValue(row, fieldMap, "Key");
            if (string.IsNullOrEmpty(key))
            {
                _errors.Add($"[ConfigData] Empty Key at row {rowNum + 1}");
                return false;
            }

            return true;
        }

        private Dictionary<string, int> GetFieldMap(IRow headerRow)
        {
            var fieldMap = new Dictionary<string, int>();
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                var cell = headerRow.GetCell(i);
                if (cell != null)
                {
                    fieldMap[cell.StringCellValue.Trim()] = i;
                }
            }
            return fieldMap;
        }

        private string GetStringValue(IRow row, Dictionary<string, int> fieldMap, string fieldName)
        {
            if (!fieldMap.TryGetValue(fieldName, out int colIndex)) return string.Empty;

            ICell cell = row.GetCell(colIndex);
            if (cell == null) return string.Empty;

            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }

            return cell.ToString();
        }

        private int GetIntValue(IRow row, Dictionary<string, int> fieldMap, string fieldName)
        {
            if (!fieldMap.TryGetValue(fieldName, out int colIndex)) return 0;

            ICell cell = row.GetCell(colIndex);
            if (cell == null) return 0;

            if (cell.CellType == CellType.Numeric)
            {
                return (int)cell.NumericCellValue;
            }

            if (int.TryParse(cell.ToString(), out int value))
            {
                return value;
            }

            return 0;
        }

        private float GetFloatValue(IRow row, Dictionary<string, int> fieldMap, string fieldName)
        {
            if (!fieldMap.TryGetValue(fieldName, out int colIndex)) return 0f;

            ICell cell = row.GetCell(colIndex);
            if (cell == null) return 0f;

            if (cell.CellType == CellType.Numeric)
            {
                return (float)cell.NumericCellValue;
            }

            if (float.TryParse(cell.ToString(), out float value))
            {
                return value;
            }

            return 0f;
        }
    }
}
