# Core.Input

> 입력 시스템을 서비스 형태로 제공합니다.  
> `InputManager`에서 `PlayerInputAction`을 생성하여 사용합니다.

---

## 주요 기능

- Unity Input System 기반 입력 관리
- `PlayerInputAction` 래퍼 제공
- 서비스 로케이터 등록 지원
- 입력 액션 맵 접근 가능

---

## Public API

| Member | Description |
|---|---|
| `PlayerInputAction ActionMaps` | 입력 액션 맵 래퍼를 제공합니다. |

---

## 예시

```csharp
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
        public Vector2 Move()
        {
             return ActionMaps.Player.Move.ReadValue<Vector2>();
        }
    }
}


```
