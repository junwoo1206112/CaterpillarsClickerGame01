using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class TouchFunctionListItem : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text functionNameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text levelText;
        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;

        private string _functionName;
        private string _description;
        private int _level;

        public System.Action<string> OnAddClicked;
        public System.Action<string> OnRemoveClicked;

        public void Initialize(string name, string description, int level)
        {
            _functionName = name;
            _description = description;
            _level = level;

            if (functionNameText != null)
                functionNameText.text = name;

            if (descriptionText != null)
                descriptionText.text = description;

            if (levelText != null)
                levelText.text = $"Lv. {level}";

            SetupButtons();
        }

        private void SetupButtons()
        {
            if (addButton != null)
            {
                addButton.onClick.RemoveAllListeners();
                addButton.onClick.AddListener(() => OnAddClicked?.Invoke(_functionName));
            }

            if (removeButton != null)
            {
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(() => OnRemoveClicked?.Invoke(_functionName));
            }
        }

        public void SetInteractable(bool interactable)
        {
            if (addButton != null)
                addButton.interactable = interactable;

            if (removeButton != null)
                removeButton.interactable = interactable;
        }
    }
}
