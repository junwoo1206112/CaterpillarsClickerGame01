# Phase 1 테스트 가이드

## 📋 테스트 목표
1. DI 컨테이너가 정상 작동하는가?
2. 엑셀 데이터 생성/변환이 되는가?
3. 데이터를 로드할 수 있는가?

---

## 🎯 테스트 단계

### 단계 1: Unity 에서 컴파일 대기
```
1. Unity Editor 실행
2. Console 창에서 컴파일 완료될 때까지 대기
3. 빨간색 에러가 없으면 OK
```

### 단계 2: 엑셀 템플릿 생성
```
Unity 메뉴 > Tools > Data > Create Excel Template
```
- `Assets/ExcelData/GameData.xlsx` 가 생성됨
- 5 개 시트 확인: CharacterData, BackgroundData, ItemData, TouchFunctionData, ConfigData

### 단계 3: 엑셀에 데이터 입력 (테스트용)
```
GameData.xlsx 열기

CharacterData 시트:
| ID       | Name   | Stage | ScoreRequired | SpritePath | Color   |
|----------|--------|-------|---------------|------------|---------|
| CHAR_001 | 애벌레 | 1     | 0             | test       | #00FF00 |
| CHAR_001 | 번데기 | 2     | 100           | test       | #FFA500 |
| CHAR_001 | 나비   | 3     | 500           | test       | #FF69B4 |

BackgroundData 시트:
| ID      | Name   | SpritePath | UnlockScore |
|---------|--------|------------|-------------|
| BG_001  | Forest | test       | 0           |
| BG_002  | Desert | test       | 1000        |

저장 후 Excel 닫기
```

### 단계 4: ScriptableObject 로 변환
```
Unity 메뉴 > Tools > Data > Convert Excel to ScriptableObject
```
- Console 에 다음 로그 확인:
  ```
  Created CharacterList at: Assets/ScriptableObjects/Character/CharacterList.asset
  Created BackgroundList at: Assets/ScriptableObjects/Background/BackgroundList.asset
  Created ItemList at: Assets/ScriptableObjects/Item/ItemList.asset
  Created ConfigList at: Assets/ScriptableObjects/Config/ConfigList.asset
  ```

### 단계 5: 테스트 씬 실행
```
1. Assets/Scenes/Phase1Test.unity 더블클릭
2. Hierarchy 에 빈 GameObject 생성
3. Add Component > Phase1Test 추가
4. Play 버튼 클릭
5. 화면에 테스트 결과 표시
```

### 단계 6: 데이터 확인
```
1. Assets/ScriptableObjects/Character/CharacterList.asset 더블클릭
2. Inspector 에서 데이터 확인
3. Characters 리스트에 3 개 항목 있어야 함
```

---

## ✅ 성공 기준

| 항목 | 기준 | 결과 |
|------|------|------|
| DI 컨테이너 | Register/Resolve 성공 | ✓ |
| 엑셀 생성 | 5 개 시트 생성 | ✓ |
| 데이터 변환 | 4 개 ScriptableObject 생성 | ✓ |
| 데이터 로드 | Character 3 개 이상 로드 | ✓ |
| Console 에러 | 없음 | ✓ |

---

## 🐛 문제 해결

### Q1. Tools 메뉴가 안 보임
```
해결: Unity 재시작
```

### Q2. 컴파일 에러 발생
```
해결: 
1. Console 에서 에러 확인
2. NPOI DLL 이 Assets/Plugins 에 있는지 확인
3. Assets > Reimport All 실행
```

### Q3. 엑셀이 생성되지 않음
```
해결:
1. Assets/ExcelData 폴더 있는지 확인
2. Console 에러 확인
3. 다시 시도
```

### Q4. 데이터가 비어있음
```
해결:
1. 엑셀에 데이터 입력했는지 확인
2. 저장했는지 확인
3. 다시 변환 실행
```

---

## 📝 테스트 스크립트 설명

### Phase1Test.cs
- 화면에 테스트 결과 표시
- "Run Tests Again" 버튼으로 재실행
- DI, DataService, Converter 테스트

### SimpleDataViewer.cs
- 실제 로드된 데이터 표시
- 캐릭터, 배경, 아이템, 설정 개수 표시
- 데이터 상세 정보 출력

### DataManager.cs
- 게임 전체에서 데이터 관리
- Singleton 패턴
- DI 컨테이너 포함

---

## 🎮 다음 단계 (Phase 2)

Phase 1 테스트 성공 후:
1. ClickHandler.cs 구현 (클릭 처리)
2. ScoreManager.cs 구현 (점수 관리)
3. CharacterEvolution.cs 구현 (진화 시스템)
