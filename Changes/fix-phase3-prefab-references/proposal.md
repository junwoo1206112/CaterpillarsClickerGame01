# Proposal: Fix Phase 3 Prefab References

## Status
Proposed

## Problem Statement
TouchFunctionListItem.prefab의 모든 UI 참조가 null로 설정되어 있어 런타임에 UI가 정상적으로 작동하지 않습니다.

### Current State
- `nameText: {fileID: 0}` (null)
- `descriptionText: {fileID: 0}` (null)
- `costText: {fileID: 0}` (null)
- `levelText: {fileID: 0}` (null)
- `pointsText: {fileID: 0}` (null)
- `addButton: {fileID: 0}` (null)
- `removeButton: {fileID: 0}` (null)

### Root Cause
Prefab 내부 UI 요소 이름이 TouchFunctionListItem.cs에서 참조하는 이름과 일치하지 않음:

| 현재 Prefab 이름 | 코드에서 찾는 이름 |
|-----------------|------------------|
| FunctionName    | NameText         |
| Description     | DescriptionText  |
| Level           | LevelText        |
| (없음)          | CostText         |
| (없음)          | PointsText       |
| AddButton       | AddButton (일치)  |
| RemoveButton    | RemoveButton (일치)|

Phase3Setup.cs의 CreatePrefab() 메서드는 올바른 이름으로 UI 요소를 생성하지만, 이미 존재하는 Prefab이 있으면 건너뜁니다 (line 35-39).

## Proposed Solution
기존 Prefab을 삭제하고 Phase3Setup.CreatePrefab()을 실행하여 올바른 구조의 Prefab을 재생성합니다.

## Impact
- TouchFunctionListItem UI가 정상적으로 작동
- TouchFunctionListView에서 아이템 표시 가능
- 터치 기능 구매/제거 버튼 작동

## References
- `Assets/Scripts/UI/TouchFunctionListItem.cs`
- `Assets/Editor/Phase3Setup.cs`
- `Assets/Prefabs/TouchFunctionListItem.prefab`