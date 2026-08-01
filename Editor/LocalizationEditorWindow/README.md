# LocalizationEditorWindow
> 현재 Locale 상태를 확인하고 Locale 전환 및 로컬라이즈 문자열 조회를 테스트하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Localization Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `LocalizationManager` 또는 `Core.Local` 서비스가 필요합니다.

## 주요 기능
- 현재 선택된 Locale 확인
- 사용 가능한 Locale Code 목록 표시
- Locale Code 기준 언어 전환
- Table / Entry 기준 문자열 조회
- 창 종료 시 초기 Locale 복구

## 주요 입력값
- `Locale Code`
- `Table`
- `Entry`

## 주요 버튼
- `Set Locale`
- `Get String`

## 참고
- 창을 닫으면 시작 시점의 Locale Code로 되돌립니다.
- 문자열 조회는 `Table`과 `Entry`가 모두 입력되어야 합니다.
