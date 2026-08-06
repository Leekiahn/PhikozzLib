# FloatingTextManager

> 플로팅 텍스트 스폰 서비스를 제공합니다.  
> `IFloatingTextService`을 상속받아 사용합니다.

---

## 주요 기능

- Key 기반 플로팅 텍스트 스폰
- `MMFloatingTextSpawner` 연동
- 여러 Key의 텍스트를 리스트로 관리
- 위치와 방향을 지정하여 출력 가능

---


## Public API

| Method | Description |
|---|---|
| `RegisterFloatingText(BaseFloatingTextLoader loader)` | `BaseFloatingTextLoader`를 통해 Key로 Spawner를 등록합니다.
| `UnRegisterFloatingText(string key)` | Key로 Spawner를 해제합니다.
| `Spawn(string key, string value, Vector3 position, Vector3 direction)` | Key의 플로팅 텍스트를 생성합니다. |

