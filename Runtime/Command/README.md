# CommandHistory

> 실행한 동작을 커맨드 객체로 기록해두고 되돌리기(Undo) / 다시 실행(Redo)하는 커맨드 패턴 유틸리티입니다.  
> 퍼즐의 무르기, 건설·배치, 레벨 에디터, 턴제 게임 등에서 사용합니다.

---

## 주요 기능

- 커맨드 실행 및 기록
- 되돌리기(Undo) / 다시 실행(Redo)
- Undo / Redo 가능 여부 조회
- 기록 초기화

---

## 장점

- **되돌리기 구현이 단순해짐**: 동작마다 "실행"과 "되돌리기"를 한 클래스에 짝지어 두므로, 기록 관리는 `CommandHistory`가 맡고 각 커맨드는 자기 동작만 신경 쓰면 됩니다.
- **동작 추가가 쉬움**: 새 동작은 `ICommand`를 구현한 클래스 하나만 추가하면 되고, `CommandHistory`는 수정할 필요가 없습니다.
- **호출하는 쪽과 실행 로직 분리**: 버튼, 단축키 등 호출하는 쪽은 커맨드를 만들어 넘기기만 하고, 실제 동작이 어떻게 수행되는지 몰라도 됩니다.

---

## Public API

### ICommand

| Method | Description |
|---|---|
| `Execute()` | 동작을 실행합니다. Redo 시에도 다시 호출됩니다. |
| `Undo()` | `Execute()`로 바꾼 상태를 되돌립니다. |

### CommandHistory

| Member | Description |
|---|---|
| `ExecuteCommand(ICommand command)` | 커맨드를 실행하고 Undo 기록에 쌓습니다. Redo 기록은 비워집니다. |
| `Undo()` | 가장 최근 커맨드를 되돌리고 Redo 기록으로 옮깁니다. 되돌릴 기록이 없으면 아무 일도 하지 않습니다. |
| `Redo()` | 가장 최근에 되돌린 커맨드를 다시 실행하고 Undo 기록으로 옮깁니다. 다시 실행할 기록이 없으면 아무 일도 하지 않습니다. |
| `Clear()` | Undo / Redo 기록을 모두 비웁니다. 스테이지 재시작 등에 사용합니다. |
| `CanUndo` | 되돌릴 기록이 있는지 여부입니다. Undo 버튼 활성화 등에 사용합니다. |
| `CanRedo` | 다시 실행할 기록이 있는지 여부입니다. |

---

## 사용 예시

```csharp
using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly Transform _target;
    private readonly Vector3 _to;
    private Vector3 _from;

    public MoveCommand(Transform target, Vector3 to)
    {
        _target = target;
        _to = to;
    }

    public void Execute()
    {
        _from = _target.position;
        _target.position = _to;
    }

    public void Undo()
    {
        _target.position = _from;
    }
}
```

```csharp
private readonly CommandHistory _history = new();

public void MovePiece(Transform piece, Vector3 targetPosition)
{
    _history.ExecuteCommand(new MoveCommand(piece, targetPosition));
}

public void OnClickUndo()
{
    _history.Undo();
}

public void OnClickRedo()
{
    _history.Redo();
}

public void RestartStage()
{
    _history.Clear();
}
```

---

## 주의사항

- **`CommandHistory`는 서비스가 아니라 일반 C# 클래스입니다.** 퍼즐 보드, 에디터 등 되돌리기 기록이 필요한 곳마다 각자 `new CommandHistory()`로 가지고 씁니다. 하나를 여러 곳에서 공유하면 서로 다른 맥락의 기록이 섞입니다.
- **되돌릴 때 필요한 이전 값은 `Execute()`에서 저장합니다.** 생성자에서 저장하면 커맨드를 만든 시점과 실행한 시점 사이에 상태가 바뀌었을 때 잘못된 값으로 되돌아갑니다.
- **새 커맨드를 실행하면 Redo 기록은 사라집니다.** 되돌린 뒤 다른 동작을 하면 이전에 되돌렸던 동작은 다시 실행할 수 없습니다.
- 기록 개수에 제한이 없으므로, 커맨드가 아주 많이 쌓이는 경우에는 적절한 시점(스테이지 재시작 등)에 `Clear()`로 기록을 비웁니다.
