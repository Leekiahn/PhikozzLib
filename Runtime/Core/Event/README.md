# Core.Event
- 이벤트 구조체의 등록/발행 서비스를 제공합니다.
- `IEventService` 인터페이스를 상속받아 구현합니다

<br>

## Overview

- 제네릭 구조체 이벤트 타입별 핸들러 등록
- 이벤트 구독(Subscribe) / 구독 해제(Unsubscribe)
- 이벤트 발행(Publish)
- 전체 이벤트 핸들러 일괄 제거

<br>

## Features

- 타입 기반 이벤트: 각 이벤트를 구조체 타입으로 정의해 타입 안전성 제공
- 다중 구독: 하나의 이벤트에 여러 핸들러 등록 가능
- 자동 등록/해제: Delegate.Combine/Remove로 안전한 추가/제거
- 메모리 효율: 핸들러가 없으면 딕셔너리 항목 제거
- 빠른 발행: 타입 기반 딕셔너리 조회로 O(1) 성능
- 전역 접근: Core.Event로 어디서든 이벤트 접근 가능

<br>

## Public API

| Method | Description |
|-----|-----|
| `Subscribe<T>(Action<T> handler)` | 이벤트 타입 T에 핸들러 등록 |
| ` Unsubscribe<T>(Action<T> handler) `|  이벤트 타입 T의 핸들러 제거  |
| ` Publish<T>(T evt) ` |  이벤트 타입 T를 발행하고 등록된 핸들러 호출  |
| ` Clear() ` |  모든 핸들러 일괄 제거 |

