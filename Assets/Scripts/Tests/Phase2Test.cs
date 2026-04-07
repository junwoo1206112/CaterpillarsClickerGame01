using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Gameplay;
using ClickerGame.Core;

namespace ClickerGame.Tests
{
    public class Phase2Test : MonoBehaviour
    {
        [Header("UI References")]
        public Text statusText;
        public Text scoreText;
        public Text touchCountText;
        public Text stageText;
        public Text multiplierText;
        public Text logText;
        public Button clickButton;
        public Button speedBoostButton;
        public Button resetButton;

        [Header("Test Character")]
        public GameObject testCharacter;

        private System.Text.StringBuilder _logBuilder = new System.Text.StringBuilder();

        private void Start()
        {
            CreateUI();
            SetupTestEnvironment();

            AppendLog("=== Phase 2 Test Started ===\n");
            AppendLog("Click the character or button to test!\n");
        }

        private void SetupTestEnvironment()
        {
            GameObject managerObj = new GameObject("GameplayManager");
            GameplayManager gameplayManager = managerObj.AddComponent<GameplayManager>();

            if (testCharacter == null)
            {
                testCharacter = CreateTestCharacter();
            }

            var clickHandler = testCharacter.GetComponent<ClickHandler>();
            clickHandler.SetCharacterName("Test Caterpillar");

            AppendLog("Test environment setup complete!\n");
        }

        private GameObject CreateTestCharacter()
        {
            GameObject character = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            character.name = "TestCharacter";
            character.transform.position = new Vector3(0, 1, 0);
            character.transform.localScale = new Vector3(2, 2, 2);

            var clickHandler = character.AddComponent<ClickHandler>();
            clickHandler.SetCharacterName("Caterpillar");

            var evolution = character.AddComponent<CharacterEvolution>();

            Destroy(character.GetComponent<Collider>());
            var collider = character.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            return character;
        }

        private void CreateUI()
        {
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            statusText = CreateText(canvas.transform, "Phase 2 Test", new Vector2(20, -20), 200, 30, Color.green);
            scoreText = CreateText(canvas.transform, "Score: 0", new Vector2(20, -60));
            touchCountText = CreateText(canvas.transform, "Touches: 0", new Vector2(20, -90));
            stageText = CreateText(canvas.transform, "Stage: 1 (Caterpillar)", new Vector2(20, -120));
            multiplierText = CreateText(canvas.transform, "Multiplier: 1x", new Vector2(20, -150));

            clickButton = CreateButton(canvas.transform, "Click!", new Vector2(20, -200), 150, 50, OnClickButton);
            speedBoostButton = CreateButton(canvas.transform, "Speed Boost (2x)", new Vector2(180, -200), 150, 50, OnSpeedBoostButton);
            resetButton = CreateButton(canvas.transform, "Reset", new Vector2(340, -200), 100, 50, OnResetButton);

            logText = CreateText(canvas.transform, "", new Vector2(20, -270), 500, 300);
            logText.alignment = TextAnchor.UpperLeft;
            logText.verticalOverflow = VerticalWrapMode.Overflow;
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            var gameplayManager = GameplayManager.Instance;
            if (gameplayManager == null)
                return;

            var scoreManager = FindFirstObjectByType<ScoreManager>();
            var touchCounter = FindFirstObjectByType<TouchCounter>();
            var evolution = FindFirstObjectByType<CharacterEvolution>();
            var touchFunctionManager = FindFirstObjectByType<TouchFunctionManager>();

            if (scoreManager != null)
            {
                scoreText.text = $"Score: {scoreManager.CurrentScore}";
            }

            if (touchCounter != null)
            {
                touchCountText.text = $"Touches: {touchCounter.TotalTouchCount}";
            }

            if (evolution != null)
            {
                stageText.text = $"Form: {evolution.CurrentStage}/{evolution.MaxStages}";
            }

            if (touchFunctionManager != null)
            {
                string mult = touchFunctionManager.CurrentMultiplier.ToString("F1");
                multiplierText.text = $"Multiplier: {mult}x";

                if (touchFunctionManager.IsSpeedBoostActive())
                {
                    multiplierText.color = Color.yellow;
                }
                else
                {
                    multiplierText.color = Color.white;
                }
            }
        }

        private void OnClickButton()
        {
            var clickHandler = FindObjectOfType<ClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.HandleClick();
            }
        }

        private void OnSpeedBoostButton()
        {
            var gameplayManager = GameplayManager.Instance;
            if (gameplayManager != null)
            {
                gameplayManager.ActivateSpeedBoost();
                AppendLog("Speed Boost activated!");
            }
        }

        private void OnResetButton()
        {
            var gameplayManager = GameplayManager.Instance;
            if (gameplayManager != null)
            {
                gameplayManager.ResetGame();
                AppendLog("Game reset!");
            }
        }

        private void AppendLog(string message)
        {
            _logBuilder.AppendLine(message);

            if (_logBuilder.Length > 2000)
            {
                _logBuilder.Remove(0, _logBuilder.Length - 2000);
            }

            if (logText != null)
            {
                logText.text = _logBuilder.ToString();
            }

            Debug.Log($"[Phase2Test] {message}");
        }

        private Text CreateText(Transform parent, string text, Vector2 position, int width = 300, int height = 30, Color? color = null)
        {
            GameObject textObj = new GameObject("Text_" + text);
            textObj.transform.SetParent(parent, false);
            textObj.transform.localPosition = position;

            RectTransform rectTransform = textObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 18;
            uiText.color = color ?? Color.white;

            return uiText;
        }

        private Button CreateButton(Transform parent, string text, Vector2 position, int width, int height, UnityEngine.Events.UnityAction onClick)
        {
            GameObject buttonObj = new GameObject("Button_" + text);
            buttonObj.transform.SetParent(parent, false);
            buttonObj.transform.localPosition = position;

            RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f);

            Button button = buttonObj.AddComponent<Button>();
            button.colors = CreateColorBlock();

            GameObject childText = new GameObject("Text");
            childText.transform.SetParent(buttonObj.transform, false);
            childText.transform.localPosition = Vector3.zero;

            RectTransform textRect = childText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            Text uiText = childText.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 20;
            uiText.color = Color.white;
            uiText.alignment = TextAnchor.MiddleCenter;

            button.onClick.AddListener(onClick);

            return button;
        }

        private ColorBlock CreateColorBlock()
        {
            ColorBlock cb = ColorBlock.defaultColorBlock;
            cb.normalColor = new Color(0.3f, 0.3f, 0.3f);
            cb.highlightedColor = new Color(0.4f, 0.4f, 0.4f);
            cb.pressedColor = new Color(0.5f, 0.5f, 0.5f);
            cb.disabledColor = new Color(0.2f, 0.2f, 0.2f);
            cb.colorMultiplier = 1f;
            cb.fadeDuration = 0.1f;
            return cb;
        }
    }
}
