# SceneEditorWindow

<img width="468" height="461" alt="Image" src="https://github.com/user-attachments/assets/5f9de0aa-7392-4cfa-972c-82c858608ce2" />

> 씬 로드/언로드/프리로드/Hold 상태를 테스트하고 현재 로드된 씬 목록을 확인하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Scene Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `SceneLoadManager` 또는 `Core.Scene` 서비스가 필요합니다.

## 주요 기능
- Single 씬 로드
- Loading Scene을 포함한 씬 로드
- Additive 씬 로드/언로드
- Preload 후 활성화
- Hold 모드 및 상태 적용
- 현재 로드된 씬 목록 표시
- 창 종료 시 테스트 중 변경한 Hold/임시 Additive 씬 정리

## 주요 입력값
- `Scene Name`
- `Loading Scene`
- `Hold Mode`
- `Hold Status`

## 주요 버튼
- `Load Scene`
- `Load With Loading`
- `Load Additive`
- `Load Additive With Loading`
- `Unload Scene`
- `Preload Additive`
- `Activate Preloaded`
- `Apply Hold`

## 참고
- `Load Scene`, `Load With Loading`은 현재 Play Mode 씬 상태를 크게 바꿀 수 있어 확인 다이얼로그를 표시합니다.
- 창을 닫으면 이 창이 로드한 Additive 씬과 Hold 상태를 정리합니다.
