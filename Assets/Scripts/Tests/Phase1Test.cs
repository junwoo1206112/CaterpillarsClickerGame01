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

        private void Start()
        {
            RunAllTests();
        }

        public void RunAllTests()
        {
            testLog = "=== Phase 1 Test Started ===\n\n";

            Test_DIContainer();
            Test_DIContainerSingleton();
            Test_DataService();
            Test_ExcelConverter();
            Test_DIContainerResolve();

            testLog += "\n=== All Tests Completed ===";
            Debug.Log(testLog);
        }

        private void Test_DIContainer()
        {
            testLog += "[TEST] DI Container...\n";

            try
            {
                var container = GameContainer.Instance;

                if (container != null)
                {
                    testLog += "✓ DI Container Instance exists\n";
                    testLog += "✓ DI Container: PASS\n";
                }
                else
                {
                    testLog += "✗ DI Container Instance is null\n";
                }
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ DI Container: FAIL ({ex.Message})\n";
            }

            testLog += "\n";
        }

        private void Test_DIContainerSingleton()
        {
            testLog += "[TEST] DI Container Singleton...\n";

            try
            {
                var container1 = GameContainer.Instance;
                var container2 = GameContainer.Instance;

                if (container1 == container2)
                {
                    testLog += "✓ Singleton pattern works\n";
                    testLog += "✓ Both references point to same instance\n";
                }
                else
                {
                    testLog += "✗ Singleton pattern failed\n";
                }
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ DI Container Singleton: FAIL ({ex.Message})\n";
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

        private void Test_DIContainerResolve()
        {
            testLog += "[TEST] DI Container Resolve...\n";

            try
            {
                var container = GameContainer.Instance;

                container.Register<ITestService, TestService>(new TestService());

                if (container.TryResolve<ITestService>(out var service))
                {
                    testLog += "✓ Service resolved from container\n";
                    testLog += $"✓ Service message: {service.GetMessage()}\n";
                    testLog += "✓ DI Container Resolve: PASS\n";
                }
                else
                {
                    testLog += "✗ Failed to resolve service\n";
                }
            }
            catch (System.Exception ex)
            {
                testLog += $"✗ DI Container Resolve: FAIL ({ex.Message})\n";
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

        private interface ITestService
        {
            string GetMessage();
        }

        private class TestService : ITestService
        {
            public string GetMessage() => "Test Service Working!";
        }
    }
}
