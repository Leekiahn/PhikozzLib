# SceneLoadManager

<img width="603" height="534" alt="Image" src="https://github.com/user-attachments/assets/0ed71238-0000-4248-bf96-33095dae842c" />

> 일반·Additive·로딩 화면 포함 씬 전환과 씬 프리로드를 제공하는 서비스입니다. (`ISceneService`)

## Public API

| Method | Description |
|---|---|
| `LoadScene(string sceneName)` | 씬을 로드합니다. |
| `LoadSceneWithLoading(string sceneName, string loadingSceneName)` | 로딩 씬을 거쳐 전환합니다. |
| `LoadAdditiveScene(string sceneName)` | 씬을 Additive로 로드합니다. |
| `LoadAdditiveSceneWithLoading(string sceneName, string loadingSceneName)` | 로딩 씬을 거쳐 Additive로 전환합니다. |
| `LoadSceneAsync(string sceneName)` / `LoadAdditiveSceneAsync(string sceneName)` | 비동기로 로드하고 `AsyncOperation`을 반환합니다. |
| `PreloadSceneAsync(string sceneName)` | 씬을 미리 로드만 해두고 활성화는 하지 않습니다. |
| `GetPreloadedSceneHandle()` | 프리로드 중인 `AsyncOperation`을 반환합니다(진행률 표시용). |
| `ActivatePreloadedScene()` | 프리로드한 씬을 활성화하고 이전 씬을 언로드합니다. |
| `UnloadSceneAsync(string sceneName)` | 씬을 언로드합니다. |
| `SetHold(eSceneLoadingHoldMode holdMode, bool status)` | Additive 로딩 전환을 지정한 단계에서 멈추거나 재개합니다. |

## 사용 예시

```csharp
private ISceneService _sceneService;

private void Start()
{
    _sceneService = ServiceLocator.Get<ISceneService>();
}

public void OnClickStart()
{
    _sceneService.LoadSceneWithLoading("Game", "Loading");
}

// 미리 로드해두고 원하는 시점에 전환
public void PrepareBossStage()
{
    _sceneService.PreloadSceneAsync("Boss");
}

public void EnterBossStage()
{
    _sceneService.ActivatePreloadedScene();
}
```

## 주의사항

- 로딩 씬 전환은 Feel의 `MMSceneLoadingManager` / `MMAdditiveSceneLoadingManager`를 사용합니다. Additive 전환 설정은 인스펙터의 `Settings`에서 합니다.
- 프리로드는 한 번에 하나만 유지됩니다. 활성화하기 전에 다른 씬을 프리로드하면 이전 핸들은 덮어씁니다.
