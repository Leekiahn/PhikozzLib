# ServiceLocater
해당 서비스 로케이터는 서비스 등록을 위해 IServiceRegister 인터페이스를 제공합니다.

1. 원하는 서비스 클래스에 **IServiceRegister** 인터페이스를 상속합니다.
2. <span style="color:orange">RegisterService()</span> 메서드를 구현하고 **ServiceLocator.Register<T>(T service)** 메서드를 호출해 서비스를 등록합니다.
3. 서비스 프리팹을 **BootstrapConfig**에 등록해 Resources 폴더에 배치합니다.

