# SceneLoadManager

<img width="603" height="534" alt="Image" src="https://github.com/user-attachments/assets/0ed71238-0000-4248-bf96-33095dae842c" />

> 기본 씬 로딩, 비동기 로딩, Additive 로딩, 로딩 화면 연동, 씬 프리로드 및 활성화, Hold 모드 제어를 지원합니다.  
> `ISceneService` 인터페이스를 상속받아 구현합니다.  
> 현재 `MoreMountains`의 `FEEL`에셋에 의존하고 있으며, 인터페이스를 상속받아 의존성이 없는 다른 객체로 교체 가능합니다.
> `Odin Inspector`을 활용해 테스트 기능을 제공합니다.

<br>

## 주요 기능

- 일반 씬 로딩
- 비동기 씬 로딩
- Additive 씬 로딩
- 로딩 씬을 포함한 전환 처리
- 씬 프리로드 후 원하는 시점에 활성화
- 현재 프리로드된 씬 핸들 접근
- 씬 언로드
- Additive Scene Loading Hold 제어
- Service Locator 기반 서비스 등록

<br>

## Public API

| Method | Description |
|-----|-----|
| `LoadScene(string sceneName)`	| 일반 씬 로드 |
| `LoadSceneWithLoading(string sceneName, string loadingSceneName)` |	로딩 씬 포함 전환 |
| `LoadAdditiveScene(string sceneName)` |	Additive 씬 로드 |
| `LoadAdditiveSceneWithLoading(string sceneName, string loadingSceneName)` | 	Additive + 로딩 씬 전환 |
| `LoadSceneAsync(string sceneName)	` | 비동기 씬 로드 |
| `LoadAdditiveSceneAsync(string sceneName)` |	비동기 Additive 로드 |
| `PreloadSceneAsync(string sceneName)` | 씬 프리로드 |
| `GetPreloadedSceneHandle()` |	프리로드 핸들 반환 |
| `ActivatePreloadedScene()`	| 프리로드된 씬 활성화 |
| `UnloadSceneAsync(string sceneName)` | 	씬 언로드 |
| `SetHold(eSceneLoadingHoldMode holdMode, bool status)` | Additive 로딩 Hold 제어 |

