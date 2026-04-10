## Context

**Background:**
SettingManager 는 현재 BGM/SFX 볼륨 슬라이더와 닫기 버튼만 제공합니다. 사용자가 볼륨을 조절할 때 실제 사운드를 들을 수 없어 적절한 볼륨 수준을 찾기 어렵습니다.

**Current State:**
```
SettingPanel UI Structure:
├── BGM Slider
├── SFX Slider
└── Close Button
```

**Constraints:**
- 기존 AudioManager 의 PlayBGM(), PlaySFX() 메서드 사용
- Unity UI Button 컴포넌트 사용
- 기존 SettingManager 로직과 호환성 유지

## Goals / Non-Goals

**Goals:**
- BGM 테스트 버튼 추가 (클릭 시 현재 BGM 재생)
- SFX 테스트 버튼 추가 (클릭 시 샘플 SFX 재생)
- 각 볼륨 슬라이더 아래에 배치하여 직관적인 UX 제공
- AudioManager 와 연동하여 현재 볼륨 설정으로 사운드 재생

**Non-Goals:**
- 새로운 사운드 시스템 추가
- 볼륨 조절 로직 변경
- UI 디자인/레이아웃 대폭 변경 (기존 레이아웃 유지)
- 사운드 프리뷰 기능 (사운드 목록에서 선택 등)

## Decisions

### Decision 1: 테스트 버튼 배치

**Decision:** 각 볼륨 슬라이더 바로 옆에 테스트 버튼 배치

```
SettingPanel UI Structure (New):
├── BGM Slider ──────── [🔊 Test]
├── SFX Slider ──────── [🔊 Test]
└── Close Button
```

**Rationale:**
- 사용자가 볼륨을 조절하면서 즉시 테스트 가능
- 직관적인 UX 제공
- 기존 레이아웃을 최소한으로 변경

**Alternatives Considered:**
1. ❌ 별도 사운드 테스트 패널
   - 복잡도 증가
   - 화면 공간 낭비

2. ❌ 자동 재생 (볼륨 조절 시)
   - 지나치게 빈번한 사운드 재생
   - 사용자 경험 저하

### Decision 2: SFX 테스트 사운드

**Decision:** AudioManager 에 기본 SFX 클립을 할당하여 테스트용으로 사용

**Rationale:**
- 별도의 SFX 샘플 파일 불필요
- 기존 AudioManager 인프라 활용
- 프로젝트에서 실제로 사용하는 사운드로 테스트

### Decision 3: 버튼 이벤트 처리

**Decision:** SettingManager 에 퍼블릭 메서드로 구현 (Unity Inspector 에서 연결)

```csharp
public void OnTestBGMClicked()
public void OnTestSFXClicked()
```

**Rationale:**
- Unity Editor 에서 쉽게 연결 가능
- 기존 버튼 이벤트 패턴과 일치
- 테스트 및 디버깅 용이

## Risks / Trade-offs

### Risk 1: BGM 중복 재생

**Risk:** BGM 테스트 버튼을 여러 번 클릭하면 BGM 이 겹쳐서 재생됨

**Mitigation:** 
- PlayBGM() 호출 전 기존 BGM 정지
- 또는 isPlaying 체크 후 재생

### Risk 2: SFX 클립 미설정

**Risk:** AudioManager 의 SFX 클립이 설정되지 않아면 테스트 실패

**Mitigation:**
- null 체크 추가
- 경고 로그 출력
