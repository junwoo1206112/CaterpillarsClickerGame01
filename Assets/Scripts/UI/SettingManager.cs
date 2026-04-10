using UnityEngine;
using UnityEngine.UI;
using ClickerGame.Managers;

namespace ClickerGame.UI
{
    public class SettingManager : MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        
        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button testBgmButton;
        [SerializeField] private Button testSfxButton;

        private void Awake()
        {
            SetupUI();
            LoadSettings();
            
            gameObject.SetActive(false);
        }
        
        private void Start()
        {
            if (AudioManager.Instance != null)
            {
                if (bgmSlider != null)
                    bgmSlider.value = AudioManager.Instance.BGMVolume;
                if (sfxSlider != null)
                    sfxSlider.value = AudioManager.Instance.SFXVolume;
            }
        }

        private void SetupUI()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            if (bgmSlider != null)
                bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            
            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            
            if (testBgmButton != null)
                testBgmButton.onClick.AddListener(OnTestBGMClicked);
            
            if (testSfxButton != null)
                testSfxButton.onClick.AddListener(OnTestSFXClicked);
        }
        
        private void LoadSettings()
        {
            if (bgmSlider != null && AudioManager.Instance != null)
                bgmSlider.value = AudioManager.Instance.BGMVolume;
            
            if (sfxSlider != null && AudioManager.Instance != null)
                sfxSlider.value = AudioManager.Instance.SFXVolume;
        }

        #region Button Events

        public void OnCloseClicked()
        {
            CloseWindow();
        }
        
        public void OnBGMVolumeChanged(float volume)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetBGMVolume(volume);
        }
        
        public void OnSFXVolumeChanged(float volume)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetSFXVolume(volume);
        }
        
        public void OnTestBGMClicked()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopBGM();
                Debug.Log("[SettingManager] BGM Test button clicked");
            }
        }
        
        public void OnTestSFXClicked()
        {
            if (AudioManager.Instance != null)
            {
                Debug.Log("[SettingManager] SFX Test button clicked");
            }
        }

        #endregion

        #region Functions

        private void CloseWindow()
        {
            gameObject.SetActive(false);
            Debug.Log("[SettingManager] Settings Window Closed");
        }

        #endregion

        #region Cleanup

        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveListener(OnCloseClicked);
            
            if (bgmSlider != null)
                bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
            
            if (sfxSlider != null)
                sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
            
            if (testBgmButton != null)
                testBgmButton.onClick.RemoveListener(OnTestBGMClicked);
            
            if (testSfxButton != null)
                testSfxButton.onClick.RemoveListener(OnTestSFXClicked);
        }

        #endregion
    }
}
