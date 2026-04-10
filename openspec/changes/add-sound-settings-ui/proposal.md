## Why

현재 세팅 창에는 BGM/SFX 볼륨 슬라이더만 존재하며, 사용자가 사운드 효과를 직접 듣고 테스트할 수 있는 기능이 없습니다. 사용자가 볼륨을 조절하면서 즉각적으로 사운드 피드백을 받을 수 있도록 사운드 테스트 UI 를 추가해야 합니다.

## What Changes

- 세팅 창에 사운드 테스트 버튼 추가 (BGM 테스트, SFX 테스트)
- 각 버튼 클릭 시 해당 사운드 재생
- AudioManager 와 연동하여 현재 볼륨 설정으로 사운드 재생
- UI 레이아웃은 기존 Slider 아래에 버튼 추가

## Capabilities

### New Capabilities
- `sound-test-buttons-ui`: 세팅 창에서 BGM/SFX 사운드를 테스트할 수 있는 버튼 UI

### Modified Capabilities
- `setting-manager`: 사운드 테스트 버튼 이벤트 핸들러 추가

## Impact

**Affected Files:**
- `Assets/Scripts/UI/SettingManager.cs` (사운드 테스트 버튼 로직 추가)
- `Assets/Prefabs/SettingPanel.prefab` (버튼 UI 추가 필요)

**Dependencies:**
- AudioManager 의 PlayBGM(), PlaySFX() 메서드 사용
- 기존 SettingManager 의 볼륨 설정과 연동

**Breaking Changes:**
- 없음 (기존 기능과 호환)
