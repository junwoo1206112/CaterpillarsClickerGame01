using UnityEngine;
using UnityEngine.UI;
using ClickerGame.UI;

namespace ClickerGame.UI
{
    public class SettingManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
        }

        #region Button Events

        public void OnCloseClicked()
        {
            CloseWindow();
        }

        #endregion

        #region Functions

        private void CloseWindow()
        {
            var uiManager = FindFirstObjectByType<UIManager>();
            
            if (uiManager != null)
            {
                uiManager.CloseAllWindows();
            }
            else
            {
                gameObject.SetActive(false);
            }

            Debug.Log("[SettingManager] Settings Window Closed");
        }

        #endregion

        #region Cleanup

        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        #endregion
    }
}
