using UnityEngine;
using UnityEditor;

namespace ClickerGame.EditorTools
{
    public class RemoveButtonFromPrefab
    {
        [MenuItem("Tools/Game/🔧 Remove Button from TouchFunctionListItem")]
        public static void RemoveRemoveButton()
        {
            string prefabPath = "Assets/Prefabs/TouchFunctionListItem.prefab";
            
            // Prefab 로드
            var prefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefabRoot == null)
            {
                Debug.LogError("[RemoveButton] Prefab not found!");
                return;
            }
            
            // RemoveButton 찾기
            var removeButton = prefabRoot.transform.Find("RemoveButton");
            
            if (removeButton != null)
            {
                // Prefab 모드에서 삭제
                GameObject.DestroyImmediate(removeButton.gameObject, true);
                Debug.Log("[RemoveButton] Removed RemoveButton from prefab");
            }
            else
            {
                Debug.Log("[RemoveButton] RemoveButton not found in prefab");
            }
            
            // 저장
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[RemoveButton] ✅ Prefab updated!");
            EditorUtility.DisplayDialog("완료", 
                "RemoveButton 이 삭제되었습니다!\n\n" +
                "이제 [+] 버튼만 표시됩니다.", 
                "OK");
        }
    }
}