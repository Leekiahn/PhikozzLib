using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    [Serializable]
    public class Service
    {
        [SerializeField] private bool _dontDestroyOnLoad;
        [SerializeField] private GameObject _managerPrefab;
    
        public bool DontDestroyOnLoad => _dontDestroyOnLoad;
        public GameObject ManagerPrefab => _managerPrefab;
    }

    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "PhikozzLib/BootstrapConfig", order = 0)]
    public class BootstrapConfig : ScriptableObject
    {
        [DetailedInfoBox("사용에 대한 안내...",
            "해당 BootstrapConfig는 Resources 폴더에 위치해야 하며,\n" +
            "BootStrapper.cs에서 해당 설정을 로드하여 서비스 매니저들을 초기화합니다.")]
    
        [TableList(AlwaysExpanded = true)]
        public List<Service> _managers;

        public IReadOnlyList<Service> Managers => _managers;
    }
}
