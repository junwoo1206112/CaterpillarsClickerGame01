using UnityEngine;

namespace ClickerGame.Data
{
    public interface IExcelConverter
    {
        void CreateTemplateExcel(string outputPath);
        ScriptableObject ConvertToScriptableObject(string excelPath);
    }
}
