using System;
using UnityEngine;

namespace PhikozzLib
{
    public class InputManager : MonoBehaviour, IServiceRegister
    {
        private PlayerInputAction _actionMaps;

        private void Awake()
        {
            _actionMaps = new PlayerInputAction();
        }

        public void RegisterService()
        {
            ServiceLocator.Register(this);
        }
    }
}

