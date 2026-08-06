# CameraManager

<img width="612" height="481" alt="Image" src="https://github.com/user-attachments/assets/b8dd4964-0bd5-43b3-9dc2-7ce89a05307e" />

> 카메라 전환 및 관리 기능을 제공합니다.  
> `CameraManager`는 씬 단위 싱글톤으로 동작합니다.

---

## 주요 기능

- Key 기반 카메라 등록/조회
- 활성 카메라 전환
- 카메라 우선순위 제어
- 현재 활성 카메라 확인
- 카메라 전환 이벤트 제공
- Cinemachine 기반 카메라 시스템 사용

---

## Public API

| Method | Description |
|---|---|
| `RegisterCamera(string cameraKey, CinemachineCamera cam)` | 카메라 Key와 카메라 인스턴스를 등록합니다. |
| `UnregisterCamera(string cameraKey)` | 등록된 카메라를 제거합니다. |
| `SetCamera(string cameraKey)` | 지정한 Key의 카메라를 활성화합니다. |
| `GetCamera(string cameraKey)` | 지정한 Key의 카메라를 반환합니다. |
| `GetActiveCamera()` | 현재 활성 카메라를 반환합니다. |
| `IsCurrent(string cameraKey)` | 해당 Key의 카메라가 현재 활성 카메라인지 확인합니다. |
| `IsCurrent(CinemachineCamera cam)` | 해당 카메라가 현재 활성 카메라인지 확인합니다. |

---

## 동작 방식

- 기본 카메라는 `CameraManager.prefab`을 통해 세팅합니다.
- 내부적으로 카메라 목록을 Dictionary로 관리합니다.
- `SetCamera()` 호출 시:
  - 기존 활성 카메라의 Priority를 낮추고
  - 대상 카메라의 Priority를 높여 전환합니다.
- 카메라가 바뀌면 `OnCameraChanged` 이벤트가 호출됩니다.
