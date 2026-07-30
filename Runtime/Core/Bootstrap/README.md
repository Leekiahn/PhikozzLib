# Bootstrap
`BootstrapConfig`에 등록된 서비스들은 씬이 시작되기 전, 자동으로 초기화되며  
`DontDestroyOnLoad` 속성을 가지고 씬에 생성됩니다.

# 주의사항
- `Bootstrapper.cs`는 `Resources`폴더에 존재하는 `BootstrapConfig`를 로드합니다.  
`BootstrapConfig`를 꼭 `Resources`에 배치해주세요.
