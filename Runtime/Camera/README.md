# CameraManager

<img width="612" height="481" alt="Image" src="https://github.com/user-attachments/assets/b8dd4964-0bd5-43b3-9dc2-7ce89a05307e" />

> 카메라 전환 및 관리 기능을 제공합니다.  
> `CameraManager`는 씬 단위 싱글톤(`SingletonScene<CameraManager>`)으로 동작합니다.

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
| `RegisterCamera(string cameraKey, CinemachineCamera cam)` | 카메라 Key와 카메라 인스턴스를 등록합니다. 이미 있는 Key면 덮어씁니다. |
| `UnregisterCamera(string cameraKey)` | 등록된 카메라를 제거합니다. 그 카메라가 현재 활성 카메라였다면 활성 상태도 같이 정리하고 `OnCameraChanged(null)`을 호출합니다. |
| `SetCamera(string cameraKey)` | 지정한 Key의 카메라를 활성화합니다. 등록되지 않은 Key면 경고 로그를 남기고 아무 것도 하지 않습니다. |
| `GetCamera(string cameraKey)` | 지정한 Key의 카메라를 반환합니다. |
| `GetActiveCamera()` | 현재 활성 카메라를 반환합니다. |
| `IsActive(string cameraKey)` | 해당 Key의 카메라가 현재 활성 카메라인지 확인합니다. |
| `IsActive(CinemachineCamera cam)` | 해당 카메라가 현재 활성 카메라인지 확인합니다. |

---

## 동작 방식

- 카메라 목록은 인스펙터의 `Cameras` 리스트(Key + `CinemachineCamera`)로 등록하고, `Awake()`에서 내부 Dictionary로 옮겨 담습니다.
- `SetCamera()` 호출 시:
  - 기존 활성 카메라의 Priority를 낮추고(`InactivePriority`)
  - 대상 카메라의 Priority를 높여(`ActivePriority`) 전환합니다.
  - 실제 블렌딩 연출은 `CinemachineBrain`이 처리합니다 — `CameraManager`는 Priority만 바꿉니다.
- 카메라가 바뀌면 `OnCameraChanged` 이벤트가 호출됩니다. 활성 카메라가 `UnregisterCamera`로 제거된 경우에도 "활성 카메라 없음"을 알리기 위해 `null`과 함께 호출됩니다.

---

## 사용 예시

```csharp
// 씬에 배치된 CameraManager는 SingletonScene이라 ServiceLocator를 거치지 않습니다.
CameraManager.Instance.SetCamera("BossCamera");

CameraManager.Instance.OnCameraChanged += cam =>
{
    Debug.Log($"Active camera changed to {cam.name}");
};
```

---

## 주의사항: `Awake()`에서 참조하지 마세요

`CameraManager.Instance`는 `CameraManager` 자신의 `Awake()`가 실행돼야 세팅되는데, Unity는 서로 다른 오브젝트의 `Awake()` 호출 순서를 보장하지 않습니다. 다른 스크립트의 `Awake()`에서 `CameraManager.Instance`를 참조하면, 그 시점에 아직 `CameraManager`의 `Awake()`가 실행되지 않아 `Instance`가 `null`일 수 있습니다.

`Start()`(또는 그 이후 시점)에서 참조하세요 — 그때는 씬의 모든 `Awake()` 호출이 끝난 뒤입니다. 또한 `CameraManager`는 `SingletonLazy`가 아니라서, 씬에 아예 배치돼 있지 않으면 자동 생성되지 않고 계속 `null`로 남습니다.
