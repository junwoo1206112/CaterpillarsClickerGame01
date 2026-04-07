# Clicker Game Caterpillars - 프로젝트 개요

## 프로젝트 구조

```
Assets/
├── Documents/
│   └── Design/
│       ├── Phase1_CoreSystem/
│       │   ├── 01_구현계획문서.md
│       │   ├── 02_세부스펙_엑셀데이터시스템.md
│       │   ├── 03_세부스펙_DI 아키텍처.md
│       │   └── 04_마일스톤_TaskList.md
│       ├── Phase2_Gameplay/
│       │   ├── 01_구현계획문서.md
│       │   ├── 02_세부스펙_터치기능시스템.md
│       │   └── 03_마일스톤_TaskList.md
│       ├── Phase3_UI/
│       │   ├── 01_구현계획문서.md
│       │   ├── 02_세부스펙_UI 시스템.md
│       │   └── 03_마일스톤_TaskList.md
│       └── Phase4_Content/
│           ├── 01_구현계획문서.md
│           └── 03_마일스톤_TaskList.md
├── Scripts/
│   ├── Core/
│   │   ├── GameContainer.cs (DI 컨테이너)
│   │   └── DataInitializer.cs (데이터 초기화)
│   ├── Data/
│   │   ├── IDataService.cs
│   │   ├── IExcelConverter.cs
│   │   ├── ExcelDataService.cs
│   │   ├── ExcelConverter.cs
│   │   └── Models/
│   │       ├── DataModels.cs (엑셀 데이터 모델)
│   │       └── ScriptableObjectModels.cs (SO 모델)
│   ├── Managers/
│   ├── Gameplay/
│   ├── UI/
│   └── Utilities/
├── Editor/
│   ├── DataMenu.cs (MenuItem)
│   ├── ExcelToScriptableObjectConverter.cs
│   └── ExcelValidator.cs
├── Plugins/ (NPOI DLLs)
├── ScriptableObjects/
│   ├── Character/
│   ├── Stage/
│   ├── Item/
│   └── Config/
└── ExcelData/
    └── GameData.xlsx (생성 예정)
```

## 사용법

### 1. 엑셀 템플릿 생성
```
Unity 메뉴 > Tools > Data > Create Excel Template
```
- `Assets/ExcelData/GameData.xlsx` 파일이 생성됩니다.
- 5 개 시트 (CharacterData, BackgroundData, ItemData, TouchFunctionData, ConfigData) 가 포함됩니다.

### 2. 데이터 편집
- 생성된 Excel 파일을 Microsoft Excel 또는 호환 프로그램으로 엽니다.
- 각 시트의 컬럼에 맞춰 데이터를 입력합니다.
- ID 컬럼은 필수이며 중복될 수 없습니다.

### 3. ScriptableObject 로 변환
```
Unity 메뉴 > Tools > Data > Convert Excel to ScriptableObject
```
- 엑셀 데이터가 ScriptableObject 로 변환됩니다.
- `Assets/ScriptableObjects/` 폴더에 저장됩니다.

### 4. 데이터 검증
```
Unity 메뉴 > Tools > Data > Validate Excel Data
```
- 데이터 유효성을 검사합니다.
- 오류 시 Console 에 에러 메시지가 표시됩니다.

## DI 컨테이너 사용법

### 서비스 등록
```csharp
var container = new GameContainer();
container.Register<IDataService<List<CharacterDataModel>>, ExcelDataService<CharacterDataModel>>(
    new ExcelDataService<CharacterDataModel>()
);
```

### 서비스 조회
```csharp
var dataService = container.Resolve<IDataService<List<CharacterDataModel>>>();
var data = await dataService.LoadAsync("path/to/excel");
```

## Phase 별 진행 상황

| Phase | 내용 | 상태 |
|-------|------|------|
| Phase1 | 핵심 시스템 (DI, 엑셀 데이터) | ✅ 완료 |
| Phase2 | Gameplay (터치, 아이템) | ⏳ 대기 |
| Phase3 | UI 시스템 | ⏳ 대기 |
| Phase4 | 콘텐츠 확장 | ⏳ 대기 |

## 기술 스택
- Unity 2022+
- C# 9.0+
- NPOI 2.7.7 (엑셀 처리)
- DI 패턴 (의존성 주입)
- ScriptableObject (데이터 저장)

## 다음 단계 (Phase2)
1. 터치 서비스 구현
2. 터치 기능 시스템 (전략 패턴)
3. 아이템 시스템
4. 점수 계산 시스템
