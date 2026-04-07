using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using ClickerGame.Data.Models;
using ClickerGame.UI;

namespace ClickerGame.Gameplay
{
    public class CharacterEvolution : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<EvolutionStageDataModel> _evolutionStages;
        [SerializeField] private Transform _spriteParent;

        [Header("Events")]
        public UnityEvent<int> OnEvolution;
        public UnityEvent<string> OnEvolutionNameChanged;

        private int _currentStage = 1;
        private int _totalTouchCount = 0;
        private SpriteRenderer _spriteRenderer;

        public int CurrentStage => _currentStage;
        public int MaxStages => _evolutionStages?.Count ?? 3;

        private void Awake()
        {
            if (OnEvolution == null)
                OnEvolution = new UnityEvent<int>();

            if (OnEvolutionNameChanged == null)
                OnEvolutionNameChanged = new UnityEvent<string>();

            if (_spriteParent == null)
                _spriteParent = transform;

            // 3D 오브젝트 (MeshRenderer) 또는 2D 스프라이트 (SpriteRenderer) 지원
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                // 3D 오브젝트는 MeshRenderer 가 있으므로 SpriteRenderer 추가 안함
                var meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    // 2D 용도라면 SpriteRenderer 추가
                    _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                }
            }
        }

        public void Initialize(List<EvolutionStageDataModel> evolutionStages)
        {
            _evolutionStages = evolutionStages;
            
            // 초기 단계 설정 (애벌레)
            UpdateStage(1);
        }

        public void SetTouchCount(int touchCount)
        {
            _totalTouchCount = touchCount;
            CheckEvolution();
        }

        public void CheckEvolution()
        {
            if (_evolutionStages == null || _evolutionStages.Count == 0)
                return;

            int targetStage = CalculateTargetStage();

            if (targetStage > _currentStage)
            {
                UpdateStage(targetStage);
            }
        }

        private int CalculateTargetStage()
        {
            int stage = 1;

            foreach (var data in _evolutionStages)
            {
                if (_totalTouchCount >= data.TouchRequired)
                {
                    stage = int.Parse(data.ID);
                }
            }

            return stage;
        }

        private void UpdateStage(int stage)
        {
            _currentStage = stage;

            var data = FindDataForStage(stage);
            if (data != null)
            {
                Debug.Log($"[CharacterEvolution] Evolved to Stage {stage}: {data.Name}");
                
                SetSpriteByPath(data.SpritePath);

                // 이벤트로 이름 알림 (GameUI 가 받아서 표시)
                Debug.Log($"[CharacterEvolution] Invoking OnEvolutionNameChanged: {data.Name}");
                OnEvolutionNameChanged?.Invoke(data.Name);
            }
            else
            {
                Debug.LogError($"[CharacterEvolution] No data found for stage {stage}");
            }
        }

        private EvolutionStageDataModel FindDataForStage(int stage)
        {
            if (_evolutionStages == null)
                return null;

            foreach (var data in _evolutionStages)
            {
                if (data.ID == stage.ToString())
                    return data;
            }

            return null;
        }

        private void SetSpriteByPath(string spritePath)
        {
            if (string.IsNullOrEmpty(spritePath))
                return;

            // 나중에 스프라이트 로드 구현
            // Sprite sprite = Resources.Load<Sprite>(spritePath);
            // if (sprite != null && _spriteRenderer != null)
            //     _spriteRenderer.sprite = sprite;
        }

        public void SetSprite(Sprite sprite)
        {
            if (_spriteRenderer != null && sprite != null)
            {
                _spriteRenderer.sprite = sprite;
            }
        }

        public void ResetEvolution()
        {
            _currentStage = 1;
            _totalTouchCount = 0;
            UpdateStage(1);
        }
    }
}
