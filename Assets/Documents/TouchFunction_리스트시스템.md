# 🎮 터치 기능 리스트 시스템 (엑셀 연동)

## 📋 완료 일자
- **완료일**: 2026-04-06
- **기간**: 1 시간

---

## ✅ 변경 내용

### 1. 풀 시스템 → 리스트 시스템
```
❌ TouchFunctionPool (풀 시스템) 제거
✅ 리스트 기반으로 단순화
✅ 재사용 가능한 기능은 계속 유지
✅ 일회성 기능은 자동 삭제
```

### 2. 엑셀 데이터 연동
```
✅ TouchFunctionData 시트 추가
✅ 10 개 컬럼으로 완전한 설정
✅ ScriptableObject 자동 변환
✅ 런타임 데이터 로드
```

### 3. 새로운 데이터 모델
```csharp
TouchFunctionDataModel:
- ID (문자열)
- FunctionName (문자열)
- FunctionType (BonusClick, SpeedBoost, CriticalClick)
- TriggerCount (정수)
- Multiplier (실수)
- Duration (실수)
- Cooldown (실수)
- CriticalChance (실수 0-1)
- IsActive (불리언)
- IsReusable (불리언)
```

---

## 📁 엑셀 시트 구조

### TouchFunctionData 시트

| 컬럼 | 타입 | 설명 | 예시 |
|------|------|------|------|
| ID | string | 고유 ID | TOUCH_001 |
| FunctionName | string | 기능 이름 | Bonus Click |
| FunctionType | string | 기능 타입 | BonusClick |
| TriggerCount | int | 발동 조건 | 50 |
| Multiplier | float | 배율 | 1.33 |
| Duration | float | 지속시간 | 60 |
| Cooldown | float | 쿨다운 | 300 |
| CriticalChance | float | 크리티컬 확률 | 0.1 |
| IsActive | bool | 활성화 여부 | true |
| IsReusable | bool | 재사용 가능 | true |

---

## 🎮 사용 방법

### 1. 엑셀 템플릿 생성
```
Tools > Data > Create Excel Template
```

### 2. 엑셀에 데이터 입력
```
TouchFunctionData 시트:

| ID       | FunctionName   | FunctionType  | TriggerCount | Multiplier | Duration | Cooldown | CriticalChance | IsActive | IsReusable |
|----------|----------------|---------------|--------------|------------|----------|----------|----------------|----------|------------|
| TOUCH_001| Bonus Click    | BonusClick    | 50           | 1.33       | 0        | 0        | 0              | true     | true       |
| TOUCH_002| Speed Boost    | SpeedBoost    | 0            | 2          | 60       | 300      | 0              | true     | true       |
| TOUCH_003| Critical Click | CriticalClick | 0            | 5          | 0        | 0        | 0.1            | true     | true       |
```

### 3. ScriptableObject 변환
```
Tools > Data > Convert Excel to ScriptableObject
```

### 4. GameplayManager 에 할당
```
Hierarchy:
1. GameplayManager 선택
2. Inspector 에서 TouchFunctionData 필드 찾기
3. TouchFunctionListSO 할당
   (Assets/ScriptableObjects/TouchFunction/TouchFunctionList.asset)
```

### 5. 테스트
```
1. Play 버튼 클릭
2. 캐릭터 클릭
3. Console 에서 로그 확인
4. 50 회 클릭 후 보너스 활성화 확인
```

---

## 🔧 코드 구조

### TouchFunctionFactory
```csharp
- 엑셀 데이터 → ITouchFunction 변환
- Create(): 단일 인스턴스 생성
- CreateList(): 리스트 생성
```

### TouchFunctionManager
```csharp
- _functions: 모든 기능 리스트
- _reusableFunctions: 재사용 가능 기능
- _temporaryFunctions: 일회성 기능
- LoadFromData(): 엑셀에서 로드
- AddFunction(): 기능 추가
- RemoveFunction(): 기능 제거
- ResetAll(): 초기화
```

### ITouchFunction
```csharp
- IsReusable: 재사용 가능 여부
- Reset(): 상태 초기화
- Clone(): 인스턴스 복제
```

---

## ✅ 장점

| 항목 | 풀 시스템 | 리스트 시스템 |
|------|-----------|---------------|
| 복잡도 | 높음 | 낮음 |
| 메모리 | 효율적 | 보통 |
| 유지보수 | 어려움 | 쉬움 |
| 엑셀 연동 | 어려움 | 쉬움 |
| 확장성 | 제한적 | 무한 |

---

## 📊 생성된 파일

### 스크립트 (3 개)
```
Data/Models/
└── TouchFunctionDataModel.cs

Gameplay/TouchFunction/
├── TouchFunctionFactory.cs
└── TouchFunctionManager.cs (수정)
```

### 에디터 (2 개 수정)
```
Editor/
├── ExcelToScriptableObjectConverter.cs (수정)
└── ExcelValidator.cs (수정)
```

### 엑셀 (1 개 시트 추가)
```
ExcelData/
└── GameData.xlsx (TouchFunctionData 시트 추가)
```

### ScriptableObject (1 개)
```
ScriptableObjects/
└── TouchFunction/
    └── TouchFunctionList.asset
```

---

## 🎯 테스트 항목

| 항목 | 방법 | 결과 |
|------|------|------|
| 엑셀 생성 | Tools > Data > Create Template | ⬜ |
| 데이터 입력 | Excel 에서 수정 | ⬜ |
| SO 변환 | Tools > Data > Convert | ⬜ |
| 기능 로드 | GameplayManager 할당 | ⬜ |
| 보너스 클릭 | 50 회 클릭 | ⬜ |
| 스피드 부스터 | 버튼 클릭 | ⬜ |
| 크리티컬 | 확률 테스트 | ⬜ |
| 재사용 | 여러 번 활성화 | ⬜ |

---

## 🚀 다음 단계

1. **Phase 2 테스트 완료**
2. **Phase 3 (UI 시스템) 시작**
   - Canvas UI 디자인
   - 상단 버튼 5 개
   - 설정 윈도우

---

**문서**: `Assets/Documents/TouchFunction_리스트시스템.md`  
**엑셀**: `Assets/ExcelData/GameData.xlsx` (TouchFunctionData 시트)  
**SO**: `Assets/ScriptableObjects/TouchFunction/TouchFunctionList.asset`
