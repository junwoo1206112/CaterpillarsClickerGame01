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
            // Resources 폴더에서 직접 로드
            var evolutionList = Resources.Load<EvolutionStageListSO>("EvolutionStageList");
            
            if (evolutionList != null && evolutionList.Stages != null && evolutionList.Stages.Count > 0)
            {
                _evolutionStages = evolutionList.Stages;
                Debug.Log($"[GameplayManager] Loaded {_evolutionStages.Count} evolution stages from Resources");
                return;
            }
            
            // 데이터가 없으면 빈 리스트로 시작 (에러 방지)
            Debug.LogWarning("[GameplayManager] EvolutionStageList is empty or null! Creating default data...");
            _evolutionStages = new List<EvolutionStageDataModel>
            {
                new EvolutionStageDataModel { ID = "1", Name = "애벌레", TouchRequired = 0 },
                new EvolutionStageDataModel { ID = "2", Name = "번데기", TouchRequired = 1000 },
                new EvolutionStageDataModel { ID = "3", Name = "나비", TouchRequired = 3000 }
            };
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
