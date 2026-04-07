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
    [SerializeField] private List<EvolutionStageDataModel> _evolutionStages;
        [SerializeField] private TouchFunctionListSO _touchFunctionData;

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

            SetupEvents();
            InitializeComponents();
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
