# ServiceLocater
- 해당 서비스 로케이터는 전역적인 공통 시스템 서비스 등록을 위해 IServiceRegister 인터페이스를 제공합니다.

<br>

## IServiceRegister

```csharp
public void RegisterService()
{
    ServiceLocator.Register<IEffectService>(this);
}

public void UnregisterService()
{
    ServiceLocator.Unregister<IEffectService>();
}
```

- 원하는 서비스 클래스에 `IServiceRegister` 인터페이스를 상속합니다.
- `RegisterService()` 메서드 안에 `ServiceLocator.Register<T>(T service)` 메서드를 호출해 서비스를 등록합니다.
- `UnregisterService()` 메서드 안에 `ServiceLocator.Unregister<T>()` 메서드를 호출해 서비스를 해제합니다.

<br>
<br>

## IServiceInit (다른 서비스를 참조해야 할 때)

`Awake()`에서 `ServiceLocator.Get<T>()`를 호출하지 마세요. `BootstrapConfig`의 매니저 등록 순서에 따라 다른 서비스가 아직 등록되지 않았을 수 있습니다.  
다른 서비스를 조회해야 하는 서비스는 `IServiceInit`을 추가로 상속하세요. `Bootstrapper`가 **모든 서비스의 `RegisterService()`가 끝난 뒤** `Init()`을 호출해줍니다.

```csharp
public class UIManager : MonoBehaviour, IUIService, IServiceRegister, IServiceInit
{
    private IAddressableService _addressableService;

    public void Init()
    {
        _addressableService = ServiceLocator.Get<IAddressableService>();
    }
}
```

- 다른 서비스를 참조할 필요가 없다면 `IServiceInit`을 구현하지 않아도 됩니다.

<br>  
<br>

## 서비스 등록 및 호출

- `Resources` 폴더에 `BootstrapConfig`를 생성합니다. 이름을 변경하지 마세요. `Create/PhikozzLib`

<img width="612" height="425" alt="Image" src="https://github.com/user-attachments/assets/cd5e7405-d452-4ce7-b0e6-96c3ea82770e" />

- 원하는 서비스 프리팹을 등록합니다.

```csharp
private IFloatingTextService _floatingTextService;
        
private void Start()
{
    _floatingTextService = ServiceLocator.Get<IFloatingTextService>();
}
```
- `ServiceLocater.Get<T>()` 메서드를 호출해 해당 서비스 객체를 캐싱할 수 있습니다.
- `Bootstrapper`가 생성하는 서비스가 아닌, 씬에 배치된 오브젝트에서 참조할 때는 `Awake()`가 아닌 `Start()`(또는 그 이후 시점)에서 호출하세요. `Awake()` 시점에는 `Bootstrapper`의 서비스 등록이 끝나지 않았을 수 있습니다.
