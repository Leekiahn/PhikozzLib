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

<img width="444" height="159" alt="Image" src="https://github.com/user-attachments/assets/f615353a-8819-4858-b67b-0bc9667858ce" />  

- `Resources` 폴더에 `BootstrapConfig`를 생성합니다. 이름을 변경하지 마세요.

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

<br>
<br>

## Editor에서 Reload Domain을 끄고 반복 재생하는 경우

`ServiceLocator._services`는 `static`이라, Unity 에디터의 **Enter Play Mode Options**에서 "Reload Domain"을 꺼두면 플레이 세션 사이에 초기화되지 않습니다. 등록된 서비스를 인터페이스 타입으로 들고 있으면 죽은 오브젝트인지 감지가 안 되기 때문에(파괴된 오브젝트가 `== null`로 안 잡힘), 등록 목록이 이전 세션 기준으로 꼬일 수 있습니다.

`ServiceLocator`는 `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]`으로 매 플레이 세션 시작 시 `_services`를 강제로 비웁니다. 이 시점은 `Bootstrapper`(`BeforeSceneLoad`)보다 항상 먼저 실행되므로, 리셋 이후 재등록되는 흐름이 항상 보장됩니다.
