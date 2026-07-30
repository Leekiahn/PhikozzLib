# Core.UI
- **Window**와 **Overlay** UI를 Addressables 기반으로 프리로드하고, 타입 기준으로 **열기**, **닫기**, **재사용**을 관리하는 UI 서비스입니다.
- `IUIService` 인터페이스를 상속받아 구현합니다.

<br>

## Overview
- Window 프리팹 프리로드
- Overlay 프리팹 프리로드
- 타입 기준 UI 인스턴스 생성 및 캐싱
- 이미 생성된 UI 재사용
- Window / Overlay 개별 닫기 및 전체 닫기

<br>

## Features

- **Addressables 연동**: 라벨 기반으로 Window/Overlay 프리팹을 로드합니다.
- **타입 기반 UI 접근**: `OpenWindow<T>()`, `OpenOverlay<T>()` 형태로 사용합니다.
- **인스턴스 재사용**: 이미 생성된 UI가 있으면 다시 Instantiate 하지 않고 재사용합니다.
- **표시 상태 관리**: `IsVisible` 값으로 현재 표시 여부를 추적합니다.
- **Window / Overlay 분리 구조**: 화면 성격에 따라 두 UI 타입을 구분해 관리합니다.
- **전역 서비스 접근**: `Core.UI`로 어디서든 동일한 방식으로 접근할 수 있습니다.

<br>

## Abstract UI Class

| Type | Role |
| --- | --- |
| `UIBase` | 모든 UI의 공통 추상 베이스 클래스 |
| `UIWindow` | 일반 창형 UI 베이스 클래스 |
| `UIOverlay` | 오버레이형 UI 베이스 클래스 |

<br>

### `UIWindow`

일반적인 팝업/창 형태 UI의 베이스 클래스입니다.

- `Open()` 호출 시 `Refresh()` 후 `OnOpen()`이 실행됩니다.
- `Close()` 호출 시 `OnClose()`가 실행됩니다.
- 기본 동작은 `gameObject.SetActive(true/false)`입니다.

### `UIOverlay`

HUD, 상단 고정 UI, 상태 표시 UI처럼 오버레이 성격의 UI 베이스 클래스입니다.

- `Show()` 호출 시 `Refresh()` 후 `OnShow()`가 실행됩니다.
- `Hide()` 호출 시 `OnHide()`가 실행됩니다.
- 기본 동작은 `gameObject.SetActive(true/false)`입니다.

<br>

## Public API

| Method | Description |
|-----|-----|
| ` LoadWindowPrefabs(string label) ` |  지정한 라벨의 Window 프리팹을 프리로드합니다. |
| ` LoadOverlayPrefabs(string label) `|  지정한 라벨의 Overlay 프리팹을 프리로드합니다. |
| ` OpenWindow<T>() ` |  타입에 해당하는 Window를 열고, 이미 생성된 인스턴스가 있으면 재사용합니다. |
| ` CloseWindow<T>() ` |  타입에 해당하는 Window를 닫습니다. |
| ` CloseWindow(UIWindow window) `|  전달한 Window 인스턴스를 기준으로 닫습니다. |
| ` CloseAllWindow() `|  현재 열린 모든 Window를 닫습니다. |
| ` OpenOverlay<T>() `|  타입에 해당하는 Overlay를 열고, 이미 생성된 인스턴스가 있으면 재사용합니다. |
| ` CloseOverlay<T>() `|  타입에 해당하는 Overlay를 닫습니다. |
| `  CloseOverlay(UIOverlay overlay) ` |  전달한 Overlay 인스턴스를 기준으로 닫습니다. |
| ` CloseAllOverlay() `|  현재 열린 모든 Overlay를 닫습니다. |


<br>

## Setup

| Step | Description |
|-----|-----|
| 1 |  Window 프리팹에 UIWindow 상속 클래스를 붙입니다.  |
| 2 |  Overlay 프리팹에 UIOverlay 상속 클래스를 붙입니다.  |
| 3 |  각 프리팹을 Addressables에 등록합니다.  |
| 4 |  Window용 라벨과 Overlay용 라벨을 설정합니다.  |
| 5 |  `UIManager`의 `windowLabelReference`, `overlayLabelReference`를 연결합니다. |
| 6 |  `UIManager`의 `windowParent`, `overlayParent`를 연결합니다.  |
| 7 |   코드에서 Core.UI로 접근해 사용합니다. |
