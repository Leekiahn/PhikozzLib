# Generic StateMachine

> 상태 전환과 상태 갱신을 위한 제네릭 상태 머신 유틸리티를 제공합니다.

---

## 주요 기능

- 제네릭 기반 상태 머신 구현
- 상태 등록 및 전환
- 현재 상태의 `Enter / Tick / Exit` 생명주기 관리
- `BaseState<T>`를 통한 공통 상태 베이스 제공
- 간단하고 확장 가능한 구조

---

## Public API

### IState

| Method | Description |
|---|---|
| `Enter()` | 상태 진입 시 호출됩니다. |
| `Tick()` | 상태 갱신 시 호출됩니다. |
| `Exit()` | 상태 종료 시 호출됩니다. |

---

### BaseState<TOwner>

| Member | Description |
|---|---|
| `Owner` | 상태를 소유한 객체입니다. |
| `StateMachine` | 연결된 상태 머신입니다. |
| `Enter()` | 진입 시 호출되는 기본 메서드입니다. |
| `Tick()` | 갱신 시 호출되는 기본 메서드입니다. |
| `Exit()` | 종료 시 호출되는 기본 메서드입니다. |

---

### StateMachine<TOwner>

| Method | Description |
|---|---|
| `AddState(BaseState<TOwner> state)` | 상태를 등록합니다. |
| `ChangeState<TState>()` | 지정한 상태로 전환합니다. |
| `Tick()` | 현재 상태의 `Tick()`을 호출합니다. |

| Member | Description |
|---|---|
| `CurrentState` | 현재 활성 상태입니다. |

---

## 사용 예시

```csharp
using PhikozzLib;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private StateMachine<PlayerController> _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine<PlayerController>(this);
        _stateMachine.AddState(new IdleState(this, _stateMachine));
        _stateMachine.AddState(new RunState(this, _stateMachine));
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}
```

```csharp
using PhikozzLib;

public class IdleState : BaseState<PlayerController>
{
    public IdleState(PlayerController owner, StateMachine<PlayerController> stateMachine)
        : base(owner, stateMachine)
    {
    }

    public override void Enter()
    {
        UnityEngine.Debug.Log("Idle Enter");
    }

    public override void Tick()
    {
        UnityEngine.Debug.Log("Idle Tick");
    }

    public override void Exit()
    {
        UnityEngine.Debug.Log("Idle Exit");
    }
}
```

---

## 동작 방식

- `AddState()`로 상태를 먼저 등록합니다.
- `ChangeState<TState>()`를 호출하면:
  - 현재 상태가 있으면 `Exit()`
  - 새 상태를 찾으면 `CurrentState` 교체
  - 새 상태의 `Enter()` 호출
- `Tick()`은 매 프레임 현재 상태에 위임합니다.
