using UnityEngine;
using ClickerGame.Gameplay;
using ClickerGame.Data.Models;
using System.Collections.Generic;

namespace ClickerGame.Core
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private ClickHandler _clickHandler;
        [SerializeField] private CharacterEvolution _evolution;
        [SerializeField] private TouchCounter _touchCounter;
        [SerializeField] private TouchFunctionManager _touchFunctionManager;

        [Header("Data")]
        [SerializeField] private TouchFunctionListSO _touchFunctionData;

        private List<EvolutionStageDataModel> _evolutionStages;
        private DataManager _dataManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            RegisterServices();
            Initialize();
        }

        private void RegisterServices()
        {
            var container = GameContainer.Instance;

            container.Register<GameplayManager>(this);

            if (_touchCounter != null)
            {
                container.Register<TouchCounter>(_touchCounter);
            }

            if (_touchFunctionManager != null)
            {
                container.Register<TouchFunctionManager>(_touchFunctionManager);
            }

            if (GameContainer.Instance.TryResolve<DataManager>(out var dataManager))
            {
                _dataManager = dataManager;
            }

            Debug.Log("[GameplayManager] Services registered to DI Container");
        }

        public void Initialize()
        {
            if (_clickHandler == null)
                _clickHandler = FindFirstObjectByType<ClickHandler>();

            if (_evolution == null)
                _evolution = FindFirstObjectByType<CharacterEvolution>();

            if (_touchCounter == null)
            {
                _touchCounter = GetComponent<TouchCounter>();
                if (_touchCounter == null)
                {
                    _touchCounter = gameObject.AddComponent<TouchCounter>();
                }
            }

            if (_touchFunctionManager == null)
                _touchFunctionManager = FindFirstObjectByType<TouchFunctionManager>();

            LoadEvolutionData();

            SetupEvents();
            InitializeComponents();
        }

private void LoadEvolutionData()
        {
            // DataInitializer 에서 데이터 가져오기
            if (DataInitializer.Instance != null)
            {
                var container = DataInitializer.Instance.Container;
                if (container.TryResolve<DataManager>(out var dataManager))
                {
                    if (dataManager.Characters != null && dataManager.Characters.Count > 0)
                    {
                        _evolutionStages = dataManager.Characters;
                        Debug.Log($"[GameplayManager] Loaded {_evolutionStages.Count} evolution stages from DataManager");
                        return;
                    }
                }
            }

            // Resources 폴더에서 로드 (백업)
            var evolutionList = Resources.Load<EvolutionStageListSO>("EvolutionStageList");
            
            if (evolutionList != null && evolutionList.Stages != null)
            {
                _evolutionStages = evolutionList.Stages;
                Debug.Log($"[GameplayManager] Loaded {_evolutionStages.Count} evolution stages from Resources");
            }
            else
            {
                Debug.LogError("[GameplayManager] EvolutionStageList not found in Resources!");
                _evolutionStages = new List<EvolutionStageDataModel>();
            }
        }

        private void SetupEvents()
        {
            if (_clickHandler != null)
            {
                _clickHandler.OnClick.AddListener(OnCharacterClicked);
            }

            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.AddListener(OnTouchCountChanged);
            }
        }

        private void InitializeComponents()
        {
            var listManager = ClickerGame.UI.TouchFunctionListManager.Instance;
            if (listManager == null)
            {
                var go = new GameObject("TouchFunctionListManager");
                listManager = go.AddComponent<ClickerGame.UI.TouchFunctionListManager>();
                Debug.Log("[GameplayManager] Created TouchFunctionListManager");
            }
            
            if (_evolution != null && _evolutionStages != null)
            {
                _evolution.Initialize(_evolutionStages);
            }

            if (_touchFunctionManager != null && _touchFunctionData != null)
            {
                _touchFunctionManager.LoadFromData(_touchFunctionData);
            }
        }

        private void OnCharacterClicked()
        {
            if (_touchCounter != null)
            {
                _touchCounter.AddTouch();
            }
        }

        private void OnTouchCountChanged(int count)
        {
            if (_evolution != null)
            {
                _evolution.SetTouchCount(count);
            }

            if (_touchFunctionManager != null)
            {
                _touchFunctionManager.ProcessTouch(count);
            }
        }

        public void ActivateSpeedBoost()
        {
            if (_touchFunctionManager != null)
            {
                _touchFunctionManager.ActivateSpeedBoost();
            }
        }

        public void ResetGame()
        {
            if (_touchCounter != null)
                _touchCounter.ResetCounter();

            if (_touchFunctionManager != null)
                _touchFunctionManager.ResetAll();

            if (_evolution != null)
                _evolution.ResetEvolution();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
