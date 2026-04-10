## ADDED Requirements

### Requirement: BGM 테스트 버튼
사용자는 세팅 창에서 BGM 테스트 버튼을 클릭하여 현재 볼륨 설정으로 BGM 을 재생할 수 있어야 합니다.

#### Scenario: BGM 테스트 버튼 클릭
- **WHEN** 사용자가 BGM 테스트 버튼을 클릭
- **THEN** 현재 BGM 볼륨 설정으로 BGM 이 1 회 재생됨

#### Scenario: BGM 테스트 중 볼륨 변경
- **WHEN** BGM 재생 중에 사용자가 BGM 슬라이더를 조절
- **THEN** 재생 중인 BGM 의 볼륨이 실시간으로 변경됨

### Requirement: SFX 테스트 버튼
사용자는 세팅 창에서 SFX 테스트 버튼을 클릭하여 현재 볼륨 설정으로 SFX 를 재생할 수 있어야 합니다.

#### Scenario: SFX 테스트 버튼 클릭
- **WHEN** 사용자가 SFX 테스트 버튼을 클릭
- **THEN** 현재 SFX 볼륨 설정으로 SFX 가 1 회 재생됨

#### Scenario: 연속 SFX 테스트
- **WHEN** 사용자가 SFX 테스트 버튼을 여러 번 연속 클릭
- **THEN** 각 클릭마다 SFX 가 독립적으로 재생됨

### Requirement: 사운드 테스트 UI 레이아웃
사운드 테스트 버튼은 각 볼륨 슬라이더와 직관적으로 연결되어야 합니다.

#### Scenario: UI 배치
- **WHEN** 세팅 창이 열림
- **THEN** BGM 테스트 버튼은 BGM 슬라이더 옆에, SFX 테스트 버튼은 SFX 슬라이더 옆에 배치됨
