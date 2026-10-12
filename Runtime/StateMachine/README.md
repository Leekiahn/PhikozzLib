# StateMachine

> 소유자(`TOwner`)별 상태를 등록하고 `Enter` / `Tick` / `Exit` 흐름으로 전환하는 제네릭 상태 머신입니다.

## Public API

| Member | Description |
|---|---|
| `StateMachine<TOwner>.AddState(BaseState<TOwner> state)` | 상태를 등록합니다. |
| `StateMachine<TOwner>.ChangeState<TState>()` | 현재 상태 `Exit()` → 새 상태 `Enter()`로 전환합니다. |
| `StateMachine<TOwner>.Tick()` | 현재 상태의 `Tick()`을 호출합니다. |
| `StateMachine<TOwner>.CurrentState` | 현재 상태입니다. |
| `BaseState<TOwner>.Owner` / `.StateMachine` | 소유자와 상태 머신입니다. |
| `BaseState<TOwner>.Enter()` / `Tick()` / `Exit()` | 필요한 것만 override합니다. |

## 사용 예시

```csharp
public class PlayerController : MonoBehaviour
{
    private StateMachine<PlayerController> _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine<PlayerController>();
        _stateMachine.AddState(new IdleState(this, _stateMachine));
        _stateMachine.AddState(new RunState(this, _stateMachine));
        _stateMachine.ChangeState<IdleState>();
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}

public class IdleState : BaseState<PlayerController>
{
    public IdleState(PlayerController owner, StateMachine<PlayerController> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override void Tick()
    {
        if (Owner.HasMoveInput)
        {
            StateMachine.ChangeState<RunState>();
        }
    }
}
```

## 주의사항

- 등록하지 않은 상태로 `ChangeState`하면 경고 로그만 남기고 현재 상태를 유지합니다.
- 상태 클래스는 흐름 제어만 하고, 실제 로직은 `Owner`의 메서드로 위임합니다.
