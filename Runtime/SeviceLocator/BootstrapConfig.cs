using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BootstrapConfig", menuName = "PhikozzLib/BootstrapConfig", order = 0)]
public class BootstrapConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> _managers;
    
    public IReadOnlyList<GameObject> Managers => _managers;
}
