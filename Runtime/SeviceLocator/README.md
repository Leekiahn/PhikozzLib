# ServiceLocater
해당 서비스 로케이터는 서비스 등록을 위해 IServiceRegister 인터페이스를 제공합니다.

<br>

1. 원하는 서비스 클래스에 **IServiceRegister** 인터페이스를 상속합니다.
2. **RegisterService()** 메서드를 구현하고 **ServiceLocator.Register<T>(T service)** 메서드를 호출해 서비스를 등록합니다.
3. 서비스 프리팹을 **BootstrapConfig**에 등록하고 BootstrapConfig는 Resources 폴더에 배치합니다.

플레이 시, 자동으로 씬에 해당 서비스가 생성됩니다.

```
public void RegisterService()
{
    ServiceLocator.Register<IEffectService>(this);
}
```

<br>

IServiceRegister 인터페이스를 상속받아 등록된 서비스는 기본적으로 DontDestroyOnLoad 속성을 가지고 있습니다.

모든 씬에서 전역적으로 사용하는 서비스를 등록하세요.

<br>

ServiceLocater.Get<T>() 메서드를 호출해 해당 서비스를 캐싱할 수 있습니다.
```
public IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
        
public IAudioService Audio => ServiceLocator.Get<IAudioService>();
  
public IUIService UI => ServiceLocator.Get<IUIService>();
```
