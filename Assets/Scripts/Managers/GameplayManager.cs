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

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        public void Initialize()
        {
            if (_clickHandler == null)
                _clickHandler = FindFirstObjectByType<ClickHandler>();

            if (_evolution == null)
                _evolution = FindFirstObjectByType<CharacterEvolution>();

            // TouchCounter 가 없으면 GameplayManager 에 자동 추가
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

            // EvolutionStageList.asset 자동 로드
            LoadEvolutionData();

            SetupEvents();
            InitializeComponents();
        }

        private void LoadEvolutionData()
        {
            // Resources 폴더에서만 로드 (빌드 호환)
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

            // TouchCounter 이벤트로 진화 확인
            if (_touchCounter != null)
            {
                _touchCounter.OnTouchCountChanged.AddListener(OnTouchCountChanged);
            }
        }

private void InitializeComponents()
        {
            // TouchFunctionListManager 초기화 보장
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
            // 진화 확인
            if (_evolution != null)
            {
                _evolution.SetTouchCount(count);
            }

            // 터치 효과 처리
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
    }
}
