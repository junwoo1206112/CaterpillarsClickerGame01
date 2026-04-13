using UnityEngine;
using UnityEditor;
using ClickerGame.Core;
using ClickerGame.Gameplay;

namespace ClickerGame.EditorTools
{
    public class Phase12Setup : EditorWindow
    {
        [MenuItem("Tools/Game/Setup Phase 1-2 (Canvas 제외)")]
        public static void SetupPhase12()
        {
            if (EditorUtility.DisplayDialog("Setup Phase 1-2",
                "Phase 1-2 의 모든 매니저를 생성합니다.\n\n계속하시겠습니까?",
                "예", "아니오"))
            {
                CreatePhase12Objects();
                Debug.Log("[Phase12Setup] Phase 1-2 설정 완료!");
            }
        }

        private static void CreatePhase12Objects()
        {
            CreateDataManager();
            CreateGameplayManager();
            CreateScoreManager();
            CreateTouchCounter();
            CreateTouchFunctionManager();
            CreateItemManager();
            CreatePlayer();
            CreateEventSystem();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateDataManager()
        {
            GameObject obj = GameObject.Find("DataManager");
            if (obj == null)
            {
                obj = new GameObject("DataManager");
                obj.AddComponent<DataManager>();
                Debug.Log("[Phase12Setup] DataManager 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] DataManager 이미 존재함");
            }
        }

        private static void CreateGameplayManager()
        {
            GameObject obj = GameObject.Find("GameplayManager");
            if (obj == null)
            {
                obj = new GameObject("GameplayManager");
                obj.AddComponent<GameplayManager>();
                Debug.Log("[Phase12Setup] GameplayManager 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] GameplayManager 이미 존재함");
            }
        }

        private static void CreateScoreManager()
        {
            GameObject obj = GameObject.Find("ScoreManager");
            if (obj == null)
            {
                obj = new GameObject("ScoreManager");
                obj.AddComponent<ScoreManager>();
                Debug.Log("[Phase12Setup] ScoreManager 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] ScoreManager 이미 존재함");
            }
        }

        private static void CreateTouchCounter()
        {
            GameObject obj = GameObject.Find("TouchCounter");
            if (obj == null)
            {
                obj = new GameObject("TouchCounter");
                obj.AddComponent<TouchCounter>();
                Debug.Log("[Phase12Setup] TouchCounter 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] TouchCounter 이미 존재함");
            }
        }

        private static void CreateTouchFunctionManager()
        {
            GameObject obj = GameObject.Find("TouchFunctionManager");
            if (obj == null)
            {
                obj = new GameObject("TouchFunctionManager");
                obj.AddComponent<TouchFunctionManager>();
                Debug.Log("[Phase12Setup] TouchFunctionManager 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] TouchFunctionManager 이미 존재함");
            }
        }

        private static void CreateItemManager()
        {
            GameObject obj = GameObject.Find("ItemManager");
            if (obj == null)
            {
                obj = new GameObject("ItemManager");
                obj.AddComponent<ItemManager>();
                Debug.Log("[Phase12Setup] ItemManager 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] ItemManager 이미 존재함");
            }
        }

        private static void CreatePlayer()
        {
            GameObject obj = GameObject.Find("Player");
            if (obj == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.name = "Player";
                obj.transform.position = new Vector3(0, 0, 5);
                obj.transform.localScale = new Vector3(4, 4, 4);

                obj.AddComponent<ClickHandler>();
                obj.AddComponent<CharacterEvolution>();

                BoxCollider collider = obj.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    collider = obj.AddComponent<BoxCollider>();
                }
                collider.isTrigger = true;

                // MeshRenderer 색상 초록색으로
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }

                Debug.Log("[Phase12Setup] Player 생성됨 (초록색 구체)");
            }
            else
            {
                Debug.Log("[Phase12Setup] Player 이미 존재함");
            }
        }

        private static void CreateEventSystem()
        {
            GameObject obj = GameObject.Find("EventSystem");
            if (obj == null)
            {
                obj = new GameObject("EventSystem");
                obj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                obj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("[Phase12Setup] EventSystem 생성됨");
            }
            else
            {
                Debug.Log("[Phase12Setup] EventSystem 이미 존재함");
            }
        }
    }
}
