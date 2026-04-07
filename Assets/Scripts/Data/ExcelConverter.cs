using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

namespace ClickerGame.Data
{
    public class ExcelConverter : IExcelConverter
    {
        public void CreateTemplateExcel(string outputPath)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();

                CreateCharacterSheet(workbook);
                CreateItemSheet(workbook);
                CreateTouchFunctionSheet(workbook);
                CreateConfigSheet(workbook);
                CreateBackgroundSheet(workbook);

                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                using (var file = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(file);
                }

                Debug.Log($"Excel template created: {outputPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating Excel template: {ex.Message}");
                throw;
            }
        }

        private void CreateCharacterSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("CharacterData");
            IRow header = sheet.CreateRow(0);

            string[] columns = { "ID", "Name", "Stage", "ScoreRequired", "SpritePath", "Color" };
            for (int i = 0; i < columns.Length; i++)
            {
                header.CreateCell(i).SetCellValue(columns[i]);
            }

            IRow example = sheet.CreateRow(1);
            example.CreateCell(0).SetCellValue("CHAR_001");
            example.CreateCell(1).SetCellValue("Caterpillar");
            example.CreateCell(2).SetCellValue(1);
            example.CreateCell(3).SetCellValue(0);
            example.CreateCell(4).SetCellValue("Assets/Sprites/character_caterpillar");
            example.CreateCell(5).SetCellValue("#00FF00");
        }

        private void CreateBackgroundSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("BackgroundData");
            IRow header = sheet.CreateRow(0);

            string[] columns = { "ID", "Name", "SpritePath", "UnlockScore" };
            for (int i = 0; i < columns.Length; i++)
            {
                header.CreateCell(i).SetCellValue(columns[i]);
            }

            IRow example = sheet.CreateRow(1);
            example.CreateCell(0).SetCellValue("BG_001");
            example.CreateCell(1).SetCellValue("Forest");
            example.CreateCell(2).SetCellValue("Assets/Sprites/background_forest");
            example.CreateCell(3).SetCellValue(0);
        }

        private void CreateItemSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("ItemData");
            IRow header = sheet.CreateRow(0);

            string[] columns = { "ID", "Name", "Effect", "Value", "Duration", "IconPath" };
            for (int i = 0; i < columns.Length; i++)
            {
                header.CreateCell(i).SetCellValue(columns[i]);
            }

            IRow example = sheet.CreateRow(1);
            example.CreateCell(0).SetCellValue("ITEM_001");
            example.CreateCell(1).SetCellValue("+50 Clicks");
            example.CreateCell(2).SetCellValue("AddScore");
            example.CreateCell(3).SetCellValue(50);
            example.CreateCell(4).SetCellValue(0f);
            example.CreateCell(5).SetCellValue("Assets/Sprites/icon_item_001");
        }

        private void CreateTouchFunctionSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("TouchFunctionData");
            IRow header = sheet.CreateRow(0);

            string[] columns = { "ID", "FunctionName", "FunctionType", "TriggerCount", "Multiplier", "Duration", "Cooldown", "CriticalChance", "IsActive", "IsReusable" };
            for (int i = 0; i < columns.Length; i++)
            {
                header.CreateCell(i).SetCellValue(columns[i]);
            }

            IRow example1 = sheet.CreateRow(1);
            example1.CreateCell(0).SetCellValue("TOUCH_001");
            example1.CreateCell(1).SetCellValue("Bonus Click");
            example1.CreateCell(2).SetCellValue("BonusClick");
            example1.CreateCell(3).SetCellValue(50);
            example1.CreateCell(4).SetCellValue(1.33f);
            example1.CreateCell(5).SetCellValue(0f);
            example1.CreateCell(6).SetCellValue(0f);
            example1.CreateCell(7).SetCellValue(0f);
            example1.CreateCell(8).SetCellValue(true);
            example1.CreateCell(9).SetCellValue(true);

            IRow example2 = sheet.CreateRow(2);
            example2.CreateCell(0).SetCellValue("TOUCH_002");
            example2.CreateCell(1).SetCellValue("Speed Boost");
            example2.CreateCell(2).SetCellValue("SpeedBoost");
            example2.CreateCell(3).SetCellValue(0);
            example2.CreateCell(4).SetCellValue(2f);
            example2.CreateCell(5).SetCellValue(60f);
            example2.CreateCell(6).SetCellValue(300f);
            example2.CreateCell(7).SetCellValue(0f);
            example2.CreateCell(8).SetCellValue(true);
            example2.CreateCell(9).SetCellValue(true);

            IRow example3 = sheet.CreateRow(3);
            example3.CreateCell(0).SetCellValue("TOUCH_003");
            example3.CreateCell(1).SetCellValue("Critical Click");
            example3.CreateCell(2).SetCellValue("CriticalClick");
            example3.CreateCell(3).SetCellValue(0);
            example3.CreateCell(4).SetCellValue(5f);
            example3.CreateCell(5).SetCellValue(0f);
            example3.CreateCell(6).SetCellValue(0f);
            example3.CreateCell(7).SetCellValue(0.1f);
            example3.CreateCell(8).SetCellValue(true);
            example3.CreateCell(9).SetCellValue(true);
        }

        private void CreateConfigSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("ConfigData");
            IRow header = sheet.CreateRow(0);

            string[] columns = { "Key", "Value", "Description" };
            for (int i = 0; i < columns.Length; i++)
            {
                header.CreateCell(i).SetCellValue(columns[i]);
            }

            IRow example = sheet.CreateRow(1);
            example.CreateCell(0).SetCellValue("BGM_VOLUME");
            example.CreateCell(1).SetCellValue("0.5");
            example.CreateCell(2).SetCellValue("Background music volume (0-1)");
        }

        public ScriptableObject ConvertToScriptableObject(string excelPath)
        {
            Debug.LogWarning("ConvertToScriptableObject is not implemented. Use Editor-only converter.");
            return null;
        }
    }
}
