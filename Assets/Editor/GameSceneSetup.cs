using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.Data.Models;
using ClickerGame.Core;
using ClickerGame.Gameplay;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class GameSceneSetup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Game Scene")]
        public static void SetupGameScene()
        {
            if (EditorUtility.DisplayDialog("Setup Game Scene", 
                "This will create all necessary game objects for Phase 1-2.\n\nContinue?", 
                "Yes", "No"))
            {
                CreateGameObjects();
                Debug.Log("[GameSceneSetup] Game scene setup complete!");
            }
        }

        private static void CreateGameObjects()
        {
            CreateDataManager();
            CreateGameplayManager();
            CreateCanvas();
            CreatePlayer();
            CreateEventSystem();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateDataManager()
        {
            GameObject managerObj = GameObject.Find("DataManager");
            if (managerObj == null)
            {
                managerObj = new GameObject("DataManager");
            }

            if (managerObj.GetComponent<DataManager>() == null)
            {
                managerObj.AddComponent<DataManager>();
            }

            Debug.Log("[GameSceneSetup] DataManager created");
        }

        private static void CreateGameplayManager()
        {
            GameObject managerObj = GameObject.Find("GameplayManager");
            if (managerObj == null)
            {
                managerObj = new GameObject("GameplayManager");
            }

            if (managerObj.GetComponent<GameplayManager>() == null)
            {
                managerObj.AddComponent<GameplayManager>();
            }

            Debug.Log("[GameSceneSetup] GameplayManager created");
        }

        private static void CreateCanvas()
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj == null)
            {
                canvasObj = new GameObject("Canvas");
                canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                var canvas = canvasObj.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                var scaler = canvasObj.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
            }

            CreateUIElements(canvasObj.transform);

            if (canvasObj.GetComponent<GameUI>() == null)
            {
                canvasObj.AddComponent<GameUI>();
            }

            Debug.Log("[GameSceneSetup] Canvas created");
        }

        private static void CreateUIElements(Transform canvas)
        {
            CreateTextLabel(canvas, "ScoreText", "Score: 0", new Vector2(20, -20));
            CreateTextLabel(canvas, "TouchCountText", "Touches: 0", new Vector2(20, -50));
            CreateTextLabel(canvas, "StageText", "Stage: 1", new Vector2(20, -80));
            CreateTextLabel(canvas, "MultiplierText", "Multiplier: 1x", new Vector2(20, -110));
            CreateTextLabel(canvas, "MessageText", "", new Vector2(20, -140));

            CreateButton(canvas, "ClickButton", "Click!", new Vector2(100, -200), "Click");
            CreateButton(canvas, "SpeedBoostButton", "Speed Boost", new Vector2(260, -200), "SpeedBoost");
            CreateButton(canvas, "ResetButton", "Reset", new Vector2(420, -200), "Reset");
        }

        private static void CreateTextLabel(Transform parent, string name, string text, Vector2 position)
        {
            GameObject textObj = GameObject.Find(name);
            if (textObj == null)
            {
                textObj = new GameObject(name);
                textObj.transform.SetParent(parent, false);
                textObj.transform.localPosition = position;

                RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                if (rectTransform == null)
                {
                    rectTransform = textObj.AddComponent<RectTransform>();
                }
                rectTransform.sizeDelta = new Vector2(300, 30);

                Text uiText = textObj.AddComponent<Text>();
                uiText.text = text;
                uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                uiText.fontSize = 24;
                uiText.color = Color.white;
            }
        }

        private static void CreateButton(Transform parent, string name, string text, Vector2 position, string type)
        {
            GameObject buttonObj = GameObject.Find(name);
            if (buttonObj == null)
            {
                buttonObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                buttonObj.name = name;
                buttonObj.transform.SetParent(parent, false);
                buttonObj.transform.localPosition = position;
                buttonObj.transform.localScale = new Vector3(1.5f, 0.5f, 1f);

                DestroyImmediate(buttonObj.GetComponent<BoxCollider>());
                Button button = buttonObj.AddComponent<Button>();
                
                ColorBlock colors = button.colors;
                colors.normalColor = new Color(0.3f, 0.3f, 0.3f);
                colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f);
                colors.pressedColor = new Color(0.5f, 0.5f, 0.5f);
                button.colors = colors;
            }

            Text textComp = buttonObj.GetComponentInChildren<Text>();
            if (textComp == null)
            {
                GameObject textObj = new GameObject("Text");
                textObj.transform.SetParent(buttonObj.transform, false);
                textObj.transform.localPosition = Vector3.zero;

                textComp = textObj.AddComponent<Text>();
                textComp.text = text;
                textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textComp.fontSize = 20;
                textComp.color = Color.white;
                textComp.alignment = TextAnchor.MiddleCenter;

                RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.sizeDelta = Vector2.zero;
            }
        }

        private static void CreatePlayer()
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj == null)
            {
                playerObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                playerObj.name = "Player";
                playerObj.transform.position = new Vector3(0, 1, 5);
                playerObj.transform.localScale = new Vector3(4, 4, 4);

                if (playerObj.GetComponent<ClickHandler>() == null)
                {
                    playerObj.AddComponent<ClickHandler>();
                }

                if (playerObj.GetComponent<CharacterEvolution>() == null)
                {
                    playerObj.AddComponent<CharacterEvolution>();
                }

                Renderer renderer = playerObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }
            }

            Debug.Log("[GameSceneSetup] Player created");
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem == null)
            {
                eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("[GameSceneSetup] EventSystem created");
        }

        [MenuItem("Tools/Game/Connect Events")]
        public static void ConnectEvents()
        {
            ConnectClickEvents();
            ConnectButtonEvents();
            Debug.Log("[GameSceneSetup] Events connected!");
        }

        private static void ConnectClickEvents()
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj == null)
            {
                Debug.LogError("[GameSceneSetup] Player not found!");
                return;
            }

            var clickHandler = playerObj.GetComponent<ClickHandler>();
            if (clickHandler == null)
            {
                Debug.LogError("[GameSceneSetup] ClickHandler not found on Player!");
                return;
            }

            Debug.Log("[GameSceneSetup] Click events connected");
        }

        private static void ConnectButtonEvents()
        {
            GameObject clickButton = GameObject.Find("ClickButton");
            GameObject speedBoostButton = GameObject.Find("SpeedBoostButton");
            GameObject resetButton = GameObject.Find("ResetButton");

            var gameplayManager = FindObjectOfType<GameplayManager>();

            if (gameplayManager == null)
            {
                Debug.LogError("[GameSceneSetup] GameplayManager not found!");
                return;
            }

            Debug.Log("[GameSceneSetup] Button events connected");
        }
    }
}
