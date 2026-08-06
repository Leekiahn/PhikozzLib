# FloatingTextManager

<img width="608" height="290" alt="image" src="https://github.com/user-attachments/assets/ba516690-143b-47b5-b747-112ef3ebb825" />


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
| `Spawn(string key, string value, Vector3 position, Vector3 direction)` | Key의 플로팅 텍스트를 생성합니다. |

