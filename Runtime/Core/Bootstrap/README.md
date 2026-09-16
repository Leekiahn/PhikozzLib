# Bootstrap
<img width="458" height="238" alt="Image" src="https://github.com/user-attachments/assets/0a10ccbc-5ab0-4549-8073-bf75d9628b74" />  

`BootstrapConfig`에 등록된 서비스들은 씬이 시작되기 전, 자동으로 초기화되며  
`DontDestroyOnLoad` 속성을 가지고 씬에 생성됩니다.

<br>
<br>

- `Bootstrapper.cs`는 `Resources`폴더에 존재하는 `BootstrapConfig`를 로드합니다.  
`BootstrapConfig`를 꼭 `Resources`에 배치해주세요.
- `BootstrapConfig`에 등록되는 서비스들은 모두 `IServiceRegister` 인터페이스를 상속받고  
내부에 `ServiceLocator.Register<T>(this)`를 호출해야 합니다.
- `Bootstrapper`는 목록에 있는 모든 서비스의 `RegisterService()`를 먼저 전부 호출한 뒤,  
`IServiceInit`을 구현한 서비스에 한해서만 `Init()`을 호출합니다.  
다른 서비스를 참조해야 하는 초기화 로직은 `Awake()`가 아니라 `IServiceInit.Init()`에 작성하세요.
- `Resources`에 `BootstrapConfig.asset`이 없으면 원인 불명의 `NullReferenceException` 대신, 무엇이 없는지 알려주는 명확한 예외를 던집니다.
