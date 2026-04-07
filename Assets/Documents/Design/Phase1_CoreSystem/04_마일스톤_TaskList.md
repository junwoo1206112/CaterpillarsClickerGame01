# Phase1 마일스톤 및 Task List

## 마일스톤 정의
- **시작일**: 2026-04-06
- **목표일**: 2026-04-12
- **상태**: ✅ 완료 (2026-04-06)

## 워크플로우
1. 리서치 → 2. 구현계획 → 3. 구현 (TDD) → 4. 유저검수

---

## Task 1: 리서치
**상태**: ✅ 완료 | **우선순위**: High | **예상시간**: 2h

### 하위 태스크
- [x] Unity DI 패턴 레퍼런스 조사
- [x] NPOI for Unity 사용법 확인 (GitHub: sarmalev2/NPOI-for-Unity-6)
- [x] ScriptableObject 데이터 구조 조사
- [x] 기존 클릭커 게임 아키텍처 조사

### 산출물
- `Phase1_CoreSystem/05_리서치문서.md`

---

## Task 2: 구현계획 (세부)
**상태**: ✅ 완료 | **우선순위**: High | **예상시간**: 3h

### 하위 태스크
- [x] 클래스 다이어그램 작성
- [x] 시퀀스 다이어그램 작성
- [x] 테스트 계획 수립
- [x] 폴더 구조 확정

### 산출물
- UML 다이어그램
- 테스트 케이스 목록

---

## Task 3: 구현 (TDD)
**상태**: ✅ 완료 | **우선순위**: High | **예상시간**: 12h

### Sprint 3.1: DI 컨테이너 (2h) ✅
- [x] `GameContainer` 클래스 구현
- [x] `Register<T>` 메서드 구현
- [x] `Resolve<T>` 메서드 구현
- [x] 단위 테스트 작성

### Sprint 3.2: 데이터 인터페이스 (2h) ✅
- [x] `IDataService<T>` 인터페이스 정의
- [x] `IExcelConverter` 인터페이스 정의
- [x] 모델 클래스 정의 (CharacterData, StageData, etc.)

### Sprint 3.3: NPOI 통합 (3h) ✅
- [x] NPOI 패키지 설치
- [x] `ExcelDataService<T>` 구현
- [x] 엑셀 읽기 기능 구현
- [x] 엑셀 쓰기 기능 구현
- [x] 통합 테스트 작성

### Sprint 3.4: ScriptableObject 컨버터 (3h) ✅
- [x] `CharacterDataSO` ScriptableObject 정의
- [x] `BackgroundDataSO` ScriptableObject 정의 (StageData → 변경)
- [x] `ItemDataSO` ScriptableObject 정의
- [x] 컨버전 로직 구현
- [x] 에러 핸들링 구현

### Sprint 3.5: MenuItem 구현 (2h) ✅
- [x] `CreateExcelTemplate` MenuItem 구현
- [x] `ConvertToScriptableObject` MenuItem 구현
- [x] `ValidateExcelData` MenuItem 구현
- [x] 에디터 UI 구현

---

## Task 4: 테스트 검증
**상태**: ✅ 완료 | **우선순위**: High | **예상시간**: 3h

### 테스트 항목
- [x] 빈 엑셀 생성 테스트
- [x] 엑셀 → ScriptableObject 변환 테스트
- [x] 유효성 검증 테스트
- [x] 에러 처리 테스트
- [x] DI 컨테이너 테스트
- [x] Phase1Test 스크립트 생성

### 테스트 방법
- EditorTest (Unity Test Framework)
- 수동 테스트 (MenuItem 직접 실행)

---

## Task 5: 개발자 검수
**상태**: ✅ 완료 | **우선순위**: High | **예상시간**: 2h

### 검수 항목
- [x] 코드 리뷰 (PR 생성)
- [x] 성능 체크 (대량 데이터 변환)
- [x] 메모리 누수 체크
- [x] 에러 로그 확인

### 검수 기준
- [x] 모든 테스트 통과
- [x] 코드 커버리지 80% 이상
- [x] Console Error 없음

---

## Task 6: 유저 검수
**상태**: ✅ 완료 | **우선순위**: Medium | **예상시간**: 1h

### 검수 항목
- [x] MenuItem 사용성 테스트
- [x] 엑셀 데이터 수정 후 변환 테스트
- [x] 에러 메시지 명확성 확인

---

## 완료 조건 (Definition of Done)
1. [x] 모든 Task 완료
2. [x] 모든 테스트 통과
3. [x] 코드 리뷰 완료
4. [x] 문서화 완료
5. [x] Git 커밋 및 푸시

---

## 리스크 및 대응
| 리스크 | 확률 | 영향 | 대응 | 결과 |
|--------|------|------|------|------|
| NPOI 호환성 문제 | Medium | High | 대체 라이브러리 조사 (MiniExcel) | ✅ 해결됨 |
| ScriptableObject 직렬화 문제 | Low | Medium | JSON 병행 지원 | ✅ 해결됨 |
| 대량 데이터 성능 | Low | Low | 비동기 처리 구현 | ✅ 해결됨 |
| 네임스페이스 충돌 | Low | Medium | EditorTools 별도 네임스페이스 | ✅ 해결됨 |

---

## 📋 Phase 1 완료 요약

### 완료된 기능
- ✅ DI 컨테이너 (GameContainer)
- ✅ 엑셀 데이터 시스템 (IDataService, ExcelDataService)
- ✅ NPOI 라이브러리 통합 (16 개 DLL)
- ✅ 엑셀 템플릿 생성 (5 개 시트)
- ✅ ScriptableObject 컨버터
- ✅ 데이터 유효성 검사
- ✅ 테스트 스크립트 (Phase1Test, SimpleDataViewer)
- ✅ 테스트 씬 (Phase1Test.unity)

### 생성된 문서
- ✅ 구현계획문서 (01_구현계획문서.md)
- ✅ 세부스펙 (02_세부스펙_엑셀데이터시스템.md, 03_세부스펙_DI 아키텍처.md)
- ✅ 마일스톤/TaskList (04_마일스톤_TaskList.md)
- ✅ 테스트가이드 (Phase1_테스트가이드.md)

### 다음 단계 (Phase 2)
- Gameplay 시스템 구현
- 클릭/점수/진화 로직
- 터치 기능 시스템
