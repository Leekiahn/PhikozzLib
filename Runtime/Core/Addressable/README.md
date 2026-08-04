# AddressableManager
> Unity Addressables를 라벨 단위로 캐싱하고 필요한 에셋을 프리로드/조회/해제하는 서비스입니다.  
> `IAddressableService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능
- 라벨에 포함된 에셋 위치를 미리 조회
- 라벨에 속한 에셋을 한 번에 미리 로드
- `key` 기준으로 개별 에셋 조회 혹은 라벨 에셋 전체 조회
- 로드 상태 확인
- 개별 또는 전체 에셋 해제

<br>

## Public API

| Method | Description |
|-----|-----|
|  `DownloadDependencies(string label)` | 라벨의 dependency 다운로드     |
| `PreloadLocations<T>(string label)` |  라벨에 속한 에셋 위치 캐시      |
| ` PreloadAssets<T>(string label) ` |  라벨의 에셋 전체 프리로드     |
| ` IsLoadedAssetKey(string label, string key) ` |  특정 에셋 로드 여부 확인    |
| ` IsCachedLabel(string label) ` |  라벨 캐시 존재 여부 확인  |
| ` Get<T>(string label, string key) ` |  키로 단일 에셋 조회  |
| ` GetAll<T>(string label) `|  라벨 내 로드된 에셋 전체 조회 |
| ` Release(string label, string key) ` |  특정 에셋 해제 |
| ` ReleaseAll(string label) ` |  해당 라벨의 모든 에셋 해제 |
