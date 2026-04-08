# Tasks: Fix Phase 3 Prefab References

## Checklist

- [ ] **Task 1: Delete existing Prefab**
  - Unity Project 창에서 `Assets/Prefabs/TouchFunctionListItem.prefab` 삭제
  - 또는 파일 시스템에서 직접 삭제

- [ ] **Task 2: Recreate Prefab**
  - Unity Editor 메뉴: `Tools > Game > Create TouchFunctionListItem Prefab`
  - 또는 `Tools > Game > Setup Phase 3 (Complete)` 실행

- [ ] **Task 3: Verify Prefab References**
  - Project 창에서 새로 생성된 Prefab 선택
  - Inspector에서 TouchFunctionListItem 컴포넌트 확인
  - 다음 필드들이 null이 아닌지 확인:
    - [ ] nameText → NameText (Text)
    - [ ] descriptionText → DescriptionText (Text)
    - [ ] costText → CostText (Text)
    - [ ] levelText → LevelText (Text)
    - [ ] pointsText → PointsText (Text)
    - [ ] addButton → AddButton (Button)
    - [ ] removeButton → RemoveButton (Button)

- [ ] **Task 4: Verify Runtime Behavior**
  - Play Mode 진입
  - TouchFunctionPanel에서 아이템 표시 확인
  - Add/Remove 버튼 클릭 동작 확인

## Commands

### Delete Prefab (if needed via CLI)
```bash
# Windows PowerShell
Remove-Item "Assets/Prefabs/TouchFunctionListItem.prefab"
Remove-Item "Assets/Prefabs/TouchFunctionListItem.prefab.meta" -ErrorAction SilentlyContinue
```

### Unity Editor Menu
- `Tools > Game > Create TouchFunctionListItem Prefab`
- 또는 `Tools > Game > Setup Phase 3 (Complete)`

## Notes
- Task 1-2는 Unity Editor에서 수행 권장
- Prefab 재생성 후 자동으로 AssetDatabase.SaveAssets() 호출됨