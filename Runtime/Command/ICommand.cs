using UnityEngine;

namespace PhikozzLib
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
