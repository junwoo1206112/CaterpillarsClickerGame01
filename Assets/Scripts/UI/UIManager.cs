using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("TopBar Buttons")]
        [SerializeField] private Button settingButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button characterCustomizeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button speedBoostButton;
        [SerializeField] private Button itemButton;

        [Header("Panels")]
        [SerializeField] private GameObject backgroundPanel;

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

        private void Initialize()
        {
            SetupButtons();
            EnableAllButtons();
        }

        private void SetupButtons()
        {
            if (settingButton != null)
                settingButton.onClick.AddListener(OpenSettingWindow);

            if (resetButton != null)
                resetButton.onClick.AddListener(OnResetClicked);

            if (characterCustomizeButton != null)
                characterCustomizeButton.onClick.AddListener(OpenCharacterCustomizeWindow);

            if (backgroundButton != null)
                backgroundButton.onClick.AddListener(OpenBackgroundWindow);

            if (speedBoostButton != null)
                speedBoostButton.onClick.AddListener(OnSpeedBoostClicked);

            if (itemButton != null)
                itemButton.onClick.AddListener(OnItemButtonClicked);
        }

        private void EnableAllButtons()
        {
            if (settingButton != null) settingButton.gameObject.SetActive(true);
            if (resetButton != null) resetButton.gameObject.SetActive(true);
            if (characterCustomizeButton != null) characterCustomizeButton.gameObject.SetActive(true);
            if (backgroundButton != null) backgroundButton.gameObject.SetActive(true);
            if (speedBoostButton != null) speedBoostButton.gameObject.SetActive(true);
            if (itemButton != null) itemButton.gameObject.SetActive(true);
        }

        #region Button Events

        public void OpenSettingWindow()
        {
            Debug.Log("[UIManager] Setting button clicked!");
            
            // Try multiple ways to find SettingManager
            SettingManager settingManager = FindFirstObjectByType<SettingManager>();
            
            if (settingManager == null)
            {
                GameObject panel = GameObject.Find("SettingPanel");
                if (panel != null)
                {
                    settingManager = panel.GetComponent<SettingManager>();
                    Debug.Log($"[UIManager] Found via GameObject.Find: {settingManager != null}");
                }
            }
            
            if (settingManager == null)
            {
                // Try finding in Canvas children
                Canvas canvas = FindFirstObjectByType<Canvas>();
                if (canvas != null)
                {
                    Transform panelTransform = canvas.transform.Find("SettingPanel");
                    if (panelTransform != null)
                    {
                        settingManager = panelTransform.GetComponent<SettingManager>();
                        Debug.Log($"[UIManager] Found in Canvas: {settingManager != null}");
                    }
                }
            }
            
            if (settingManager != null)
            {
                settingManager.gameObject.SetActive(!settingManager.gameObject.activeSelf);
                Debug.Log($"[UIManager] SettingPanel toggled: {settingManager.gameObject.activeSelf}");
            }
            else
            {
                Debug.LogError("[UIManager] SettingManager NOT FOUND! Check if SettingPanel is in scene.");
                Debug.LogError("[UIManager] Tried: FindFirstObjectByType, GameObject.Find, Canvas.Find");
            }
        }

        public void OpenCharacterCustomizeWindow()
        {
            Debug.Log("[UIManager] Character customize button clicked!");
            var customizeManager = FindFirstObjectByType<CharacterCustomizeManager>();
            if (customizeManager != null)
            {
                customizeManager.gameObject.SetActive(!customizeManager.gameObject.activeSelf);
            }
        }

public void OpenBackgroundWindow()
        {
            Debug.Log("[UIManager] Background button clicked!");

            if (backgroundPanel == null)
                backgroundPanel = GameObject.Find("BackgroundPanel");

            if (backgroundPanel != null)
            {
                backgroundPanel.SetActive(!backgroundPanel.activeSelf);
            }
            else
            {
                Debug.LogError("[UIManager] BackgroundPanel not found! Run Tools > Game > Setup Background Panel first.");
            }
        }

        public void OnResetClicked()
        {
            var gameplayManager = FindFirstObjectByType<Core.GameplayManager>();
            if (gameplayManager != null)
            {
                gameplayManager.ResetGame();
                Debug.Log("[UIManager] Game reset!");
            }
        }

        private void OnSpeedBoostClicked()
        {
            var gameplayManager = FindFirstObjectByType<Core.GameplayManager>();
            if (gameplayManager != null)
            {
                gameplayManager.ActivateSpeedBoost();
            }
        }

        public void OnItemButtonClicked()
        {
            Debug.Log("[UIManager] Item button clicked!");
            
            int randomCount = Random.Range(100, 501);
            Debug.Log($"[UIManager] Random count generated: {randomCount}");
            
            var touchCounter = FindFirstObjectByType<Gameplay.TouchCounter>();
            
            if (touchCounter != null)
            {
                for (int i = 0; i < randomCount; i++)
                {
                    touchCounter.AddTouch();
                }
                Debug.Log($"[UIManager] Item used! +{randomCount} touches");
            }
            else
            {
                Debug.LogError("[UIManager] TouchCounter not found!");
            }
        }

        #endregion

        #region Cleanup

        private void OnDestroy()
        {
            if (settingButton != null)
                settingButton.onClick.RemoveListener(OpenSettingWindow);

            if (resetButton != null)
                resetButton.onClick.RemoveListener(OnResetClicked);

            if (characterCustomizeButton != null)
                characterCustomizeButton.onClick.RemoveListener(OpenCharacterCustomizeWindow);

            if (backgroundButton != null)
                backgroundButton.onClick.RemoveListener(OpenBackgroundWindow);

            if (speedBoostButton != null)
                speedBoostButton.onClick.RemoveListener(OnSpeedBoostClicked);

            if (itemButton != null)
                itemButton.onClick.RemoveListener(OnItemButtonClicked);
        }

        #endregion
    }
}
