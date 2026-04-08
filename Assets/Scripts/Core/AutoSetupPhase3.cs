using UnityEngine;
using UnityEngine.UI;
using ClickerGame.UI;

namespace ClickerGame.Core
{
    public class AutoSetupPhase3 : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void AutoSetup()
        {
            Debug.Log("[AutoSetup] Checking Phase 3 setup...");
        }

        private void Start()
        {
            CheckAndSetupUI();
        }

        private void CheckAndSetupUI()
        {
            var panel = GameObject.Find("TouchFunctionPanel");

            if (panel == null)
            {
                Debug.LogWarning("[AutoSetup] TouchFunctionPanel not found in scene!");
                Debug.LogWarning("[AutoSetup] Run: Tools > Game > Setup Phase 3 (Complete)");
                return;
            }

            var listView = panel.GetComponent<TouchFunctionListView>();
            if (listView == null)
            {
                listView = panel.AddComponent<TouchFunctionListView>();
                Debug.Log("[AutoSetup] Added TouchFunctionListView");
            }

            var manager = panel.GetComponent<TouchFunctionListManager>();
            if (manager == null)
            {
                manager = panel.AddComponent<TouchFunctionListManager>();
                Debug.Log("[AutoSetup] Added TouchFunctionListManager");
            }

            Debug.Log("[AutoSetup] Phase 3 UI setup complete!");
        }
    }
}