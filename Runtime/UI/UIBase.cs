using UnityEngine;

namespace PhikozzLib
{
    public abstract class UIBase : MonoBehaviour
    {
        public bool IsVisible { get; protected set; }
        
        public abstract void Refresh();
    }
}