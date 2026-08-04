# FloatingTextManager

> 플로팅 텍스트 스폰 서비스를 제공합니다.  
> `ServiceLocator`에 등록하여 사용합니다.

---

## 주요 기능

- 타입 기반 플로팅 텍스트 스폰
- `MMFloatingTextSpawner` 연동
- 여러 타입의 텍스트를 리스트로 관리
- 위치와 방향을 지정하여 출력 가능

---

## BaseFloatingTextLoader

```csharp
public class DefaultFloatingTextLoader : BaseFloatingTextLoader
{
    public override string FloatingTextKey => "Default";
}
```
- `FloatingTextManager` 프리팹 하위에 있는 `MMFloatingTextSpawner`에 붙여 Spawner를 등록/해제합니다.


## Public API

| Method | Description |
|---|---|
| `RegisterFloatingText(BaseFloatingTextLoader loader)` | `BaseFloatingTextLoader`를 통해 Key로 Spawner를 등록합니다.
| `UnRegisterFloatingText(string key)` | Key로 Spawner를 해제합니다.
| `Spawn(eFloatingTextType type, string value, Vector3 position, Vector3 direction)` | 지정한 타입의 플로팅 텍스트를 생성합니다. |

