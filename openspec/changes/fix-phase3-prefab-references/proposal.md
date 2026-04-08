## Why

TouchFunctionListItem 프리팹의 UI 참조가 모두 null로 설정되어 있어, 게임 실행 시 터치 강화 목록 UI가 정상 작동하지 않습니다. 프리팹 내부 텍스트 요소 이름이 코드에서 참조하는 이름과 일치하지 않고, 일부 UI 요소(CostText, PointsText)가 프리팹에 누락되어 있습니다. Phase 3 완료를 위해 이 문제를 즉시 해결해야 합니다.

## What Changes

- TouchFunctionListItem 프리팹의 UI 요소 이름을 코드와 일치시킴
- 누락된 CostText, PointsText UI 요소 추가
- 모든 UI 참조를 SerializedObject로 자동 연결
- Phase3Setup.cs의 CreatePrefab() 메서드 개선
- 프리팹 재생성 Editor 메뉴 제공

## Capabilities

### New Capabilities
- `touch-function-list-item-ui`: 터치 강화 리스트 아이템 UI 컴포넌트의 완전한 프리팹 구조

### Modified Capabilities
- `touch-function-list-view`: 리스트 뷰가 올바르게 연결된 프리팹을 인스턴스화하도록 수정

## Impact

**Affected Files:**
- `Assets/Prefabs/TouchFunctionListItem.prefab` (재생성)
- `Assets/Editor/Phase3Setup.cs` (UI 요소 이름 수정)
- `Assets/Scripts/UI/TouchFunctionListItem.cs` (UI 참조 확인)

**Dependencies:**
- Phase 3 터치 강화 시스템이 정상 작동하려면 이 수정이 필수
- TouchFunctionListView가 프리팹을 올바르게 인스턴스화 가능

**Breaking Changes:**
- 기존 Scene에 배치된 UpgradeListItem 오브젝트는 더 이상 작동하지 않음 (새 프리팹 사용 권장)