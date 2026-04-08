using UnityEngine;
using UnityEngine.UI;

namespace ClickerGame.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Windows")]
        [SerializeField] private GameObject settingWindow;
        [SerializeField] private GameObject characterCustomizeWindow;
        [SerializeField] private GameObject backgroundWindow;

        [Header("TopBar Buttons")]
        [SerializeField] private Button settingButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button characterCustomizeButton;
        [SerializeField] private Button backgroundButton;
        [SerializeField] private Button speedBoostButton;
        [SerializeField] private Button itemButton;

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
            CloseAllWindows();
            
            // 버튼 강제 활성화
            EnableAllButtons();
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

        #region Window Management

        public void CloseAllWindows()
        {
            if (settingWindow != null) settingWindow.SetActive(false);
            if (characterCustomizeWindow != null) characterCustomizeWindow.SetActive(false);
            if (backgroundWindow != null) backgroundWindow.SetActive(false);
        }

        public void OpenSettingWindow()
        {
            CloseAllWindows();
            if (settingWindow != null)
            {
                settingWindow.SetActive(true);
            }
        }

        public void OpenCharacterCustomizeWindow()
        {
            CloseAllWindows();
            if (characterCustomizeWindow != null)
            {
                characterCustomizeWindow.SetActive(true);
            }
        }

        public void OpenBackgroundWindow()
        {
            CloseAllWindows();
            if (backgroundWindow != null)
            {
                backgroundWindow.SetActive(true);
            }
        }

        #endregion

        #region Button Events

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
            
            // 터치 횟수 100~500 랜덤 증가
            int randomCount = Random.Range(100, 501);
            Debug.Log($"[UIManager] Random count generated: {randomCount}");
            
            var touchCounter = FindFirstObjectByType<Gameplay.TouchCounter>();
            Debug.Log($"[UIManager] TouchCounter found: {touchCounter != null}");
            
            if (touchCounter != null)
            {
                Debug.Log($"[UIManager] Starting loop to add {randomCount} touches...");
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
