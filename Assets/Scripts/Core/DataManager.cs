using UnityEngine;
using ClickerGame.Data;
using ClickerGame.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClickerGame.Core
{
    public class DataManager : MonoBehaviour
    {
        private IDataService<List<EvolutionStageDataModel>> _characterService;
        private IDataService<List<BackgroundDataModel>> _backgroundService;
        private IDataService<List<ItemDataModel>> _itemService;
        private IDataService<List<ConfigDataModel>> _configService;

        private List<EvolutionStageDataModel> _characters;
        private List<BackgroundDataModel> _backgrounds;
        private List<ItemDataModel> _items;
        private List<ConfigDataModel> _configs;

        public List<EvolutionStageDataModel> Characters => _characters;
        public List<BackgroundDataModel> Backgrounds => _backgrounds;
        public List<ItemDataModel> Items => _items;
        public List<ConfigDataModel> Configs => _configs;

        public void Initialize(
            IDataService<List<EvolutionStageDataModel>> characterService,
            IDataService<List<BackgroundDataModel>> backgroundService,
            IDataService<List<ItemDataModel>> itemService,
            IDataService<List<ConfigDataModel>> configService)
        {
            _characterService = characterService;
            _backgroundService = backgroundService;
            _itemService = itemService;
            _configService = configService;
        }

        public async Task LoadDataAsync(string excelPath)
        {
            _characters = await _characterService.LoadAsync(excelPath);
            Debug.Log($"Loaded {_characters?.Count ?? 0} characters");

            _backgrounds = await _backgroundService.LoadAsync(excelPath);
            Debug.Log($"Loaded {_backgrounds?.Count ?? 0} backgrounds");

            _items = await _itemService.LoadAsync(excelPath);
            Debug.Log($"Loaded {_items?.Count ?? 0} items");

            _configs = await _configService.LoadAsync(excelPath);
            Debug.Log($"Loaded {_configs?.Count ?? 0} configs");

            Debug.Log("DataManager: All data loaded successfully!");
        }
    }
}
