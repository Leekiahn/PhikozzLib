using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "PhikozzLib/BootstrapConfig", order = 0)]
    public class BootstrapConfig : ScriptableObject
    {
        [TableList(AlwaysExpanded = true)]
        [SerializeField] private List<GameObject> _managers;

        public List<GameObject> Managers => _managers;
    }
}
