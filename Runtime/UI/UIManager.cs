using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class UIManager : MonoBehaviour, IUIService, IServiceRegister
    {
        private Transform _uiRoot;

        public void RegisterService()
        {
            ServiceLocator.Register<IUIService>(this);
            _uiRoot = transform;
        }
    }
}