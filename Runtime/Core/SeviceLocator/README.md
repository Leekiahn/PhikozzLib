# ServiceLocater
- 해당 서비스 로케이터는 전역적인 공통 시스템 서비스 등록을 위해 IServiceRegister 인터페이스를 제공합니다.

<br>

## IServiceRegister

```csharp
public void RegisterService()
{
    ServiceLocator.Register<IEffectService>(this);
}
```

- 원하는 서비스 클래스에 `IServiceRegister` 인터페이스를 상속합니다.
- `RegisterService()` 메서드 안에 `ServiceLocator.Register<T>(T service)` 메서드를 호출해 서비스를 등록합니다.

<br>  
<br>

## 서비스 등록 및 호출

<img width="444" height="159" alt="Image" src="https://github.com/user-attachments/assets/f615353a-8819-4858-b67b-0bc9667858ce" />  

- `Resources` 폴더에 `BootstrapConfig`를 생성합니다. 이름을 변경하지 마세요.

<img width="612" height="425" alt="Image" src="https://github.com/user-attachments/assets/cd5e7405-d452-4ce7-b0e6-96c3ea82770e" />

- 원하는 서비스 프리팹을 등록합니다.

```csharp
private IFloatingTextService _floatingTextService;
        
private void Awake()
{
_floatingTextService = ServiceLocator.Get<IFloatingTextService>();
}
```
- `ServiceLocater.Get<T>()` 메서드를 호출해 해당 서비스 객체를 캐싱할 수 있습니다.
