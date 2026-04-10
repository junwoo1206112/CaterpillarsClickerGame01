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
        [SerializeField] private SpriteRenderer _targetSpriteRenderer;

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

            _spriteParent = transform;
        }
        
        private void Start()
        {
            if (_evolutionStages == null || _evolutionStages.Count == 0)
            {
                LoadEvolutionData();
            }
        }
        
        private void LoadEvolutionData()
        {
            _evolutionStages = new List<EvolutionStageDataModel>
            {
                new EvolutionStageDataModel { ID = "1", Name = "애벌레", TouchRequired = 0, SpritePath = "Sprites/Characters/caterpillar", Scale = new Vector3(1, 1, 1) },
                new EvolutionStageDataModel { ID = "2", Name = "번데기", TouchRequired = 1000, SpritePath = "Sprites/Characters/cocoon", Scale = new Vector3(1, 1, 1) },
                new EvolutionStageDataModel { ID = "3", Name = "나비", TouchRequired = 3000, SpritePath = "Sprites/Characters/butterfly", Scale = new Vector3(1, 1, 1) }
            };
            Debug.Log($"[CharacterEvolution] Using default evolution stages with sprite paths");
            UpdateStage(1);
        }

        public void Initialize(List<EvolutionStageDataModel> evolutionStages)
        {
            _evolutionStages = evolutionStages;
            
            // 3D Mesh 비활성화
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = false;
            
            // Inspector에서 지정한 SpriteRenderer 사용
            if (_targetSpriteRenderer != null)
            {
                _spriteRenderer = _targetSpriteRenderer;
                Debug.Log($"[CharacterEvolution] Using target SpriteRenderer: {_spriteRenderer.gameObject.name}");
            }
            else
            {
                // 기존 SpriteRenderer 찾기
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                
                if (_spriteRenderer == null)
                {
                    // 없으면 새로 생성
                    GameObject spriteObj = new GameObject("Sprite");
                    spriteObj.transform.SetParent(transform);
                    spriteObj.transform.localPosition = Vector3.zero;
                    spriteObj.transform.localRotation = Quaternion.identity;
                    spriteObj.transform.localScale = Vector3.one;
                    _spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
                    Debug.Log("[CharacterEvolution] Created new SpriteRenderer");
                }
            }
            
            _spriteRenderer.sortingOrder = 10;
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
                Debug.Log($"[CharacterEvolution] Evolved to Stage {stage}: {data.Name}, SpritePath: {data.SpritePath}");
                
                SetSpriteByPath(data.SpritePath);
                
                if (_spriteRenderer != null && data.Scale != Vector3.zero)
                {
                    _spriteRenderer.transform.localScale = data.Scale;
                }

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
            {
                Debug.LogWarning("[CharacterEvolution] Sprite path is null or empty");
                return;
            }

            Sprite sprite = Resources.Load<Sprite>(spritePath);
            if (sprite != null && _spriteRenderer != null)
            {
                _spriteRenderer.sprite = sprite;
                _spriteRenderer.sortingOrder = 10;
                
                var meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                    meshRenderer.enabled = false;
                
                Debug.Log($"[CharacterEvolution] Sprite loaded: {spritePath}, size: {sprite.rect.size}");
            }
            else
            {
                if (sprite == null)
                    Debug.LogError($"[CharacterEvolution] Failed to load sprite at path: {spritePath}");
                if (_spriteRenderer == null)
                    Debug.LogError("[CharacterEvolution] SpriteRenderer is null");
            }
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
