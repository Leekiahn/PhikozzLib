# SaveEditorWindow
> 샘플 저장 데이터를 저장/로드/삭제하며 Save 서비스를 테스트하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Save Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `SaveManager` 또는 `Core.Save` 서비스가 필요합니다.

## 주요 기능
- 샘플 데이터 저장
- 샘플 데이터 로드
- 샘플 데이터 삭제
- 현재 로드된 샘플 데이터 요약 표시
- 임시 테스트 키 자동 정리

## 주요 입력값
- `Save Key`
- `Message`
- `Count`

## 주요 버튼
- `Save Sample`
- `Load Sample`
- `Delete Sample`

## 참고
- 기본 키 접두사는 `phikozzlib_editor_test_save`입니다.
- 창 종료 시 마지막 임시 테스트 키가 자동 삭제됩니다.
