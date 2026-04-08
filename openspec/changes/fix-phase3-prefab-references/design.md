## Context

**Background:**
TouchFunctionListItem 프리팹은 Phase 3 터치 강화 시스템의 핵심 UI 컴포넌트입니다. 현재 프리팹의 UI 요소 이름이 코드와 일치하지 않아 모든 참조가 null로 설정되어 있습니다.

**Current State:**
```
Prefab Structure (현재):
├── FunctionName (Text)      ← 코드는 NameText 찾음
├── Description (Text)       ← 코드는 DescriptionText 찾음
├── Level (Text)             ← 코드는 LevelText 찾음
├── AddButton (Button)
├── RemoveButton (Button)
└── [CostText 없음]         ← 코드는 CostText 찾음
└── [PointsText 없음]        ← 코드는 PointsText 찾음
```

**Constraints:**
- Unity Editor에서 자동 생성 방식 사용 (SerializedObject)
- 기존 Scene의 UpgradeListItem 오브젝트와 호환성 유지 불필요 (삭제 예정)
- Phase3Setup.cs Editor 스크립트 활용

## Goals / Non-Goals

**Goals:**
- TouchFunctionListItem 프리팹의 모든 UI 참조가 정상 연결됨
- CostText, PointsText UI 요소가 프리팹에 포함됨
- Phase3Setup.CreatePrefab() 실행 시 자동으로 UI 참조 설정
- 프리팹 재생성 후 기존 Scene의 TouchFunctionPanel과 호환됨

**Non-Goals:**
- UI 디자인/레이아웃 변경 (위치, 크기, 색상 등)
- TouchFunctionListItem.cs의 로직 변경
- 새로운 UI 기능 추가 (레벨업, 업그레이드 등)

## Decisions

### Decision 1: 프리팹 재생성 방식 선택

**Decision:** 기존 프리팹 삭제 후 Phase3Setup.CreatePrefab()으로 재생성

**Rationale:**
- 수동으로 프리팹 편집하는 것보다 자동화된 코드로 생성하는 것이 일관성 보장
- SerializedObject를 사용하면 private 필드도 자동 설정 가능
- 향후 프리팹 재생성이 필요할 때 동일한 메서드 사용 가능

**Alternatives Considered:**
1. ❌ 수동으로 프리팹 편집
   - 일관성 보장 어려움
   - 재현 불가능
   - 휴먼 에러 가능성

2. ❌ 프리팹 Variant 사용
   - 복잡도 증가
   - 현재 규모에 과도한 솔루션

### Decision 2: UI 요소 이름 규칙

**Decision:** 코드의 필드명과 일치하는 이름 사용

```
Prefab UI Elements:
├── NameText (Text)
├── DescriptionText (Text)
├── CostText (Text)
├── LevelText (Text)
├── PointsText (Text)
├── AddButton (Button)
└── RemoveButton (Button)
```

**Rationale:**
- Phase3Setup.cs의 CreateText() 메서드에서 설정한 이름과 일치
- TouchFunctionListItem.cs의 SerializedProperty 경로와 일치
- 명확하고 일관된 이름 규칙

### Decision 3: Editor 메뉴 유지

**Decision:** 기존 `Tools > Game > Setup Phase 3 (Complete)` 메뉴 유지

**Rationale:**
- 이미 구현되어 있음
- 사용자가 쉽게 접근 가능
- 1-클릭으로 모든 설정 완료

## Risks / Trade-offs

### Risk 1: 기존 Scene의 UpgradeListItem 오브젝트 무효화

**Risk:** Scene에 수동으로 배치한 UpgradeListItem이 새 프리팹과 호환되지 않음

**Mitigation:** Phase3Setup.ConnectSceneUI()에서 자동으로 삭제하고 새 프리팹으로 교체

### Risk 2: 프리팹 재생성 중 파일 잠금

**Risk:** Unity가 프리팹 파일을 잠근 상태에서 재생성 시도 시 실패

**Mitigation:**
- AssetDatabase.DeleteAsset()로 기존 프리팹 삭제
- AssetDatabase.Refresh()로 캐시 갱신
- PrefabUtility.SaveAsPrefabAsset()로 새 프리팹 저장

### Risk 3: Scene 연결 끊김

**Risk:** 기존 TouchFunctionPanel의 itemPrefab 참조가 끊김

**Mitigation:** Phase3Setup.ConnectSceneUI()에서 SerializedObject로 자동 재연결