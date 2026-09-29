using UnityEngine;

public interface ICommand
{
    void Execute();
    void Undo();
    void Clear();
}
