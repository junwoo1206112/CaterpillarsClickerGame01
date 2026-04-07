using UnityEngine;
using ClickerGame.Core;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.Tests
{
    public class Phase1Test : MonoBehaviour
    {
        [Header("Test Results")]
        [TextArea]
        public string testLog = "";

        private GameContainer _container;

        private void Start()
        {
            RunAllTests();
        }

        public void RunAllTests()
        {
            testLog = "=== Phase 1 Test Started ===\n\n";

            Test_DIContainer();
            Test_DataService();
            Test_ExcelConverter();

            testLog += "\n=== All Tests Completed ===";
            Debug.Log(testLog);
        }

        private void Test_DIContainer()
        {
            testLog += "[TEST] DI Container...\n";

            try
            {
                _container = new GameContainer();

                var dataService = new ExcelDataService<EvolutionStageDataModel>();
                _container.Register<IDataService<List<EvolutionStageDataModel>>, ExcelDataService<EvolutionStageDataModel>>(dataService);

                var resolved = _container.Resolve<IDataService<List<EvolutionStageDataModel>>>();

                if (resolved != null)
                {
                    testLog += "✓ DI Container: PASS\n";
                }
                else
                {
                    testLog += "✗ DI Container: FAIL (resolved is null)\n";
                }
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ DI Container: FAIL ({ex.Message})\n";
            }

            testLog += "\n";
        }

        private void Test_DataService()
        {
            testLog += "[TEST] Excel Data Service...\n";

            try
            {
                string excelPath = "Assets/ExcelData/GameData.xlsx";

                if (System.IO.File.Exists(excelPath))
                {
                    testLog += $"✓ Excel file exists: {excelPath}\n";

                    var dataService = new ExcelDataService<EvolutionStageDataModel>();
                    testLog += "✓ ExcelDataService created\n";
                }
                else
                {
                    testLog += $"✗ Excel file not found: {excelPath}\n";
                    testLog += "  → Run: Tools > Data > Create Excel Template\n";
                }
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ Excel Data Service: FAIL ({ex.Message})\n";
            }

            testLog += "\n";
        }

        private void Test_ExcelConverter()
        {
            testLog += "[TEST] Excel Converter...\n";

            try
            {
                var converter = new ClickerGame.Data.ExcelConverter();
                testLog += "✓ ExcelConverter created\n";
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ Excel Converter: FAIL ({ex.Message})\n";
            }

            testLog += "\n";
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 600, 800));
            GUILayout.BeginVertical("box");

            GUILayout.Label("=== Phase 1 Test Results ===", GUILayout.Height(30));
            GUILayout.Space(10);

            if (GUILayout.Button("Run Tests Again", GUILayout.Height(40)))
            {
                RunAllTests();
            }

            GUILayout.Space(10);
            GUILayout.Label(testLog, GUILayout.ExpandHeight(true));

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
