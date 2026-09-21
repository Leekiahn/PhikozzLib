# AddressableManager

> Unity Addressables를 라벨 단위로 캐싱하고 필요한 에셋을 프리로드/조회/해제하는 서비스입니다.  
> `IAddressableService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- 라벨에 포함된 에셋 위치를 미리 조회 (`PreloadLocations`)
- 라벨에 속한 에셋을 한 번에 미리 로드 (`PreloadAssets`)
- `key` 기준으로 개별 에셋 조회 혹은 라벨 에셋 전체 조회
- 로드 상태 확인
- 개별 또는 라벨 단위 에셋 해제

<br>

## Public API

| Method | Description |
|-----|-----|
| `DownloadDependencies(string label)` | 라벨의 dependency(원격 번들 등)를 디스크에 다운로드합니다. 이미 캐시돼 있으면 스킵합니다. |
| `PreloadLocations<T>(string label)` | 라벨에 속한 에셋의 위치(메타데이터)만 조회해서 캐시합니다. 실제 에셋을 메모리에 올리지는 않습니다. |
| `PreloadAssets<T>(string label)` | 라벨에 속한 에셋을 실제로 로드합니다. 위치가 아직 캐시 안 됐으면 `PreloadLocations`를 먼저 내부적으로 호출합니다. |
| `IsLoadedAssetKey(string label, string key)` | 특정 에셋이 로드됐는지 확인합니다. |
| `IsCachedLabel(string label)` | 해당 라벨이 프리로드된 적 있는지 확인합니다. |
| `Get<T>(string label, string key)` | 키로 로드된 에셋을 조회합니다. 라벨이 프리로드된 적 없으면 예외를 던집니다. |
| `GetAll<T>(string label)` | 라벨 내 로드된 에셋 전체를 조회합니다. 라벨이 프리로드된 적 없으면 예외를 던집니다. |
| `GetAllWithKeys<T>(string label)` | Addressable key와 에셋을 함께 조회합니다. 에셋 이름 변경에 영향받지 않는 등록 key가 필요할 때 사용합니다. |
| `Release(string label, string key)` | 특정 에셋을 해제합니다(핸들 반환 + 캐시에서 제거). 프리로드된 적 없는 라벨이면 조용히 아무 일도 하지 않습니다. |
| `ReleaseAll(string label)` | 해당 라벨의 로드된 에셋을 전부 해제합니다. 프리로드된 적 없는 라벨이면 조용히 아무 일도 하지 않습니다. |

<br>

## 세 단계로 나뉜 이유

`DownloadDependencies`(디스크 다운로드) → `PreloadLocations`(위치 메타데이터 조회) → `PreloadAssets`(실제 메모리 로드) 세 단계는 관심사가 다릅니다.

- 라벨에 뭐가 몇 개 있는지만 알고 싶을 때는 `PreloadLocations`만으로 충분하고, 전부 메모리에 올릴 필요가 없습니다.
- 원격 콘텐츠를 쓰는 프로젝트라면 로딩 화면에서 `DownloadDependencies`로 다운로드 진행률을 따로 보여준 뒤, 실제 로드는 나중에 `PreloadAssets`로 진행할 수 있습니다.

<br>

## `ReleaseAll` 이후에도 라벨 자체는 캐시에 남습니다

`ReleaseAll(label)`은 로드된 에셋과 핸들만 해제하고, 라벨의 위치 목록(`LocationsHandle`/`LocationByKey`)은 그대로 유지합니다. 이건 의도된 동작입니다 — 실제 에셋 인스턴스는 메모리에서 내리되, "이 라벨에 어떤 키들이 있는지"는 다시 조회하지 않고 재사용해서, 나중에 `PreloadAssets`를 다시 호출했을 때 위치 조회 없이 바로 로드부터 시작할 수 있게 하기 위함입니다.

<br>

## `Get`/`GetAll` vs `Release`/`ReleaseAll`의 실패 처리 차이

- `Get`/`GetAll`은 프리로드 안 된 라벨을 조회하면 **명확한 메시지와 함께 예외를 던집니다** ("Label 'X' has not been preloaded. Call PreloadLocations first.") — 존재해야 할 걸 조회하는 것이므로 호출 순서를 잘못 짠 실수를 바로 드러내는 게 낫습니다.
- `Release`/`ReleaseAll`은 프리로드 안 된 라벨을 넘기면 **조용히 아무 일도 하지 않습니다** — 정리(cleanup) 성격의 메서드는 "지울 게 없으면 그냥 넘어간다"는 게 자연스럽고, 매번 `IsCachedLabel`로 먼저 확인하고 부르게 강제할 필요가 없습니다.

<br>

## 사용 예시

```csharp
private IAddressableService _addressableService;

private async UniTaskVoid LoadUISprites()
{
    _addressableService = ServiceLocator.Get<IAddressableService>();

    await _addressableService.DownloadDependencies("UI_Sprites");
    await _addressableService.PreloadAssets<Sprite>("UI_Sprites");

    var icon = _addressableService.Get<Sprite>("UI_Sprites", "coin_icon");
}

private void OnLevelUnload()
{
    _addressableService.ReleaseAll("UI_Sprites");
}
```
