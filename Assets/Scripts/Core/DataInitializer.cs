using UnityEngine;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class DataInitializer : MonoBehaviour
    {
        public static DataInitializer Instance { get; private set; }

        [SerializeField] private EvolutionStageListSO _characterListSO;
        [SerializeField] private BackgroundListSO _backgroundListSO;
        [SerializeField] private ItemListSO _itemListSO;
        [SerializeField] private ConfigListSO _configListSO;

        public GameContainer Container => GameContainer.Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeContainer();
        }

        private void InitializeContainer()
        {
            var container = GameContainer.Instance;

            var characterDataService = new ExcelDataService<EvolutionStageDataModel>();
            var backgroundDataService = new ExcelDataService<BackgroundDataModel>();
            var itemDataService = new ExcelDataService<ItemDataModel>();
            var configDataService = new ExcelDataService<ConfigDataModel>();

            container.Register<IDataService<List<EvolutionStageDataModel>>, ExcelDataService<EvolutionStageDataModel>>(characterDataService);
            container.Register<IDataService<List<BackgroundDataModel>>, ExcelDataService<BackgroundDataModel>>(backgroundDataService);
            container.Register<IDataService<List<ItemDataModel>>, ExcelDataService<ItemDataModel>>(itemDataService);
            container.Register<IDataService<List<ConfigDataModel>>, ExcelDataService<ConfigDataModel>>(configDataService);

            var excelConverter = new ExcelConverter();
            container.Register<IExcelConverter, ExcelConverter>(excelConverter);

            var dataManager = new DataManager();
            dataManager.Initialize(characterDataService, backgroundDataService, itemDataService, configDataService);
            container.Register<DataManager>(dataManager);

            Debug.Log("[DataInitializer] DI Container initialized");
        }

        private void Start()
        {
            LoadDataFromScriptableObjects();
        }

        public void LoadDataFromScriptableObjects()
        {
            Debug.Log("[DataInitializer] Loading data from ScriptableObjects...");

            if (_characterListSO != null && _characterListSO.Stages != null)
            {
                Debug.Log($"[DataInitializer] Loaded {_characterListSO.Stages.Count} evolution stages");
            }

            if (_backgroundListSO != null && _backgroundListSO.Backgrounds != null)
            {
                Debug.Log($"[DataInitializer] Loaded {_backgroundListSO.Backgrounds.Count} backgrounds");
            }

            if (_itemListSO != null && _itemListSO.Items != null)
            {
                Debug.Log($"[DataInitializer] Loaded {_itemListSO.Items.Count} items");
            }

            if (_configListSO != null && _configListSO.Configs != null)
            {
                Debug.Log($"[DataInitializer] Loaded {_configListSO.Configs.Count} configs");
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                GameContainer.Reset();
                Instance = null;
            }
        }
    }
}
