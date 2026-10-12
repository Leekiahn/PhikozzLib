# CameraManager

<img width="612" height="481" alt="Image" src="https://github.com/user-attachments/assets/b8dd4964-0bd5-43b3-9dc2-7ce89a05307e" />

> Cinemachine 카메라를 key로 등록하고 Priority로 전환하는 씬 단위 싱글톤(`SingletonScene<CameraManager>`)입니다.

## Public API

| Member | Description |
|---|---|
| `RegisterCamera(string cameraKey, CinemachineCamera cam)` | 카메라를 등록합니다. 이미 있는 key면 덮어씁니다. |
| `UnregisterCamera(string cameraKey)` | 카메라 등록을 해제합니다. 활성 카메라였다면 `OnCameraChanged(null)`을 호출합니다. |
| `SetCamera(string cameraKey)` | 해당 카메라로 전환합니다. 블렌딩은 `CinemachineBrain`이 처리합니다. |
| `GetCamera(string cameraKey)` | 카메라를 반환합니다. 없으면 `null`. |
| `GetActiveCamera()` | 현재 활성 카메라를 반환합니다. |
| `IsActive(string cameraKey)` / `IsActive(CinemachineCamera cam)` | 현재 활성 카메라인지 확인합니다. |
| `OnCameraChanged` | 활성 카메라가 바뀌면 호출됩니다. |

## 사용 예시

```csharp
CameraManager.Instance.SetCamera("BossCamera");

CameraManager.Instance.OnCameraChanged += cam =>
{
    Debug.Log($"Active camera changed to {cam?.name}");
};
```

## 주의사항

- 카메라 목록은 인스펙터의 `Cameras` 리스트(key + `CinemachineCamera`)에 등록합니다.
- **`Awake()`에서 `CameraManager.Instance`를 참조하지 않습니다.** 아직 `null`일 수 있으니 `Start()` 이후에 참조합니다.
- 씬에 배치해야 동작합니다. 없으면 자동 생성되지 않습니다.
- `OnCameraChanged`에는 `null`이 올 수 있습니다(활성 카메라를 등록 해제했을 때).
