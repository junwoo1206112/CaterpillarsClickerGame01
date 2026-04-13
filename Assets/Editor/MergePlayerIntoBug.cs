using UnityEngine;
using UnityEditor;
using ClickerGame.Gameplay;
using ClickerGame.Core;

namespace ClickerGame.Editor
{
    public class MergePlayerIntoBug
    {
        [MenuItem("Tools/Game/Merge Player into Bug")]
        public static void Merge()
        {
            var player = GameObject.Find("Player");
            var bug = GameObject.Find("Bug");

            if (player == null)
            {
                Debug.LogError("[MergePlayerIntoBug] Player 오브젝트를 찾을 수 없습니다.");
                return;
            }

            if (bug == null)
            {
                Debug.LogError("[MergePlayerIntoBug] Bug 오브젝트를 찾을 수 없습니다.");
                return;
            }

            var visual = bug.transform.Find("Visual");
            if (visual == null)
            {
                Debug.LogError("[MergePlayerIntoBug] Bug/Visual 오브젝트를 찾을 수 없습니다.");
                return;
            }

            var visualSpriteRenderer = visual.GetComponent<SpriteRenderer>();

            var playerClickHandler = player.GetComponent<ClickHandler>();
            var playerEvolution = player.GetComponent<CharacterEvolution>();
            var playerBoxCollider = player.GetComponent<BoxCollider>();
            var playerSphereCollider = player.GetComponent<SphereCollider>();

            CharacterEvolution bugEvolution = null;
            ClickHandler bugClickHandler = null;

            if (playerEvolution != null)
            {
                bugEvolution = bug.AddComponent<CharacterEvolution>();

                SerializedObject evSo = new SerializedObject(bugEvolution);
                evSo.Update();

                if (visualSpriteRenderer != null)
                {
                    evSo.FindProperty("_targetSpriteRenderer").objectReferenceValue = visualSpriteRenderer;
                    Debug.Log("[MergePlayerIntoBug] _targetSpriteRenderer → Visual 연결 완료");
                }

                evSo.FindProperty("_evolutionStages").arraySize = 0;

                CopyUnityEventPersistentListeners(
                    new SerializedObject(playerEvolution).FindProperty("OnEvolutionNameChanged"),
                    evSo.FindProperty("OnEvolutionNameChanged"));

                evSo.ApplyModifiedProperties();
            }

            if (playerClickHandler != null)
            {
                bugClickHandler = bug.AddComponent<ClickHandler>();
                SerializedObject clickSo = new SerializedObject(bugClickHandler);
                clickSo.Update();
                clickSo.FindProperty("characterName").stringValue = "Character";

                CopyUnityEventPersistentListeners(
                    new SerializedObject(playerClickHandler).FindProperty("OnClick"),
                    clickSo.FindProperty("OnClick"));
                CopyUnityEventPersistentListeners(
                    new SerializedObject(playerClickHandler).FindProperty("OnClickWithPosition"),
                    clickSo.FindProperty("OnClickWithPosition"));

                clickSo.ApplyModifiedProperties();
            }

            if (playerBoxCollider != null)
            {
                var bugBoxCollider = bug.AddComponent<BoxCollider>();
                bugBoxCollider.isTrigger = playerBoxCollider.isTrigger;
                bugBoxCollider.size = playerBoxCollider.size;
                bugBoxCollider.center = playerBoxCollider.center;
            }

            if (playerSphereCollider != null)
            {
                var bugSphereCollider = bug.AddComponent<SphereCollider>();
                bugSphereCollider.isTrigger = playerSphereCollider.isTrigger;
                bugSphereCollider.radius = playerSphereCollider.radius;
                bugSphereCollider.center = playerSphereCollider.center;
            }

            var gameplayManager = Object.FindFirstObjectByType<GameplayManager>();
            if (gameplayManager != null)
            {
                SerializedObject gso = new SerializedObject(gameplayManager);
                gso.Update();

                if (bugClickHandler != null)
                {
                    var clickHandlerProp = gso.FindProperty("_clickHandler");
                    if (clickHandlerProp != null)
                    {
                        clickHandlerProp.objectReferenceValue = bugClickHandler;
                        Debug.Log("[MergePlayerIntoBug] GameplayManager._clickHandler → Bug 업데이트 완료");
                    }
                }

                if (bugEvolution != null)
                {
                    var evolutionProp = gso.FindProperty("_evolution");
                    if (evolutionProp != null)
                    {
                        evolutionProp.objectReferenceValue = bugEvolution;
                        Debug.Log("[MergePlayerIntoBug] GameplayManager._evolution → Bug 업데이트 완료");
                    }
                }

                gso.ApplyModifiedProperties();
            }

            Undo.DestroyObjectImmediate(player);
            Debug.Log("[MergePlayerIntoBug] Player 오브젝트 삭제 완료");

            Debug.Log("[MergePlayerIntoBug] 완료! Bug 오브젝트 구조:");
            Debug.Log("  Bug (CharacterEvolution, ClickHandler, Animator, BoxCollider, SphereCollider)");
            Debug.Log("  └── Visual (SpriteRenderer)");
            Debug.Log("[MergePlayerIntoBug] 씬을 저장하세요 (Ctrl+S)");
        }

        private static void CopyUnityEventPersistentListeners(SerializedProperty srcEvents, SerializedProperty dstEvents)
        {
            if (srcEvents == null || dstEvents == null) return;

            var srcCalls = srcEvents.FindPropertyRelative("m_PersistentCalls.m_Calls");
            if (srcCalls == null) return;

            int count = srcCalls.arraySize;
            if (count == 0) return;

            dstEvents.arraySize = count;

            var dstCalls = dstEvents.FindPropertyRelative("m_PersistentCalls.m_Calls");
            dstCalls.arraySize = count;

            for (int i = 0; i < count; i++)
            {
                var srcCall = srcCalls.GetArrayElementAtIndex(i);
                var dstCall = dstCalls.GetArrayElementAtIndex(i);

                CopyPropertyValue(srcCall, dstCall, "m_Target");
                CopyPropertyValue(srcCall, dstCall, "m_MethodName");
                CopyPropertyValue(srcCall, dstCall, "m_Mode");
                CopyPropertyValue(srcCall, dstCall, "m_CallState");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_ObjectArgument");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_ObjectArgumentAssemblyTypeName");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_IntArgument");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_FloatArgument");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_StringArgument");
                CopyPropertyValue(srcCall, dstCall, "m_Arguments.m_BoolArgument");
            }
        }

        private static void CopyPropertyValue(SerializedProperty src, SerializedProperty dst, string propertyPath)
        {
            var srcProp = src.FindPropertyRelative(propertyPath);
            var dstProp = dst.FindPropertyRelative(propertyPath);
            if (srcProp == null || dstProp == null) return;

            switch (srcProp.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    dstProp.objectReferenceValue = srcProp.objectReferenceValue;
                    break;
                case SerializedPropertyType.String:
                    dstProp.stringValue = srcProp.stringValue;
                    break;
                case SerializedPropertyType.Integer:
                    dstProp.intValue = srcProp.intValue;
                    break;
                case SerializedPropertyType.Float:
                    dstProp.floatValue = srcProp.floatValue;
                    break;
                case SerializedPropertyType.Boolean:
                    dstProp.boolValue = srcProp.boolValue;
                    break;
                case SerializedPropertyType.Enum:
                    dstProp.enumValueIndex = srcProp.enumValueIndex;
                    break;
            }
        }
    }
}