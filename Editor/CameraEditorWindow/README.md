# CameraEditorWindow

<img width="482" height="598" alt="Image" src="https://github.com/user-attachments/assets/33f83cd9-bce3-42ab-8960-7f1db76ece24" />

> 등록된 카메라 상태를 확인하고 `eCameraType` 기준으로 활성 카메라를 전환하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Camera Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `CameraManager`가 필요합니다.

## 주요 기능
- 현재 활성 카메라 정보 확인
- 등록된 카메라 목록 확인
- `eCameraType` 선택 후 활성 카메라 전환

## 표시 정보
- 현재 활성 카메라 타입
- 활성 카메라 이름
- 현재 Priority
- 등록된 각 카메라의 활성 여부

## 주요 버튼
- `Set Camera`

## 참고
- 선택한 타입에 등록된 카메라가 없으면 전환하지 않고 오류 상태를 표시합니다.
