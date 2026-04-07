# 🎮 Phase 2: Gameplay 시스템 테스트 가이드

## 📋 테스트 목표
1. 캐릭터 클릭 → 점수 획득
2. 점수에 따른 진화 (애벌레 → 번데기 → 나비)
3. 터치 카운트 및 보너스 기능
4. 스피드 부스터 (2 배속)
5. 아이템 사용

---

## 🎯 테스트 단계

### 단계 1: Unity 컴파일
```
1. Unity Editor 실행
2. Console 에서 컴파일 완료 대기
3. 빨간색 에러 확인 (없어야 함)
```

### 단계 2: 테스트 씬 실행
```
1. Assets/Scenes/Phase2Test.unity 더블클릭
2. Play 버튼 클릭
3. 테스트 UI 표시 확인
```

### 단계 3: 기본 클릭 테스트
```
1. 화면 중앙의 구체 (캐릭터) 클릭
2. 또는 "Click!" 버튼 클릭
3. Score 증가 확인
4. Console 에 "Clicked!" 로그 확인
```

### 단계 4: 진화 테스트
```
1. 100 회 이상 클릭 (점수 100 이상)
2. Stage 가 2 로 변경되는지 확인
3. 500 회 이상 클릭 (점수 500 이상)
4. Stage 가 3 으로 변경되는지 확인
```

### 단계 5: 터치 보너스 테스트
```
1. 50 회 이상 클릭
2. Console 에 "Bonus Enabled!" 로그 확인
3. 그 후 3 회 클릭마다 "Bonus Click Activated!" 확인
4. Score 가 더 빠르게 오르는지 확인
```

### 단계 6: 스피드 부스터 테스트
```
1. "Speed Boost (2x)" 버튼 클릭
2. Multiplier 가 2x 로 변경되는지 확인
3. 텍스트 색상이 노란색으로 변경
4. 60 초 후 자동으로 해제되는지 확인
```

### 단계 7: 리셋 테스트
```
1. "Reset" 버튼 클릭
2. Score 가 0 으로 리셋
3. Stage 가 1 로 리셋
4. Touch Count 가 0 으로 리셋
```

---

## ✅ 성공 기준

| 테스트 | 기준 | 결과 |
|--------|------|------|
| 클릭 | 클릭 시 Score +1 | ⬜ |
| 진화 | 100 점/500 점에 Stage 변화 | ⬜ |
| 터치 보너스 | 50 회 후 3 회마다 추가 | ⬜ |
| 스피드 부스터 | 2 배속 60 초 | ⬜ |
| UI 표시 | Score, Stage, Multiplier 표시 | ⬜ |
| Console 로그 | 정상 동작 로그 | ⬜ |

---

## 🎮 테스트 씬 구성

### 생성되는 오브젝트
```
Hierarchy:
├── GameplayManager (Singleton)
│   ├── ClickHandler
│   ├── ScoreManager
│   ├── CharacterEvolution
│   ├── TouchCounter
│   ├── TouchFunctionManager
│   └── ItemManager
├── TestCharacter (구체)
│   ├── ClickHandler
│   └── CharacterEvolution
├── Canvas (UI)
│   ├── Status Text
│   ├── Score Text
│   ├── Touch Count Text
│   ├── Stage Text
│   ├── Multiplier Text
│   ├── Click Button
│   ├── Speed Boost Button
│   ├── Reset Button
│   └── Log Text
└── EventSystem
```

---

## 📝 생성된 스크립트

### Gameplay (8 개)
```
Gameplay/
├── ClickHandler.cs - 클릭 입력 처리
├── ScoreManager.cs - 점수 관리
├── CharacterEvolution.cs - 진화 시스템
├── TouchCounter.cs - 터치 카운트
├── ItemManager.cs - 아이템 관리
└── TouchFunction/
    ├── ITouchFunction.cs
    ├── BonusTouchFunction.cs
    ├── SpeedBoostFunction.cs
    └── TouchFunctionManager.cs

Managers/
└── GameplayManager.cs - 통합 관리자
```

### Tests (1 개)
```
Tests/
└── Phase2Test.cs - 테스트 UI 및 로직
```

---

## 🐛 문제 해결

### Q1. 클릭이 안 됨
```
해결: TestCharacter 의 BoxCollider 가 Trigger 로 되어 있는지 확인
```

### Q2. 진화가 안 됨
```
해결: 
1. ScoreManager 와 CharacterEvolution 연결 확인
2. CharacterData 가 할당되었는지 확인
```

### Q3. UI 가 안 보임
```
해결: Canvas 의 Render Mode 가 ScreenSpaceOverlay 인지 확인
```

### Q4. Console 에 에러
```
해결: 
1. 에러 메시지 확인
2. 모든 스크립트가 컴파일되었는지 확인
3. Assets > Reimport All 실행
```

---

## 🎯 게임 데이터 설정

### CharacterData 입력 (Excel)
```
CharacterData 시트:
| ID       | Name      | Stage | ScoreRequired | SpritePath | Color   |
|----------|-----------|-------|---------------|------------|---------|
| CHAR_001 | 애벌레    | 1     | 0             | -          | #00FF00 |
| CHAR_001 | 번데기    | 2     | 100           | -          | #FFA500 |
| CHAR_001 | 나비      | 3     | 500           | -          | #FF69B4 |
```

### ItemData 입력 (Excel)
```
ItemData 시트:
| ID       | Name        | Effect     | Value | Duration | IconPath |
|----------|-------------|------------|-------|----------|----------|
| ITEM_001 | +50 Clicks  | AddScore   | 50    | 0        | -        |
| ITEM_002 | Speed Boost | SpeedBoost | 0     | 60       | -        |
```

---

## 🚀 다음 단계

Phase 2 테스트 완료 후:
1. Excel 데이터 입력
2. 실제 스프라이트 적용
3. UI 디자인 개선
4. Phase 3 (UI 시스템) 으로 이동

---

**테스트 씬: Assets/Scenes/Phase2Test.unity**
