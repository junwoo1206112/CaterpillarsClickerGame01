using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class BackgroundManager : MonoBehaviour
    {
        [System.Serializable]
        public class BackgroundItem
        {
            public string name;
            public GameObject particlePrefab;
        }
        
        [Header("Backgrounds")]
        [SerializeField] private BackgroundItem[] backgrounds;
        
        [Header("UI")]
        [SerializeField] private Transform backgroundsGrid;
        [SerializeField] private Button closeButton;

        [Header("Particle Spawn Point")]
        [SerializeField] private Transform particleSpawnPoint;

        private GameObject _currentParticleInstance;
        
        private void RefreshBackgrounds()
        {
            if (backgroundsGrid == null) return;

            foreach (Transform child in backgroundsGrid)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var bg in backgrounds)
            {
                CreateBackgroundButton(bg);
            }
        }
        
        private void CreateBackgroundButton(BackgroundItem bg)
        {
            GameObject buttonObj = new GameObject("Background_" + bg.name);
            buttonObj.transform.SetParent(backgroundsGrid, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            
            text.text = bg.name;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => OnBackgroundSelected(bg));
        }
        
        public void OnBackgroundSelected(BackgroundItem bg)
        {
            if (_currentParticleInstance != null)
            {
                Destroy(_currentParticleInstance);
                _currentParticleInstance = null;
            }

            if (bg.particlePrefab != null)
            {
                Vector3 spawnPos;

                if (particleSpawnPoint != null)
                {
                    spawnPos = particleSpawnPoint.position;
                }
                else
                {
                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        Vector3 top = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 5f));
                        spawnPos = new Vector3(cam.transform.position.x, top.y, 0f);
                    }
                    else
                    {
                        spawnPos = new Vector3(0, 6f, 0);
                    }
                }

                _currentParticleInstance = Instantiate(bg.particlePrefab, spawnPos, Quaternion.identity);

                var ps = _currentParticleInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var shape = ps.shape;
                    shape.enabled = true;
                    shape.shapeType = ParticleSystemShapeType.Box;
                    shape.position = Vector3.zero;
                    shape.scale = new Vector3(20f, 1f, 1f);

                    var renderer = _currentParticleInstance.GetComponent<ParticleSystemRenderer>();
                    if (renderer != null)
                    {
                        renderer.sortingOrder = 5;
                    }
                }

                Debug.Log($"[Background] Particle spawned: {bg.particlePrefab.name}");
            }
            
            OnCloseClicked();
        }
        
        public void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            RefreshBackgrounds();
        }
    }
}