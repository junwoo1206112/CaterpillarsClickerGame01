using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;
using ClickerGame.Data.Models;

namespace ClickerGame.EditorTools
{
    public class ExcelToScriptableObjectConverter
    {
        private const string CharacterSOPath = "Assets/ScriptableObjects/Character";
        private const string BackgroundSOPath = "Assets/ScriptableObjects/Background";
        private const string ItemSOPath = "Assets/ScriptableObjects/Item";
        private const string ConfigSOPath = "Assets/ScriptableObjects/Config";
        private const string TouchFunctionSOPath = "Assets/Resources";

        public void ConvertAll(string excelPath)
        {
            using (var file = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);

                ConvertCharacterData(workbook);
                ConvertBackgroundData(workbook);
                ConvertItemData(workbook);
                ConvertConfigData(workbook);
                ConvertTouchFunctionData(workbook);
            }

            AssetDatabase.SaveAssets();
        }

        private void ConvertCharacterData(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet("CharacterData");
            if (sheet == null) return;

            var evolutionStageListSO = ScriptableObject.CreateInstance<EvolutionStageListSO>();
            var stages = new List<EvolutionStageDataModel>();

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) return;

            var fieldMap = GetFieldMap(headerRow);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                var stage = ReadCharacterData(row, fieldMap);
                if (stage != null)
                {
                    stages.Add(stage);
                }
            }

            evolutionStageListSO.Stages = stages;

            string assetPath = Path.Combine(CharacterSOPath, "EvolutionStageList.asset");
            Directory.CreateDirectory(CharacterSOPath);

            if (AssetDatabase.LoadAssetAtPath<EvolutionStageListSO>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(evolutionStageListSO, assetPath);
            Debug.Log($"Created EvolutionStageList at: {assetPath}");
        }

        private EvolutionStageDataModel ReadCharacterData(IRow row, Dictionary<string, int> fieldMap)
        {
            try
            {
                var data = new EvolutionStageDataModel
                {
                    ID = GetStringValue(row, fieldMap, "ID"),
                    Name = GetStringValue(row, fieldMap, "Name"),
                    TouchRequired = GetIntValue(row, fieldMap, "TouchRequired"),
                    SpritePath = GetStringValue(row, fieldMap, "SpritePath")
                };

                if (string.IsNullOrEmpty(data.ID)) return null;

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading character data at row {row.RowNum}: {ex.Message}");
                return null;
            }
        }

        private void ConvertBackgroundData(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet("BackgroundData");
            if (sheet == null) return;

            var backgroundListSO = ScriptableObject.CreateInstance<BackgroundListSO>();
            var backgrounds = new List<BackgroundDataModel>();

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) return;

            var fieldMap = GetFieldMap(headerRow);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                var background = ReadBackgroundData(row, fieldMap);
                if (background != null)
                {
                    backgrounds.Add(background);
                }
            }

            backgroundListSO.Backgrounds = backgrounds;

            string assetPath = Path.Combine(BackgroundSOPath, "BackgroundList.asset");
            Directory.CreateDirectory(BackgroundSOPath);

            if (AssetDatabase.LoadAssetAtPath<BackgroundListSO>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(backgroundListSO, assetPath);
            Debug.Log($"Created BackgroundList at: {assetPath}");
        }

        private BackgroundDataModel ReadBackgroundData(IRow row, Dictionary<string, int> fieldMap)
        {
            try
            {
                var data = new BackgroundDataModel
                {
                    ID = GetStringValue(row, fieldMap, "ID"),
                    Name = GetStringValue(row, fieldMap, "Name"),
                    SpritePath = GetStringValue(row, fieldMap, "SpritePath"),
                    UnlockScore = GetIntValue(row, fieldMap, "UnlockScore")
                };

                if (string.IsNullOrEmpty(data.ID)) return null;

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading background data at row {row.RowNum}: {ex.Message}");
                return null;
            }
        }

        private void ConvertItemData(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet("ItemData");
            if (sheet == null) return;

            var itemListSO = ScriptableObject.CreateInstance<ItemListSO>();
            var items = new List<ItemDataModel>();

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) return;

            var fieldMap = GetFieldMap(headerRow);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                var item = ReadItemData(row, fieldMap);
                if (item != null)
                {
                    items.Add(item);
                }
            }

            itemListSO.Items = items;

            string assetPath = Path.Combine(ItemSOPath, "ItemList.asset");
            Directory.CreateDirectory(ItemSOPath);

            if (AssetDatabase.LoadAssetAtPath<ItemListSO>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(itemListSO, assetPath);
            Debug.Log($"Created ItemList at: {assetPath}");
        }

        private ItemDataModel ReadItemData(IRow row, Dictionary<string, int> fieldMap)
        {
            try
            {
                var data = new ItemDataModel
                {
                    ID = GetStringValue(row, fieldMap, "ID"),
                    Name = GetStringValue(row, fieldMap, "Name"),
                    Effect = GetStringValue(row, fieldMap, "Effect"),
                    Value = GetIntValue(row, fieldMap, "Value"),
                    Duration = GetFloatValue(row, fieldMap, "Duration"),
                    IconPath = GetStringValue(row, fieldMap, "IconPath")
                };

                if (string.IsNullOrEmpty(data.ID)) return null;

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading item data at row {row.RowNum}: {ex.Message}");
                return null;
            }
        }

        private void ConvertConfigData(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet("ConfigData");
            if (sheet == null) return;

            var configListSO = ScriptableObject.CreateInstance<ConfigListSO>();
            var configs = new List<ConfigDataModel>();

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) return;

            var fieldMap = GetFieldMap(headerRow);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                var config = ReadConfigData(row, fieldMap);
                if (config != null)
                {
                    configs.Add(config);
                }
            }

            configListSO.Configs = configs;

            string assetPath = Path.Combine(ConfigSOPath, "ConfigList.asset");
            Directory.CreateDirectory(ConfigSOPath);

            if (AssetDatabase.LoadAssetAtPath<ConfigListSO>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(configListSO, assetPath);
            Debug.Log($"Created ConfigList at: {assetPath}");
        }

        private ConfigDataModel ReadConfigData(IRow row, Dictionary<string, int> fieldMap)
        {
            try
            {
                var data = new ConfigDataModel
                {
                    Key = GetStringValue(row, fieldMap, "Key"),
                    Value = GetStringValue(row, fieldMap, "Value"),
                    Description = GetStringValue(row, fieldMap, "Description")
                };

                if (string.IsNullOrEmpty(data.Key)) return null;

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading config data at row {row.RowNum}: {ex.Message}");
                return null;
            }
        }

        private void ConvertTouchFunctionData(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheet("TouchFunctionData");
            if (sheet == null) return;

            var touchFunctionListSO = ScriptableObject.CreateInstance<TouchFunctionListSO>();
            var functions = new List<TouchFunctionDataModel>();

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null) return;

            var fieldMap = GetFieldMap(headerRow);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                var function = ReadTouchFunctionData(row, fieldMap);
                if (function != null)
                {
                    functions.Add(function);
                }
            }

            touchFunctionListSO.Functions = functions;

            string assetPath = Path.Combine(TouchFunctionSOPath, "TouchFunctionList.asset");
            Directory.CreateDirectory(TouchFunctionSOPath);

            if (AssetDatabase.LoadAssetAtPath<TouchFunctionListSO>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(touchFunctionListSO, assetPath);
            Debug.Log($"Created TouchFunctionList at: {assetPath}");
        }

        private TouchFunctionDataModel ReadTouchFunctionData(IRow row, Dictionary<string, int> fieldMap)
        {
            try
            {
                var data = new TouchFunctionDataModel
                {
                    ID = GetStringValue(row, fieldMap, "ID"),
                    FunctionName = GetStringValue(row, fieldMap, "FunctionName"),
                    FunctionType = GetStringValue(row, fieldMap, "FunctionType"),
                    TriggerCount = GetIntValue(row, fieldMap, "TriggerCount"),
                    Multiplier = GetFloatValue(row, fieldMap, "Multiplier"),
                    Duration = GetFloatValue(row, fieldMap, "Duration"),
                    Cooldown = GetFloatValue(row, fieldMap, "Cooldown"),
                    CriticalChance = GetFloatValue(row, fieldMap, "CriticalChance"),
                    IsActive = GetBoolValue(row, fieldMap, "IsActive"),
                    IsReusable = GetBoolValue(row, fieldMap, "IsReusable")
                };

                if (string.IsNullOrEmpty(data.ID)) return null;

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading touch function data at row {row.RowNum}: {ex.Message}");
                return null;
            }
        }

        private bool GetBoolValue(IRow row, Dictionary<string, int> fieldMap, string fieldName)
        {
            if (!fieldMap.TryGetValue(fieldName, out int colIndex)) return true;

            ICell cell = row.GetCell(colIndex);
            if (cell == null) return true;

            if (cell.CellType == CellType.Boolean)
            {
                return cell.BooleanCellValue;
            }

            if (int.TryParse(cell.ToString(), out int intValue))
            {
                return intValue != 0;
            }

            return cell.ToString().ToLower() == "true";
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
