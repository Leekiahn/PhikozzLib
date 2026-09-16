# EventManager

> 이벤트 구조체(struct)의 등록/발행 서비스를 제공합니다.  
> `IEventService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- 제네릭 구조체 이벤트 타입별 핸들러 등록
- 이벤트 구독(Subscribe) / 구독 해제(Unsubscribe)
- 이벤트 발행(Publish)
- `Component`를 넘겨서 구독하면, 그 오브젝트가 파괴될 때 자동으로 구독 해제
- 전체 이벤트 핸들러 일괄 제거

<br>

## Public API

| Method | Description |
|---|---|
| `Subscribe<T>(Action<T> handler)` | 이벤트 타입 `T`에 핸들러를 등록합니다. 구독 해제는 직접 `Unsubscribe`를 호출해야 합니다. |
| `Subscribe<T>(Component owner, Action<T> handler)` | 핸들러를 등록하고, `owner`가 파괴될 때(`OnDestroy`) 자동으로 구독 해제되도록 합니다. |
| `Unsubscribe<T>(Action<T> handler)` | 이벤트 타입 `T`의 핸들러를 제거합니다. |
| `Publish<T>(T evt)` | 이벤트 타입 `T`를 발행하고 등록된 핸들러를 호출합니다. |
| `Clear()` | 모든 핸들러를 일괄 제거합니다. |

<br>

## 왜 `Subscribe(Component, Action<T>)`가 있는가

`MonoBehaviour`가 `Subscribe`만 하고 `OnDestroy`에서 `Unsubscribe`를 깜빡하면, 파괴된 오브젝트를 향한 델리게이트가 `_handlers`에 계속 쌓입니다. 그러면 그 이벤트가 발행될 때마다 죽은 오브젝트의 메서드를 호출하려다 예외가 나거나, 메모리에서 안 풀립니다.

`Subscribe<T>(Component owner, Action<T> handler)`는 내부적으로 `owner`의 GameObject에 자동 구독 해제용 컴포넌트(`EventUnsubscriber`)를 붙여뒀다가, `owner`가 파괴되면 등록해둔 모든 구독을 자동으로 해제합니다.

**한계**: `owner`는 GameObject를 가진 `Component`여야 합니다 — `BaseState<TOwner>`처럼 GameObject가 없는 순수 C# 클래스나 `ScriptableObject`(Component가 아님)에는 쓸 수 없습니다. 그런 경우엔 `Subscribe<T>(Action<T>)`/`Unsubscribe<T>(Action<T>)`를 수동으로 대칭 호출해야 합니다.

<br>

## 사용 예시

```csharp
public struct PlayerDiedEvent
{
    public int PlayerId;
}
```

**자동 구독 해제 (일반적인 경우 — 권장)**

```csharp
private IEventService _eventService;

private void Start()
{
    _eventService = ServiceLocator.Get<IEventService>();
    _eventService.Subscribe<PlayerDiedEvent>(this, OnPlayerDied);
}

private void OnPlayerDied(PlayerDiedEvent evt)
{
    Debug.Log($"Player {evt.PlayerId} died");
}
```

**수동 구독 해제 (Component가 아닌 대상 — 예: State 클래스)**

```csharp
public class DeadState : BaseState<PlayerController>
{
    private IEventService _eventService;

    public override void Enter()
    {
        _eventService = ServiceLocator.Get<IEventService>();
        _eventService.Subscribe<PlayerDiedEvent>(OnPlayerDied);
    }

    public override void Exit()
    {
        _eventService.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
    }

    private void OnPlayerDied(PlayerDiedEvent evt) { ... }
}
```

**발행**

```csharp
_eventService.Publish(new PlayerDiedEvent { PlayerId = 1 });
```
