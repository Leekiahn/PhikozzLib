# Core.Addressable
- Unity Addressables를 라벨 단위로 캐싱하고 필요한 에셋을 프리로드/조회/해제하는 서비스입니다.
- `IAddressableService` 인터페이스를 상속받아 구현합니다.

<br>

## Overview
- 라벨에 포함된 에셋 위치를 미리 조회
- 라벨에 속한 에셋을 한 번에 미리 로드
- `key` 기준으로 개별 에셋 조회 혹은 라벨 에셋 전체 조회
- 로드 상태 확인
- 개별 또는 전체 에셋 해제

## Features

- **라벨 캐싱**: 라벨별 `Location`, `Handle`, `Asset` 캐시 관리
- **위치 프리로드**: `LoadResourceLocationsAsync`로 에셋 위치 선조회
- **에셋 일괄 프리로드**: 라벨에 포함된 모든 에셋 비동기 로드
- **키 기반 조회**: `PrimaryKey` 기준 에셋 접근
- **다운로드 지원**: 원격 Addressable dependency 다운로드
- **메모리 해제 지원**: 개별 해제 및 라벨 전체 해제


| Method | Description |
|  `DownloadDependencies(string label)` | 라벨의 dependency 다운로드 |
|  |  |
|  |  |
|  |  |
|  |  |
|  |  |
