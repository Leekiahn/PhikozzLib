# Generic Singleton

> 씬에 직접 배치해서 쓰는 매니저를 위한 싱글톤 베이스 클래스입니다. (서비스는 `ServiceLocator`를 사용)

## Public API

| Type | Description |
|---|---|
| `SingletonGlobal<T>` | 씬에 배치하면 `DontDestroyOnLoad`로 앱 전체에서 유지됩니다. |
| `SingletonLazy<T>` | `Instance`를 처음 참조할 때 씬에 없으면 자동 생성합니다. `Dont Destroy On Load` 옵션이 있습니다. |
| `SingletonScene<T>` | 현재 씬에서만 유지됩니다(예: `CameraManager`). |

공통으로 `Instance`로 접근하며, 중복 인스턴스는 `Awake()`에서 파괴됩니다.

## 사용 예시

```csharp
public class GameSettings : SingletonGlobal<GameSettings>
{
    public float MasterVolume { get; private set; } = 1f;

    protected override void Awake()
    {
        base.Awake();
        // 이후 초기화
    }
}

// 다른 곳에서
float volume = GameSettings.Instance.MasterVolume;
```

## 주의사항

- `Awake()`를 override하면 **`base.Awake()`를 먼저 호출**해야 `Instance`가 설정됩니다.
- 다른 스크립트의 `Awake()`에서 `Instance`를 참조하면 `null`일 수 있습니다. `Start()` 이후에 참조합니다.
- `SingletonGlobal` / `SingletonScene`은 씬에 배치하지 않으면 자동 생성되지 않습니다.
