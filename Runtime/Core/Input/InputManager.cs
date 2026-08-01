using System;
using UnityEngine;

namespace PhikozzLib
{
    public class InputManager : MonoBehaviour, IServiceRegister
    {
        public PlayerInputAction ActionMaps { get; private set; }

        private void Awake()
        {
            ActionMaps = new PlayerInputAction();
        }

        public void RegisterService()
        {
            ServiceLocator.Register(this);
        }

        // ActionMaps의 액션을 사용하여 입력을 처리하는 메서드를 아래와 같이 추가할 수 있습니다.
        // public Vector2 Move()
        // {
        //     return ActionMaps.Player.Move.ReadValue<Vector2>();
        // }
    }
}

