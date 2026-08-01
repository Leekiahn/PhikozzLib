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

## 서비스 등록

- [BootstrapConfigEditorWindow](https://github.com/Leekiahn/PhikozzLib/tree/main/Editor/BootstrapConfigEditorWindow)
- 서비스 등록은 위 문서를 참고하세요.


<br>  
<br>
    

```csharp
public IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
        
public IAudioService Audio => ServiceLocator.Get<IAudioService>();
  
public IUIService UI => ServiceLocator.Get<IUIService>();
```

`ServiceLocater.Get<T>()` 메서드를 호출해 해당 서비스 객체를 캐싱할 수 있습니다.
