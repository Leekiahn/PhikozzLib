# ServiceLocator

> 전역 서비스를 인터페이스 타입으로 등록·조회하는 서비스 로케이터입니다.

## Public API

| Member | Description |
|---|---|
| `ServiceLocator.Register<T>(T service)` | 서비스를 `T` 타입으로 등록합니다. |
| `ServiceLocator.Unregister<T>()` | 서비스 등록을 해제합니다. |
| `ServiceLocator.Get<T>()` | 등록된 서비스를 조회합니다. |
| `IServiceRegister` | `RegisterService()` / `UnregisterService()`를 구현하면 `Bootstrapper`가 호출합니다. |
| `IServiceInit` | `InitAsync()`를 구현하면 모든 서비스 등록이 끝난 뒤 `Bootstrapper`가 호출합니다. |

## 사용 예시

<img width="612" height="425" alt="Image" src="https://github.com/user-attachments/assets/cd5e7405-d452-4ce7-b0e6-96c3ea82770e" />

**서비스 만들기** (프리팹으로 만들어 `BootstrapConfig`에 등록)

```csharp
public class UIManager : MonoBehaviour, IUIService, IServiceRegister, IServiceInit
{
    private IAddressableService _addressableService;

    public void RegisterService()
    {
        ServiceLocator.Register<IUIService>(this);
    }

    public void UnregisterService()
    {
        ServiceLocator.Unregister<IUIService>();
    }

    public UniTask InitAsync() // 다른 서비스가 필요할 때만 구현
    {
        _addressableService = ServiceLocator.Get<IAddressableService>();
        return UniTask.CompletedTask;
    }
}
```

**서비스 사용하기**

```csharp
private IFloatingTextService _floatingTextService;

private void Start()
{
    _floatingTextService = ServiceLocator.Get<IFloatingTextService>();
}
```

## 주의사항

- **`Awake()`에서 `Get<T>()`를 호출하지 않습니다.** 서비스 안에서는 `InitAsync()`, 씬 오브젝트에서는 `Start()` 이후에 조회합니다.
