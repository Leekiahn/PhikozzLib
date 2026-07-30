# Bootstrap
`BootstrapConfig`에 등록된 서비스들은 씬이 시작되기 전, 자동으로 초기화되며  
`DontDestroyOnLoad` 속성을 가지고 씬에 생성됩니다.

# 주의사항
- `Bootstrapper.cs`는 `Resources`폴더에 존재하는 `BootstrapConfig`를 로드합니다.  
`BootstrapConfig`를 꼭 `Resources`에 배치해주세요.
- `BootstrapConfig`에 등록되는 서비스들은 모두 `IServiceRegister` 인터페이스를 상속받고  
내부에 `ServiceLocator.Register<T>(this)`를 호출해야 합니다.
