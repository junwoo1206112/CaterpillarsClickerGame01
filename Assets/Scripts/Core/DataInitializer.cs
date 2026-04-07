using UnityEngine;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class DataInitializer : MonoBehaviour
    {
        [SerializeField] private EvolutionStageListSO _characterListSO;
        [SerializeField] private BackgroundListSO _backgroundListSO;
        [SerializeField] private ItemListSO _itemListSO;
        [SerializeField] private ConfigListSO _configListSO;

        private GameContainer _container;

        public GameContainer Container => _container;

        private void Awake()
        {
            _container = new GameContainer();

            var characterDataService = new ExcelDataService<EvolutionStageDataModel>();
            var backgroundDataService = new ExcelDataService<BackgroundDataModel>();
            var itemDataService = new ExcelDataService<ItemDataModel>();
            var configDataService = new ExcelDataService<ConfigDataModel>();

            _container.Register<IDataService<List<EvolutionStageDataModel>>, ExcelDataService<EvolutionStageDataModel>>(characterDataService);
            _container.Register<IDataService<List<BackgroundDataModel>>, ExcelDataService<BackgroundDataModel>>(backgroundDataService);
            _container.Register<IDataService<List<ItemDataModel>>, ExcelDataService<ItemDataModel>>(itemDataService);
            _container.Register<IDataService<List<ConfigDataModel>>, ExcelDataService<ConfigDataModel>>(configDataService);

            var excelConverter = new ExcelConverter();
            _container.Register<IExcelConverter, ExcelConverter>(excelConverter);

            Debug.Log("DataInitializer: DI Container initialized");
        }

        public void LoadDataFromScriptableObjects()
        {
            Debug.Log("Loading data from ScriptableObjects...");

            if (_characterListSO != null && _characterListSO.Stages != null)
            {
                Debug.Log($"Loaded {_characterListSO.Stages.Count} evolution stages");
            }

            if (_backgroundListSO != null && _backgroundListSO.Backgrounds != null)
            {
                Debug.Log($"Loaded {_backgroundListSO.Backgrounds.Count} backgrounds");
            }

            if (_itemListSO != null && _itemListSO.Items != null)
            {
                Debug.Log($"Loaded {_itemListSO.Items.Count} items");
            }

            if (_configListSO != null && _configListSO.Configs != null)
            {
                Debug.Log($"Loaded {_configListSO.Configs.Count} configs");
            }
        }
    }
}
