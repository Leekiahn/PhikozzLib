# FloatingTextEditorWindow
> 설정된 FloatingText 타입을 확인하고 샘플 텍스트를 원하는 위치로 스폰하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Floating Text Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `FloatingTextManager` 또는 `Core.FloatingText` 서비스가 필요합니다.

## 주요 기능
- `eFloatingTextType` 선택 스폰
- 텍스트 값, 위치, 방향 지정
- 현재 설정된 FloatingText 타입 목록 조회

## 주요 입력값
- `Type`
- `Value`
- `Position`
- `Direction`

## 주요 버튼
- `Spawn`

## 참고
- 값이 비어 있으면 스폰되지 않습니다.
- 설정되지 않은 타입은 선택해도 실행되지 않습니다.
