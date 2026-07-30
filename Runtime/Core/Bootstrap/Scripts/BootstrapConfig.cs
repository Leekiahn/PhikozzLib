using UnityEngine;
using System.Collections.Generic;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "PhikozzLib/BootstrapConfig", order = 0)]
    public class BootstrapConfig : ScriptableObject
    {
        [SerializeField] private List<GameObject> _managers;

        public List<GameObject> Managers => _managers;
    }
}
