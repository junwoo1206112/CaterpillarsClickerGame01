using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.Core;
using ClickerGame.Gameplay;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class Phase2Setup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Phase 2")]
        public static void SetupPhase2()
        {
            Debug.Log("[Phase2Setup] Starting Phase 2 setup...");

            CreateManagers();
            CreatePlayer();
            CreateCanvas();
            CreateEventSystem();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[Phase2Setup] Phase 2 setup complete!");
        }

        private static void CreateManagers()
        {
            CreateOrAddComponent<DataManager>("DataManager");
            CreateOrAddComponent<GameplayManager>("GameplayManager");
            CreateOrAddComponent<TouchCounter>("TouchCounter");
            CreateOrAddComponent<TouchFunctionManager>("TouchFunctionManager");

            Debug.Log("[Phase2Setup] All managers created");
        }

        private static void CreatePlayer()
        {
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                player.name = "Player";
                player.transform.position = new Vector3(0, 0, 5);
                player.transform.localScale = new Vector3(4, 4, 4);

                player.AddComponent<ClickHandler>();
                player.AddComponent<CharacterEvolution>();

                BoxCollider collider = player.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    collider = player.AddComponent<BoxCollider>();
                }
                collider.isTrigger = true;

                Renderer renderer = player.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }

                Debug.Log("[Phase2Setup] Player created");
            }
            else
            {
                EnsureComponent<ClickHandler>(player);
                EnsureComponent<CharacterEvolution>(player);
                Debug.Log("[Phase2Setup] Player exists, components ensured");
            }
        }

        private static void CreateCanvas()
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                var scaler = canvasObj.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                Debug.Log("[Phase2Setup] Canvas created");
            }

            // Add GameUI component
            GameUI gameUI = canvas.GetComponent<GameUI>();
            if (gameUI == null)
            {
                gameUI = canvas.gameObject.AddComponent<GameUI>();
                Debug.Log("[Phase2Setup] GameUI component added");
            }

            // Connect UI references
            ConnectGameUIReferences(canvas, gameUI);

            // Create basic UI texts
            CreateTextElement(canvas.transform, "TouchCountText", "Touches: 0", new Vector2(0, 100), Color.white);
            CreateTextElement(canvas.transform, "FormText", "Form: 1", new Vector2(0, 50), Color.white);
            CreateTextElement(canvas.transform, "MultiplierText", "Bonus: Ready!", new Vector2(0, 0), Color.yellow);
            CreateTextElement(canvas.transform, "ClickScoreText", "+1", new Vector2(0, -50), Color.green);
        }

        private static void CreateTextElement(Transform parent, string name, string text, Vector2 position, Color color)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);

            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(300, 50);
            rectTransform.localPosition = position;

            Text uiText = textObj.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            uiText.fontSize = 36;
            uiText.color = color;
            uiText.alignment = TextAnchor.MiddleCenter;

            Debug.Log($"[Phase2Setup] Created {name}");
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem == null)
            {
                eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("[Phase2Setup] EventSystem created");
            }
            else
            {
                Debug.Log("[Phase2Setup] EventSystem exists");
            }
        }

        private static GameObject CreateOrAddComponent<T>(string name) where T : Component
        {
            GameObject obj = GameObject.Find(name);
            if (obj == null)
            {
                obj = new GameObject(name);
                obj.AddComponent<T>();
                Debug.Log($"[Phase2Setup] {name} created");
            }
            else
            {
                EnsureComponent<T>(obj);
                Debug.Log($"[Phase2Setup] {name} exists");
            }
            return obj;
        }

        private static void EnsureComponent<T>(GameObject obj) where T : Component
        {
            if (obj.GetComponent<T>() == null)
            {
                obj.AddComponent<T>();
            }
        }

        private static void ConnectGameUIReferences(Canvas canvas, GameUI gameUI)
        {
            SerializedObject so = new SerializedObject(gameUI);

            Text touchCountText = FindChildText(canvas.transform, "TouchCountText");
            Text stageText = FindChildText(canvas.transform, "FormText");
            Text multiplierText = FindChildText(canvas.transform, "MultiplierText");
            Text clickScoreText = FindChildText(canvas.transform, "ClickScoreText");

            if (touchCountText != null)
                so.FindProperty("touchCountText").objectReferenceValue = touchCountText;
            if (stageText != null)
                so.FindProperty("stageText").objectReferenceValue = stageText;
            if (multiplierText != null)
                so.FindProperty("multiplierText").objectReferenceValue = multiplierText;
            if (clickScoreText != null)
                so.FindProperty("clickScoreText").objectReferenceValue = clickScoreText;

            so.ApplyModifiedProperties();

            Debug.Log("[Phase2Setup] GameUI references connected");
        }

        private static Text FindChildText(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null)
            {
                return child.GetComponent<Text>();
            }
            return null;
        }
    }
}
