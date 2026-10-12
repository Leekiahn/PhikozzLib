# EventManager

> 구조체(struct) 이벤트를 타입별로 구독·발행하는 전역 이벤트 서비스입니다. (`IEventService`)

## Public API

| Method | Description |
|---|---|
| `Subscribe<T>(Component owner, Action<T> handler)` | 구독하고, `owner`가 파괴되면 자동으로 해제합니다. (권장) |
| `Subscribe<T>(Action<T> handler)` | 구독합니다. 해제는 직접 `Unsubscribe`해야 합니다. |
| `Unsubscribe<T>(Action<T> handler)` | 구독을 해제합니다. |
| `Publish<T>(T evt)` | 이벤트를 발행합니다. |
| `Clear()` | 모든 구독을 제거합니다. |

## 사용 예시

```csharp
public struct PlayerDiedEvent
{
    public int PlayerId;
}
```

```csharp
private IEventService _eventService;

private void Start()
{
    _eventService = ServiceLocator.Get<IEventService>();
    _eventService.Subscribe<PlayerDiedEvent>(this, OnPlayerDied); // 이 오브젝트가 파괴되면 자동 해제
}

private void OnPlayerDied(PlayerDiedEvent evt)
{
    Debug.Log($"Player {evt.PlayerId} died");
}

// 발행
_eventService.Publish(new PlayerDiedEvent { PlayerId = 1 });
```

## 주의사항

- 자동 해제는 `Component`에만 쓸 수 있습니다. State 클래스나 `ScriptableObject`처럼 GameObject가 없는 곳에서는 `Subscribe(handler)` / `Unsubscribe(handler)`를 짝 맞춰 직접 호출합니다(예: State의 `Enter` / `Exit`).
- 특정 객체의 값 변화(HP, 골드 등)를 구독할 때는 전역 이벤트 대신 `ObservableValue`를 씁니다.
