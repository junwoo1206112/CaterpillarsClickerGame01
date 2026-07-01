# Caterpillar Clicker

Unity 6 기반의 캐주얼 클릭커 게임 프로토타입입니다. 캐릭터를 클릭해 점수와 포인트를 획득하고, 터치 강화 기능을 구매하며 애벌레에서 번데기, 나비로 성장하는 반복 성장 루프를 구현했습니다.

## Project Overview

이 프로젝트는 클릭커 장르의 핵심 구조인 입력, 점수 누적, 강화 구매, 성장 단계, UI 피드백을 Unity/C#로 구성한 작업입니다. 단순 클릭 샘플이 아니라 Excel 데이터, ScriptableObject 변환, 터치 기능 모듈, UI 패널, 에디터 자동화 도구까지 포함하도록 확장했습니다.

## Main Features

- 캐릭터 클릭 입력 처리
- 클릭 애니메이션 피드백
- 점수 및 터치 카운트 관리
- 애벌레, 번데기, 나비 3단계 진화 구조
- 터치 강화 기능 관리
- 크리티컬 클릭, 스피드 부스트, 보너스 포인트, 트리플 클릭 등 기능 확장 구조
- 포인트를 사용한 강화 구매 UI
- 캐릭터 색상, 배경, 설정 패널 관리
- Excel 데이터 로드 및 ScriptableObject 변환
- 간단한 DI Container 기반 서비스 등록/조회
- Phase별 테스트/검증용 스크립트 포함

## Tech Stack

- Unity `6000.3.10f1`
- C#
- Unity UI
- ScriptableObject
- NPOI Excel parser
- Unity Editor tooling

## Key Implementation Points

### Click and Score Loop

`ClickHandler`가 캐릭터 클릭 이벤트를 발생시키고, 점수/터치 카운트 시스템이 이를 받아 누적 값을 갱신합니다. 클릭 시 스케일 애니메이션을 적용해 즉각적인 피드백을 제공합니다.

### Evolution System

`CharacterEvolution`은 누적 터치 수를 기준으로 현재 성장 단계를 계산합니다. 기본 데이터 기준으로 0회, 1000회, 3000회 구간에서 캐릭터 상태가 변경되도록 구성했습니다.

### Touch Function System

`TouchFunctionManager`는 터치 기능 목록을 관리하고, 각 기능을 `ITouchFunction` 인터페이스 기반으로 처리합니다. 기능 추가/삭제, 데이터 기반 로드, 임시 기능 재사용, 스피드 부스트와 크리티컬 모드 처리를 분리했습니다.

### Data Pipeline

`ExcelDataService`는 Excel 행 데이터를 모델 필드에 매핑하고, `ExcelConverter`와 에디터 도구를 통해 ScriptableObject로 변환할 수 있게 구성했습니다. 반복적으로 수정되는 밸런스 데이터를 코드와 분리하려는 목적입니다.

## Folder Structure

```text
Assets/
  Scenes/
    GamePlayScene.unity
  Scripts/
    Core/        # GameContainer, DataInitializer, setup helpers
    Data/        # Excel service, converter, data models
    Gameplay/    # Click, score, item, evolution, touch functions
    UI/          # Panels, buttons, touch function list UI
    Managers/    # GameplayManager, AudioManager
    Tests/       # Phase test scripts
  Editor/        # Data conversion and scene setup tools
  ExcelData/
    GameData.xlsx
  Resources/     # Generated ScriptableObject assets
```

## How to Run

1. Unity Hub에서 Unity `6000.3.10f1` 이상으로 프로젝트를 엽니다.
2. `Assets/Scenes/GamePlayScene.unity`를 엽니다.
3. Play 버튼을 누릅니다.
4. 캐릭터를 클릭해 점수와 포인트를 얻고, 우측 강화 패널에서 기능을 구매합니다.

## Portfolio Notes

이 프로젝트는 메인 대표작보다는 캐주얼 게임의 반복 성장 구조와 데이터 기반 설계를 보여주는 보조 포트폴리오로 적합합니다. 클릭커 장르의 기본 루프, 기능 확장 구조, Unity UI 구성, Excel/ScriptableObject 데이터 흐름을 설명할 때 활용할 수 있습니다.

## Next Improvements

- 실제 플레이 캡처 또는 GIF 추가
- 클릭/강화/진화 흐름 다이어그램 추가
- Unity Test Runner 기반 기능 테스트 추가
- 모바일 터치 입력과 해상도 대응 점검
- WebGL 데모 배포
