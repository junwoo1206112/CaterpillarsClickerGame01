# Design: Fix Phase 3 Prefab References

## Overview
TouchFunctionListItem.prefab의 UI 요소 이름을 코드의 SerializedProperty 이름과 일치시키고, 누락된 요소를 추가합니다.

## Technical Approach

### Option A: Recreate Prefab (Recommended)
기존 Prefab을 삭제하고 Phase3Setup.CreatePrefab()으로 재생성.

**장점:**
- 간단하고 확실한 해결
- 코드가 이미 올바르게 작성되어 있음
- SerializedObject를 통한 참조 설정 자동화

**단점:**
- 수동으로 Prefab에 적용한 커스터마이징 손실

### Option B: Manual Fix
Unity Editor에서 수동으로 Prefab 수정.

**장점:**
- 기존 구조 유지

**단점:**
- 누락된 요소(CostText, PointsText) 직접 추가 필요
- 참조 수동 연결 필요
- 재현 가능성 낮음

## Selected Approach: Option A

### Implementation Details

1. **Delete existing Prefab**
   - `Assets/Prefabs/TouchFunctionListItem.prefab` 삭제

2. **Run CreatePrefab()**
   - Unity Editor 메뉴: `Tools > Game > Create TouchFunctionListItem Prefab`
   - 또는 `Phase3Setup.SetupPhase3Complete()` 실행

3. **Verify References**
   - SerializedObject를 통해 자동으로 참조 설정
   - 각 UI 요소가 올바르게 연결되었는지 확인

### Expected Prefab Structure
```
TouchFunctionListItem (GameObject)
├── NameText (Text)           → nameText
├── DescriptionText (Text)   → descriptionText
├── CostText (Text)           → costText
├── LevelText (Text)          → levelText
├── PointsText (Text)         → pointsText
├── AddButton (Button)       → addButton
│   └── Text (Text)
└── RemoveButton (Button)     → removeButton
    └── Text (Text)
```

### Code Reference (Phase3Setup.cs:60-68)
```csharp
var so = new SerializedObject(listItem);
so.FindProperty("nameText").objectReferenceValue = itemObj.transform.Find("NameText").GetComponent<Text>();
so.FindProperty("descriptionText").objectReferenceValue = itemObj.transform.Find("DescriptionText").GetComponent<Text>();
so.FindProperty("costText").objectReferenceValue = itemObj.transform.Find("CostText").GetComponent<Text>();
so.FindProperty("levelText").objectReferenceValue = itemObj.transform.Find("LevelText").GetComponent<Text>();
so.FindProperty("pointsText").objectReferenceValue = itemObj.transform.Find("PointsText").GetComponent<Text>();
so.FindProperty("addButton").objectReferenceValue = itemObj.transform.Find("AddButton").GetComponent<Button>();
so.FindProperty("removeButton").objectReferenceValue = itemObj.transform.Find("RemoveButton").GetComponent<Button>();
so.ApplyModifiedProperties();
```

## Verification Steps
1. Unity Editor에서 Prefab 선택
2. Inspector에서 TouchFunctionListItem 컴포넌트 확인
3. 모든 UI References 필드에 올바른 참조 표시되는지 확인
4. Play Mode에서 UI 정상 작동 확인

## Risks
- 없음. Prefab은 버전 관리에서 추적 가능.