# AddressableManager

> Addressable 에셋을 라벨 단위로 프리로드·캐싱·조회·해제하는 서비스입니다. (`IAddressableService`)

## Public API

| Method | Description |
|---|---|
| `DownloadDependencies(string label)` | 라벨의 원격 번들을 다운로드합니다. 이미 캐시돼 있으면 건너뜁니다. |
| `PreloadLocations<T>(string label)` | 라벨의 에셋 위치 정보만 조회해서 캐시합니다. |
| `PreloadAssets<T>(string label)` | 라벨의 에셋을 메모리에 로드합니다. |
| `IsLoadedAssetKey(string label, string key)` | 해당 에셋이 로드됐는지 확인합니다. |
| `IsCachedLabel(string label)` | 해당 라벨이 프리로드됐는지 확인합니다. |
| `Get<T>(string label, string key)` | 로드된 에셋 하나를 조회합니다. |
| `GetAll<T>(string label)` | 라벨의 로드된 에셋 전체를 리스트로 조회합니다. |
| `GetAllWithKeys<T>(string label)` | 라벨의 로드된 에셋 전체를 `key → 에셋` Dictionary로 조회합니다. |
| `Release(string label, string key)` | 에셋 하나를 해제합니다. |
| `ReleaseAll(string label)` | 라벨의 에셋을 모두 해제합니다. |

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

## 주의사항

- **`key`는 에셋 이름이 아니라 Addressable 주소(Address)입니다.** 주소를 바꾸지 않았다면 `Assets/.../coin_icon.png` 같은 전체 경로가 key입니다.
- `Get` / `GetAll` / `GetAllWithKeys`는 프리로드되지 않은 라벨이면 예외를 던집니다. `Release` / `ReleaseAll`은 아무 일도 하지 않습니다.
- `GetAll` / `GetAllWithKeys`는 호출할 때마다 새 컬렉션을 만듭니다. 자주 호출하는 곳에서는 결과를 받아서 들고 씁니다.
- `ReleaseAll` 후에도 라벨의 위치 정보는 남아 있어서, 다시 `PreloadAssets`를 호출하면 위치 조회 없이 바로 로드합니다.
