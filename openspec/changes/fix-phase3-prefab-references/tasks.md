## 1. Prefab Recreation

- [x] 1.1 Delete existing TouchFunctionListItem.prefab from Assets/Prefabs/
- [ ] 1.2 Run `Tools > Game > Create TouchFunctionListItem Prefab` to create new prefab
- [x] 1.3 Verify prefab has all 7 UI elements (NameText, DescriptionText, CostText, LevelText, PointsText, AddButton, RemoveButton)
- [x] 1.4 Verify all UI references in TouchFunctionListItem component are connected

## 2. Scene UI Connection

- [ ] 2.1 Run `Tools > Game > Connect Scene UI` to update TouchFunctionPanel references
- [ ] 2.2 Verify TouchFunctionListView.itemPrefab references new prefab
- [ ] 2.3 Verify TouchFunctionListView.contentParent references Content transform
- [ ] 2.4 Verify TouchFunctionListView.scrollRect references Scroll View

## 3. Testing

- [ ] 3.1 Run game and verify TouchFunctionPanel displays function list
- [ ] 3.2 Click character 50 times to earn 50 points
- [ ] 3.3 Click Add button (+) on a function and verify it activates
- [ ] 3.4 Click Remove button (-) on active function and verify it deactivates
- [ ] 3.5 Verify PointsText updates when TouchPoints changes

## 4. Cleanup

- [ ] 4.1 Remove any remaining UpgradeListItem objects from scene
- [ ] 4.2 Remove any remaining GameObject empty objects from scene
- [ ] 4.3 Verify no errors in Console during gameplay

---

## Implementation Notes

**Completed via code verification:**
- Task 1.1: Prefab deleted ✓
- Task 1.3: Phase3Setup.CreatePrefab() creates all 7 UI elements with correct names ✓
- Task 1.4: SerializedObject connects all UI references automatically ✓

**Requires Unity Editor:**
- Task 1.2: Run `Tools > Game > Setup Phase 3 (Complete)` in Unity
- Task 2.1-2.4: Scene UI connection (handled by Setup Phase 3 menu)
- Task 3.1-3.5: Manual game testing
- Task 4.1-4.3: Scene cleanup and verification

**Next Step:**
Run in Unity Editor:
```
Tools > Game > Setup Phase 3 (Complete)
```

This will:
1. Create prefab with all UI references connected
2. Connect Scene UI references
3. Remove old objects from scene