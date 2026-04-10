## 1. SettingManager 코드 수정

- [ ] 1.1 SettingManager.cs 에 BGM 테스트 버튼 참조 추가 (testBgmButton)
- [ ] 1.2 SettingManager.cs 에 SFX 테스트 버튼 참조 추가 (testSfxButton)
- [ ] 1.3 SetupUI() 메서드에 테스트 버튼 이벤트 리스너 등록
- [ ] 1.4 OnTestBGMClicked() 메서드 구현 (AudioManager.Instance.PlayBGM 호출)
- [ ] 1.5 OnTestSFXClicked() 메서드 구현 (AudioManager.Instance.PlaySFX 호출)
- [ ] 1.6 OnDestroy() 에서 이벤트 리스너 제거

## 2. Prefab UI 수정

- [ ] 2.1 SettingPanel Prefab 에 BGM 테스트 버튼 추가
- [ ] 2.2 SettingPanel Prefab 에 SFX 테스트 버튼 추가
- [ ] 2.3 각 버튼을 해당 슬라이더 옆에 배치
- [ ] 2.4 버튼에 텍스트 레이블 설정 ("Test" 또는 "🔊")

## 3. Inspector 연결 및 테스트

- [ ] 3.1 Scene 에서 SettingPanel 의 SettingManager 컴포넌트 확인
- [ ] 3.2 Inspector 에서 새 버튼 슬롯에 UI 버튼 연결
- [ ] 3.3 Unity Editor 에서 테스트 (볼륨 조절 후 테스트 버튼 클릭)
- [ ] 3.4 BGM/SFX 가 현재 볼륨으로 정상 재생되는지 확인
