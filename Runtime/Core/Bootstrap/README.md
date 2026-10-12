# Bootstrap

<img width="458" height="238" alt="Image" src="https://github.com/user-attachments/assets/0a10ccbc-5ab0-4549-8073-bf75d9628b74" />

> `BootstrapConfig`에 등록한 서비스 프리팹을 씬 로드 전에 생성(`DontDestroyOnLoad`)하고 등록·초기화합니다.

## Public API

| Member | Description |
|---|---|
| `Bootstrapper.WaitUntilReadyAsync()` | 모든 서비스의 등록과 `IServiceInit.InitAsync()`가 끝날 때까지 기다립니다. |
| `BootstrapConfig.Managers` | 생성할 서비스 프리팹 목록입니다. |

## 사용 예시

1. `Create > PhikozzLib`으로 `BootstrapConfig`를 만들어 `Resources` 폴더에 둡니다.
2. `Managers`에 서비스 프리팹을 등록합니다.

```csharp
private async UniTaskVoid Start()
{
    await Bootstrapper.WaitUntilReadyAsync(); // 비동기 초기화(프리로드 등)가 끝난 뒤 진행
    StartGame();
}
```

## 주의사항

- `BootstrapConfig`는 반드시 `Resources/BootstrapConfig.asset` 이름과 경로를 지켜야 합니다.
- 등록 순서: 모든 서비스의 `RegisterService()` → `IServiceInit`을 구현한 서비스의 `InitAsync()` → 준비 완료.
- 다른 서비스를 참조하는 초기화는 `Awake()`가 아니라 `InitAsync()`에 작성합니다.
