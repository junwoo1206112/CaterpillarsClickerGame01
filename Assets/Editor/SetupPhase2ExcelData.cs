using UnityEngine;
using UnityEditor;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ClickerGame.EditorTools
{
    public class SetupPhase2ExcelData : EditorWindow
    {
        [MenuItem("Tools/Data/Setup Phase 2 Excel Data")]
        public static void SetupPhase2Data()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";
            
            if (!File.Exists(excelPath))
            {
                Debug.LogError($"Excel file not found: {excelPath}");
                Debug.Log("Creating new Excel file...");
                CreateNewExcel(excelPath);
                return;
            }

            Debug.Log("[SetupPhase2ExcelData] Updating Excel data...");
            UpdateExcelData(excelPath);
            
            Debug.Log("[SetupPhase2ExcelData] Excel data updated!");
            Debug.Log("Now run: Tools > Data > Convert Excel to ScriptableObject");
        }

        private static void CreateNewExcel(string path)
        {
            IWorkbook workbook = new XSSFWorkbook();
            
            CreateCharacterSheet(workbook);
            CreateTouchFunctionSheet(workbook);
            CreateBackgroundSheet(workbook);
            CreateItemSheet(workbook);
            CreateConfigSheet(workbook);
            
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(file);
            }
            
            Debug.Log($"[SetupPhase2ExcelData] New Excel created: {path}");
        }

        private static void UpdateExcelData(string path)
        {
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                
                // Remove existing sheets
                while (workbook.NumberOfSheets > 0)
                {
                    workbook.RemoveSheetAt(0);
                }
                
                // Create new sheets
                CreateCharacterSheet(workbook);
                CreateTouchFunctionSheet(workbook);
                CreateBackgroundSheet(workbook);
                CreateItemSheet(workbook);
                CreateConfigSheet(workbook);
                
                file.Close();
            }
            
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                // Need to recreate to save changes
                IWorkbook newWorkbook = new XSSFWorkbook();
                CreateCharacterSheet(newWorkbook);
                CreateTouchFunctionSheet(newWorkbook);
                CreateBackgroundSheet(newWorkbook);
                CreateItemSheet(newWorkbook);
                CreateConfigSheet(newWorkbook);
                newWorkbook.Write(file);
            }
        }

        private static void CreateCharacterSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("CharacterData");
            
            // Header
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("ID");
            header.CreateCell(1).SetCellValue("Name");
            header.CreateCell(2).SetCellValue("TouchRequired");
            header.CreateCell(3).SetCellValue("SpritePath");
            
            // Row 1: 애벌레 (기본)
            IRow row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("1");
            row1.CreateCell(1).SetCellValue("애벌레");
            row1.CreateCell(2).SetCellValue(0);
            row1.CreateCell(3).SetCellValue("Sprites/Caterpillar");
            
            // Row 2: 번데기 (1000 터치)
            IRow row2 = sheet.CreateRow(2);
            row2.CreateCell(0).SetCellValue("2");
            row2.CreateCell(1).SetCellValue("번데기");
            row2.CreateCell(2).SetCellValue(1000);
            row2.CreateCell(3).SetCellValue("Sprites/Pupa");
            
            // Row 3: 나비 (3000 터치)
            IRow row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("3");
            row3.CreateCell(1).SetCellValue("나비");
            row3.CreateCell(2).SetCellValue(3000);
            row3.CreateCell(3).SetCellValue("Sprites/Butterfly");
            
            Debug.Log("[SetupPhase2ExcelData] CharacterData sheet created");
        }

        private static void CreateTouchFunctionSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("TouchFunctionData");
            
            // Header
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("ID");
            header.CreateCell(1).SetCellValue("FunctionName");
            header.CreateCell(2).SetCellValue("FunctionType");
            header.CreateCell(3).SetCellValue("TriggerCount");
            header.CreateCell(4).SetCellValue("Multiplier");
            header.CreateCell(5).SetCellValue("Duration");
            header.CreateCell(6).SetCellValue("Cooldown");
            
            // Row 1: 크리티컬
            IRow row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("1");
            row1.CreateCell(1).SetCellValue("크리티컬");
            row1.CreateCell(2).SetCellValue("Critical");
            row1.CreateCell(3).SetCellValue(10);
            row1.CreateCell(4).SetCellValue(2.0);
            row1.CreateCell(5).SetCellValue(0);
            row1.CreateCell(6).SetCellValue(5);
            
            // Row 2: 스피드부스트
            IRow row2 = sheet.CreateRow(2);
            row2.CreateCell(0).SetCellValue("2");
            row2.CreateCell(1).SetCellValue("스피드부스트");
            row2.CreateCell(2).SetCellValue("SpeedBoost");
            row2.CreateCell(3).SetCellValue(50);
            row2.CreateCell(4).SetCellValue(1.0);
            row2.CreateCell(5).SetCellValue(10);
            row2.CreateCell(6).SetCellValue(30);
            
            // Row 3: 보너스
            IRow row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("3");
            row3.CreateCell(1).SetCellValue("보너스");
            row3.CreateCell(2).SetCellValue("Bonus");
            row3.CreateCell(3).SetCellValue(50);
            row3.CreateCell(4).SetCellValue(1.0);
            row3.CreateCell(5).SetCellValue(0);
            row3.CreateCell(6).SetCellValue(0);
            
            Debug.Log("[SetupPhase2ExcelData] TouchFunctionData sheet created");
        }

        private static void CreateBackgroundSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("BackgroundData");
            
            // Header
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("ID");
            header.CreateCell(1).SetCellValue("Name");
            header.CreateCell(2).SetCellValue("SpritePath");
            header.CreateCell(3).SetCellValue("UnlockScore");
            
            // Row 1: 기본 배경
            IRow row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("1");
            row1.CreateCell(1).SetCellValue("기본배경");
            row1.CreateCell(2).SetCellValue("Backgrounds/Default");
            row1.CreateCell(3).SetCellValue(0);
            
            // Row 2: 숲 배경
            IRow row2 = sheet.CreateRow(2);
            row2.CreateCell(0).SetCellValue("2");
            row2.CreateCell(1).SetCellValue("숲");
            row2.CreateCell(2).SetCellValue("Backgrounds/Forest");
            row2.CreateCell(3).SetCellValue(500);
            
            // Row 3: 꽃밭 배경
            IRow row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("3");
            row3.CreateCell(1).SetCellValue("꽃밭");
            row3.CreateCell(2).SetCellValue("Backgrounds/FlowerField");
            row3.CreateCell(3).SetCellValue(2000);
            
            Debug.Log("[SetupPhase2ExcelData] BackgroundData sheet created");
        }

        private static void CreateItemSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("ItemData");
            
            // Header
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("ID");
            header.CreateCell(1).SetCellValue("Name");
            header.CreateCell(2).SetCellValue("Effect");
            header.CreateCell(3).SetCellValue("Value");
            header.CreateCell(4).SetCellValue("Duration");
            header.CreateCell(5).SetCellValue("IconPath");
            
            // Row 1: 점수 부스터
            IRow row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("1");
            row1.CreateCell(1).SetCellValue("점수부스터");
            row1.CreateCell(2).SetCellValue("AddScore");
            row1.CreateCell(3).SetCellValue(10);
            row1.CreateCell(4).SetCellValue(0);
            row1.CreateCell(5).SetCellValue("Items/ScoreBooster");
            
            // Row 2: 자동클릭
            IRow row2 = sheet.CreateRow(2);
            row2.CreateCell(0).SetCellValue("2");
            row2.CreateCell(1).SetCellValue("오토클릭");
            row2.CreateCell(2).SetCellValue("AutoClick");
            row2.CreateCell(3).SetCellValue(1);
            row2.CreateCell(4).SetCellValue(30);
            row2.CreateCell(5).SetCellValue("Items/AutoClick");
            
            // Row 3: 배율 증가
            IRow row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("3");
            row3.CreateCell(1).SetCellValue("배율증가");
            row3.CreateCell(2).SetCellValue("Multiplier");
            row3.CreateCell(3).SetCellValue(2);
            row3.CreateCell(4).SetCellValue(60);
            row3.CreateCell(5).SetCellValue("Items/Multiplier");
            
            Debug.Log("[SetupPhase2ExcelData] ItemData sheet created");
        }

        private static void CreateConfigSheet(IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("ConfigData");
            
            // Header
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("Key");
            header.CreateCell(1).SetCellValue("Value");
            header.CreateCell(2).SetCellValue("Description");
            
            // Row 1: 게임 이름
            IRow row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("GameName");
            row1.CreateCell(1).SetCellValue("애벌레 클리커");
            row1.CreateCell(2).SetCellValue("게임 이름");
            
            // Row 2: 기본 배율
            IRow row2 = sheet.CreateRow(2);
            row2.CreateCell(0).SetCellValue("DefaultMultiplier");
            row2.CreateCell(1).SetCellValue("1");
            row2.CreateCell(2).SetCellValue("기본 클릭 배율");
            
            // Row 3: 보너스 활성화 터치
            IRow row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("BonusTriggerCount");
            row3.CreateCell(1).SetCellValue("50");
            row3.CreateCell(2).SetCellValue("보너스 활성화所需 터치 수");
            
            Debug.Log("[SetupPhase2ExcelData] ConfigData sheet created");
        }
    }
}
