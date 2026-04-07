using UnityEngine;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using ClickerGame.Gameplay;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class GameSetup : MonoBehaviour
    {
    [Header("Data References")]
    [SerializeField] private EvolutionStageListSO _characterList;
        [SerializeField] private TouchFunctionListSO _touchFunctionList;
        [SerializeField] private ItemListSO _itemList;

        [Header("Auto Create")]
        [SerializeField] private bool _autoCreateManagers = true;

        private GameContainer _container;
        private DataManager _dataManager;

        private void Awake()
        {
            if (_autoCreateManagers)
            {
                CreateGameContainer();
                CreateManagers();
                LoadData();
            }
        }

        private void CreateGameContainer()
        {
            _container = new GameContainer();

            // 데이터 서비스 등록 (DI)
            var characterService = new ExcelDataService<EvolutionStageDataModel>();
            var backgroundService = new ExcelDataService<BackgroundDataModel>();
            var itemService = new ExcelDataService<ItemDataModel>();
            var configService = new ExcelDataService<ConfigDataModel>();

            _container.Register<IDataService<List<EvolutionStageDataModel>>, ExcelDataService<EvolutionStageDataModel>>(characterService);
            _container.Register<IDataService<List<BackgroundDataModel>>, ExcelDataService<BackgroundDataModel>>(backgroundService);
            _container.Register<IDataService<List<ItemDataModel>>, ExcelDataService<ItemDataModel>>(itemService);
            _container.Register<IDataService<List<ConfigDataModel>>, ExcelDataService<ConfigDataModel>>(configService);

            Debug.Log("[GameSetup] GameContainer created with DI");
        }

        private void CreateManagers()
        {
            // DataManager 생성 및 DI 주입
            GameObject managerObj = GameObject.Find("DataManager");
            if (managerObj == null)
            {
                managerObj = new GameObject("DataManager");
            }

            _dataManager = managerObj.GetComponent<DataManager>();
            if (_dataManager == null)
            {
                _dataManager = managerObj.AddComponent<DataManager>();
            }

    // DI 컨테이너에서 서비스 주입
    var characterService = _container.Resolve<IDataService<List<EvolutionStageDataModel>>>();
            var backgroundService = _container.Resolve<IDataService<List<BackgroundDataModel>>>();
            var itemService = _container.Resolve<IDataService<List<ItemDataModel>>>();
            var configService = _container.Resolve<IDataService<List<ConfigDataModel>>>();

            _dataManager.Initialize(characterService, backgroundService, itemService, configService);

            Debug.Log("[GameSetup] DataManager initialized with DI");
        }

        private async void LoadData()
        {
            string excelPath = "Assets/ExcelData/GameData.xlsx";
            await _dataManager.LoadDataAsync(excelPath);
            Debug.Log("[GameSetup] Data loading complete");
        }

        private void OnValidate()
        {
    if (_characterList == null)
    {
        string path = "Assets/ScriptableObjects/Character/EvolutionStageList.asset";
        _characterList = UnityEditor.AssetDatabase.LoadAssetAtPath<EvolutionStageListSO>(path);
    }

            if (_touchFunctionList == null)
            {
                string path = "Assets/ScriptableObjects/TouchFunction/TouchFunctionList.asset";
                _touchFunctionList = UnityEditor.AssetDatabase.LoadAssetAtPath<TouchFunctionListSO>(path);
            }

            if (_itemList == null)
            {
                string path = "Assets/ScriptableObjects/Item/ItemList.asset";
                _itemList = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemListSO>(path);
            }
        }
    }
}
