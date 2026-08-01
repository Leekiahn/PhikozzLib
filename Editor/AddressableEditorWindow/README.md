# AddressableEditorWindow

<img width="554" height="750" alt="Image" src="https://github.com/user-attachments/assets/8b482f9b-c686-4e71-98f2-3ef022feba9f" />

> Addressable 라벨 다운로드, 프리로드, 캐시 상태 확인, 에셋 해제를 테스트하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Addressable Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `AddressableManager`가 필요합니다.

## 주요 기능
- 라벨 dependency 다운로드
- 라벨 location 프리로드
- 라벨 에셋 전체 프리로드
- 라벨/키 기준 로드 상태 확인
- 특정 에셋 또는 라벨 전체 해제
- 현재 캐시된 라벨과 키 목록 확인

## 주요 입력값
- `Label`: Addressables 라벨명
- `Key`: 라벨 내부 에셋 키
- `AddressableManager`: 테스트 대상 매니저

## 주요 버튼
- `Download`
- `Preload Locations`
- `Preload Assets`
- `Is Loaded Asset Key`
- `Is Cached Label`
- `Release`
- `Release All`

## 참고
- 내부 캐시 정보를 반영해 라벨별 다운로드/로드 상태를 Foldout으로 표시합니다.
- `Label`과 `Key`가 비어 있으면 동작하지 않습니다.
