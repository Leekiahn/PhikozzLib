using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class Manager
{
    [SerializeField] private bool _dontDestroyOnLoad;
    [SerializeField] private GameObject _managerPrefab;
    
    public bool DontDestroyOnLoad => _dontDestroyOnLoad;
    public GameObject ManagerPrefab => _managerPrefab;
}

[CreateAssetMenu(fileName = "BootstrapConfig", menuName = "PhikozzLib/BootstrapConfig", order = 0)]
public class BootstrapConfig : ScriptableObject
{
    [TableList(AlwaysExpanded = true)]
    public List<Manager> _managers;

    public IReadOnlyList<Manager> Managers => _managers;
}
