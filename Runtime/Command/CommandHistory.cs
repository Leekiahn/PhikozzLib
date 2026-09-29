using UnityEngine;
using System.Collections.Generic;

public class CommandHistory
{
    private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
    private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
