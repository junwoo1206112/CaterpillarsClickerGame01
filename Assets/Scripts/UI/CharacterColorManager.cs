using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class CharacterColorManager : MonoBehaviour
    {
        [Header("Color Grid")]
        [SerializeField] private Transform colorGridParent;
        [SerializeField] private Button closeButton;

        [Header("Preview")]
        [SerializeField] private Image previewImage;

        private string[] _colorHexCodes = new string[]
        {
            "#00FF00", // Green
            "#FF0000", // Red
            "#0000FF", // Blue
            "#FFFF00", // Yellow
            "#FF00FF", // Magenta
            "#00FFFF", // Cyan
            "#FFA500", // Orange
            "#800080", // Purple
            "#FFC0CB", // Pink
            "#A52A2A"  // Brown
        };

        private void Awake()
        {
            CreateColorButtons();
            SetupButtons();
        }

        private void CreateColorButtons()
        {
            if (colorGridParent == null)
                return;

            foreach (Transform child in colorGridParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var hexColor in _colorHexCodes)
            {
                GameObject buttonObj = new GameObject("ColorButton_" + hexColor);
                buttonObj.transform.SetParent(colorGridParent, false);

                RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(50, 50);

                Image image = buttonObj.AddComponent<Image>();
                if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
                {
                    image.color = color;
                }

                Button button = buttonObj.AddComponent<Button>();
                button.onClick.AddListener(() => OnColorSelected(hexColor));
            }
        }

        private void SetupButtons()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseWindow);
        }

        private void OnColorSelected(string hexColor)
        {
            Debug.Log($"[CharacterColor] Selected color: {hexColor}");

            if (previewImage != null && ColorUtility.TryParseHtmlString(hexColor, out Color color))
            {
                previewImage.color = color;
            }

            // TODO: Apply to character
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveListener(CloseWindow);
        }
    }
}
