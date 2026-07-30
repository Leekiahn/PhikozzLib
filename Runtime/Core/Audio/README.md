# Core.Audio
- `AudioManager`는 PhikozzLib에서 **BGM**, **SFX**, **UI 사운드**, **트랙 제어**, **페이드 처리**를 담당하는 오디오 서비스입니다.  
- `IAudioService` 인터페이스를 상속받아 구현합니다.
- `AudioManager`는 MoreMountains의 Feel 에셋에 의존하고 있습니다. Feel 에셋에 의존하지 않는 다른 매니저로 교체가 가능합니다.

<br>

## Overview
- 채널 기반 BGM 재생
- SFX/UI 사운드 이름 기반 재생
- 특정 트랙 음소거, 정지, 재생, 볼륨 제어
- 전체 사운드 일괄 제어
- 트랙 페이드 인/아웃
- 오디오 데이터베이스(`PlaylistDatabase`, `SoundDatabase`) 연결

<br>

## Features

- **라벨 캐싱**: 라벨별 `Location`, `Handle`, `Asset` 캐시 관리
- **위치 프리로드**: `LoadResourceLocationsAsync`로 에셋 위치 선조회
- **에셋 일괄 프리로드**: 라벨에 포함된 모든 에셋 비동기 로드
- **키 기반 조회**: `PrimaryKey` 기준 에셋 접근
- **다운로드 지원**: 원격 Addressable dependency 다운로드
- **메모리 해제 지원**: 개별 해제 및 라벨 전체 해제

<br>

## Public API

| Method | Description |
|-----|-----|
|  `DownloadDependencies(string label)` | 라벨의 dependency 다운로드     |
| `PreloadLocations<T>(string label)` |  라벨에 속한 에셋 위치 캐시      |
| ` PreloadAssets<T>(string label) ` |  라벨의 에셋 전체 프리로드     |
| ` IsLoaded(string label, string key) ` |  특정 에셋 로드 여부 확인    |
| ` ContainsLabel(string label) ` |  라벨 캐시 존재 여부 확인  |
| ` Get<T>(string label, string key) ` |  키로 단일 에셋 조회  |
| ` GetAll<T>(string label) `|  라벨 내 로드된 에셋 전체 조회 |
| ` Release(string label, string key) ` |  특정 에셋 해제 |
| ` ReleaseAll(string label) ` |  해당 라벨의 모든 에셋 해제 |
