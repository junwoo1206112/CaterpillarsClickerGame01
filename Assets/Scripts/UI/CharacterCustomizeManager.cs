using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Gameplay;

namespace ClickerGame.UI
{
    public class CharacterCustomizeManager : MonoBehaviour
    {
        [System.Serializable]
        public class CustomizeItem
        {
            public string name;
            public Sprite sprite;
            public Vector2 caterpillarOffset;
            public Vector2 butterflyOffset;
        }
        
        [Header("Accessories")]
        [SerializeField] private CustomizeItem[] items;
        
        [Header("UI")]
        [SerializeField] private Transform itemsGrid;
        [SerializeField] private Button closeButton;
        
        [Header("Target Character")]
        [SerializeField] private Transform characterTransform;

        private SpriteRenderer _accessoryRenderer;
        private GameObject _accessoryObj;
        private Font _font;
        private CustomizeItem _currentItem;
        
        private void RefreshItems()
        {
            if (itemsGrid == null) return;

            foreach (Transform child in itemsGrid)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var item in items)
            {
                CreateItemButton(item);
            }
        }
        
        private void CreateItemButton(CustomizeItem item)
        {
            GameObject buttonObj = new GameObject("Item_" + item.name);
            buttonObj.transform.SetParent(itemsGrid, false);
            
            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(300, 220);
            
            Image buttonBg = buttonObj.AddComponent<Image>();
            buttonBg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            VerticalLayoutGroup vlg = buttonObj.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(10, 10, 10, 10);
            vlg.spacing = 5;
            vlg.childAlignment = TextAnchor.MiddleCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            if (item.sprite != null)
            {
                GameObject iconObj = new GameObject("Icon");
                iconObj.transform.SetParent(buttonObj.transform, false);
                LayoutElement iconLayout = iconObj.AddComponent<LayoutElement>();
                iconLayout.preferredHeight = 150;
                iconLayout.flexibleWidth = 1;
                Image iconImage = iconObj.AddComponent<Image>();
                iconImage.sprite = item.sprite;
                iconImage.preserveAspect = true;
            }
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            LayoutElement textLayout = textObj.AddComponent<LayoutElement>();
            textLayout.preferredHeight = 30;
            textLayout.flexibleWidth = 1;
            Text text = textObj.AddComponent<Text>();
            
            text.text = item.name;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            if (_font == null) _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.font = _font;
            text.fontSize = 20;
            
            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => OnItemSelected(item));
        }
        
        public void OnItemSelected(CustomizeItem item)
        {
            Debug.Log($"[Customize] Selected: {item.name}");
            _currentItem = item;

            EnsureAccessoryRenderer();

            if (_accessoryRenderer != null && item.sprite != null)
            {
                _accessoryRenderer.sprite = item.sprite;
                _accessoryRenderer.enabled = true;
                _accessoryRenderer.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                UpdateAccessoryPosition();
                Debug.Log($"[Customize] Accessory applied: {item.name}");
            }
            
            OnCloseClicked();
        }

        private void UpdateAccessoryPosition()
        {
            if (_currentItem == null || _accessoryObj == null) return;

            Transform target = characterTransform;
            if (target == null)
            {
                var bug = GameObject.Find("Bug");
                if (bug != null) target = bug.transform;
            }
            if (target == null) return;

            var evolution = target.GetComponent<CharacterEvolution>();
            if (evolution == null)
            {
                _accessoryRenderer.enabled = false;
                return;
            }

            int stage = evolution.CurrentStage;

            if (stage == 2)
            {
                _accessoryRenderer.enabled = false;
                return;
            }

            Vector2 offset = Vector2.zero;

            if (stage == 1)
            {
                offset = _currentItem.caterpillarOffset;
            }
            else if (stage == 3)
            {
                offset = _currentItem.butterflyOffset;
            }

            _accessoryObj.transform.localPosition = new Vector3(offset.x, offset.y, 0);
            _accessoryRenderer.enabled = true;
        }

public void RemoveAccessory()
        {
            if (_accessoryRenderer != null)
            {
                _accessoryRenderer.enabled = false;
            }
            _currentItem = null;
        }

        private void EnsureAccessoryRenderer()
        {
            if (_accessoryRenderer != null) return;

            Transform target = characterTransform;
            if (target == null)
            {
                var bug = GameObject.Find("Bug");
                if (bug != null) target = bug.transform;
            }
            if (target == null) return;

            Transform accessoryChild = target.Find("Accessory");
            if (accessoryChild == null)
            {
                _accessoryObj = new GameObject("Accessory");
                _accessoryObj.transform.SetParent(target, false);
                _accessoryObj.transform.localPosition = Vector3.zero;
                _accessoryObj.transform.localScale = Vector3.one;
                _accessoryRenderer = _accessoryObj.AddComponent<SpriteRenderer>();
                _accessoryRenderer.sortingOrder = 15;
                Debug.Log("[Customize] Accessory 오브젝트 생성 완료");
            }
            else
            {
                _accessoryObj = accessoryChild.gameObject;
                _accessoryRenderer = accessoryChild.GetComponent<SpriteRenderer>();
                if (_accessoryRenderer == null)
                {
                    _accessoryRenderer = accessoryChild.gameObject.AddComponent<SpriteRenderer>();
                    _accessoryRenderer.sortingOrder = 15;
                }
            }
        }
        
        public void OnCloseClicked()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            RefreshItems();
        }

        private void Start()
        {
            var evolution = FindEvolution();
            if (evolution != null)
            {
                evolution.OnEvolutionNameChanged.AddListener(OnEvolutionChanged);
            }
        }

        private void OnDestroy()
        {
            var evolution = FindEvolution();
            if (evolution != null)
            {
                evolution.OnEvolutionNameChanged.RemoveListener(OnEvolutionChanged);
            }
        }

        private CharacterEvolution FindEvolution()
        {
            if (characterTransform != null)
                return characterTransform.GetComponent<CharacterEvolution>();

            var bug = GameObject.Find("Bug");
            if (bug != null)
                return bug.GetComponent<CharacterEvolution>();

            return null;
        }

        private void OnEvolutionChanged(string name)
        {
            Debug.Log($"[Customize] Evolution changed to: {name}");
            if (_currentItem != null)
            {
                EnsureAccessoryRenderer();
                UpdateAccessoryPosition();
            }
            else if (_accessoryRenderer != null)
            {
                _accessoryRenderer.enabled = false;
            }
        }
    }
}