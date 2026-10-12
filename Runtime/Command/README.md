# CommandHistory

> 동작을 커맨드 객체로 기록해 되돌리기(Undo) / 다시 실행(Redo)하는 커맨드 패턴 유틸리티입니다.

## Public API

| Member | Description |
|---|---|
| `ICommand.Execute()` | 동작을 실행합니다. Redo 때도 호출됩니다. |
| `ICommand.Undo()` | `Execute()`로 바꾼 상태를 되돌립니다. |
| `CommandHistory.ExecuteCommand(ICommand command)` | 커맨드를 실행하고 기록합니다. Redo 기록은 비워집니다. |
| `CommandHistory.Undo()` / `Redo()` | 되돌리기 / 다시 실행. 기록이 없으면 아무 일도 하지 않습니다. |
| `CommandHistory.CanUndo` / `CanRedo` | 가능 여부입니다. 버튼 활성화 등에 씁니다. |
| `CommandHistory.Clear()` | 기록을 모두 비웁니다. |

## 사용 예시

```csharp
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
```

## 주의사항

- **되돌릴 때 필요한 이전 값은 생성자가 아니라 `Execute()`에서 저장합니다.**
- 서비스가 아니라 일반 클래스입니다. 기록이 섞이지 않게 필요한 곳마다 `new CommandHistory()`로 따로 가집니다.
- 기록 개수 제한이 없으므로 스테이지 재시작 등에서 `Clear()`합니다.
