using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ClickerGame.UI;

namespace ClickerGame.EditorTools
{
    public class CanvasSetup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Canvas UI")]
        public static void SetupCanvasUI()
        {
            // Canvas 찾기
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("[CanvasSetup] Canvas not found!");
                return;
            }

            Debug.Log("[CanvasSetup] Canvas found!");

            // GameUI 컴포넌트 추가
            GameUI gameUI = canvas.GetComponent<GameUI>();
            if (gameUI == null)
            {
                gameUI = canvas.gameObject.AddComponent<GameUI>();
                Debug.Log("[CanvasSetup] GameUI component added to Canvas");
            }
            else
            {
                Debug.Log("[CanvasSetup] GameUI already exists on Canvas");
            }

            // UI 텍스트들 찾기
            Text scoreText = FindChildText(canvas.transform, "ScoreText");
            Text touchCountText = FindChildText(canvas.transform, "TouchCountText");
            Text stageText = FindChildText(canvas.transform, "StageText");
            Text multiplierText = FindChildText(canvas.transform, "MultiplierText");
            Text messageText = FindChildText(canvas.transform, "MessageText");

            // GameUI 에 연결
            SerializedObject serializedObject = new SerializedObject(gameUI);
            
            if (scoreText != null)
            {
                serializedObject.FindProperty("scoreText").objectReferenceValue = scoreText;
                Debug.Log($"[CanvasSetup] ScoreText connected: {scoreText.name}");
            }
            
            if (touchCountText != null)
            {
                serializedObject.FindProperty("touchCountText").objectReferenceValue = touchCountText;
                Debug.Log($"[CanvasSetup] TouchCountText connected: {touchCountText.name}");
            }
            
            if (stageText != null)
            {
                serializedObject.FindProperty("stageText").objectReferenceValue = stageText;
                Debug.Log($"[CanvasSetup] StageText connected: {stageText.name}");
            }
            
            if (multiplierText != null)
            {
                serializedObject.FindProperty("multiplierText").objectReferenceValue = multiplierText;
                Debug.Log($"[CanvasSetup] MultiplierText connected: {multiplierText.name}");
            }
            
            if (messageText != null)
            {
                serializedObject.FindProperty("messageText").objectReferenceValue = messageText;
                Debug.Log($"[CanvasSetup] MessageText connected: {messageText.name}");
            }

            serializedObject.ApplyModifiedProperties();
            Debug.Log("[CanvasSetup] Canvas UI setup complete!");
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
