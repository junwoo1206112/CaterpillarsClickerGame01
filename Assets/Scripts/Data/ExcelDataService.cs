using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ClickerGame.Data.Models;
using UnityEngine;

namespace ClickerGame.Data
{
    public class ExcelDataService<T> : IDataService<List<T>> where T : new()
    {
        public async Task<List<T>> LoadAsync(string path)
        {
            return await Task.Run(() =>
            {
                var dataList = new List<T>();

                if (!File.Exists(path))
                {
                    Debug.LogError($"File not found: {path}");
                    return dataList;
                }

                try
                {
                    using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new XSSFWorkbook(file);
                        ISheet sheet = workbook.GetSheetAt(0);

                        IRow headerRow = sheet.GetRow(0);
                        if (headerRow == null) return dataList;

                        var fieldMap = new Dictionary<string, int>();
                        for (int i = 0; i < headerRow.LastCellNum; i++)
                        {
                            fieldMap[headerRow.GetCell(i).StringCellValue.Trim()] = i;
                        }

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;

                            try
                            {
                                var data = new T();
                                var type = typeof(T);

                                foreach (var field in fieldMap)
                                {
                                    var fieldInfo = type.GetField(field.Key);
                                    if (fieldInfo == null) continue;

                                    ICell cell = row.GetCell(field.Value);
                                    if (cell == null) continue;

                                    object value = GetCellValue(cell, fieldInfo.FieldType);
                                    fieldInfo.SetValue(data, value);
                                }

                                dataList.Add(data);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"Error reading row {i}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading Excel file: {ex.Message}");
                }

                return dataList;
            });
        }

        public async Task SaveAsync(string path, List<T> data)
        {
            await Task.Run(() =>
            {
                try
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet(typeof(T).Name);

                    var type = typeof(T);
                    var fields = type.GetFields();

                    IRow headerRow = sheet.CreateRow(0);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(fields[i].Name);
                    }

                    for (int i = 0; i < data.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < fields.Length; j++)
                        {
                            var value = fields[j].GetValue(data[i]);
                            row.CreateCell(j).SetCellValue(value?.ToString() ?? string.Empty);
                        }
                    }

                    using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(file);
                    }

                    Debug.Log($"Saved {data.Count} records to {path}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error saving Excel file: {ex.Message}");
                }
            });
        }

        public bool Validate(List<T> data)
        {
            if (data == null || data.Count == 0)
            {
                Debug.LogWarning("No data to validate");
                return false;
            }

            var type = typeof(T);
            var idField = type.GetField("ID");

            if (idField == null) return true;

            var ids = new HashSet<string>();
            foreach (var item in data)
            {
                var id = idField.GetValue(item)?.ToString();
                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogError("Validation failed: Empty ID found");
                    return false;
                }

                if (!ids.Add(id))
                {
                    Debug.LogError($"Validation failed: Duplicate ID '{id}' found");
                    return false;
                }
            }

            return true;
        }

        private object GetCellValue(ICell cell, Type fieldType)
        {
            try
            {
                if (cell.CellType == CellType.Blank)
                {
                    return GetDefaultValue(fieldType);
                }

                if (fieldType == typeof(string))
                {
                    return cell.ToString();
                }

                if (fieldType == typeof(int))
                {
                    return (int)cell.NumericCellValue;
                }

                if (fieldType == typeof(float))
                {
                    return (float)cell.NumericCellValue;
                }

                if (fieldType == typeof(double))
                {
                    return cell.NumericCellValue;
                }

                if (fieldType == typeof(bool))
                {
                    return cell.BooleanCellValue;
                }

                return cell.ToString();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error converting cell value to {fieldType.Name}: {ex.Message}");
                return GetDefaultValue(fieldType);
            }
        }

        private object GetDefaultValue(Type type)
        {
            if (type == typeof(string)) return string.Empty;
            if (type == typeof(int)) return 0;
            if (type == typeof(float)) return 0f;
            if (type == typeof(double)) return 0.0;
            if (type == typeof(bool)) return false;
            return null;
        }
    }
}
