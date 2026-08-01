# Core.Event
- 이벤트 구조체의 등록/발행 서비스를 제공합니다.
- `IEventService` 인터페이스를 상속받아 구현합니다

<br>

## 주요 기능

- 제네릭 구조체 이벤트 타입별 핸들러 등록
- 이벤트 구독(Subscribe) / 구독 해제(Unsubscribe)
- 이벤트 발행(Publish)
- 전체 이벤트 핸들러 일괄 제거

<br>

## Public API

| Method | Description |
|-----|-----|
| `Subscribe<T>(Action<T> handler)` | 이벤트 타입 T에 핸들러 등록 |
| ` Unsubscribe<T>(Action<T> handler) `|  이벤트 타입 T의 핸들러 제거  |
| ` Publish<T>(T evt) ` |  이벤트 타입 T를 발행하고 등록된 핸들러 호출  |
| ` Clear() ` |  모든 핸들러 일괄 제거 |

