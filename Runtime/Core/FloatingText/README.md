# FloatingTextManager

<img width="608" height="290" alt="image" src="https://github.com/user-attachments/assets/ba516690-143b-47b5-b747-112ef3ebb825" />

> key별 Feel `MMFloatingTextSpawner`로 플로팅 텍스트(데미지 숫자 등)를 띄우는 서비스입니다. (`IFloatingTextService`)

## Public API

| Method | Description |
|---|---|
| `Spawn(string key, string value, Vector3 position, Vector3 direction)` | key의 스포너로 `value` 텍스트를 `position`에서 `direction` 방향으로 띄웁니다. |

## 사용 예시

```csharp
private IFloatingTextService _floatingTextService;

private void Start()
{
    _floatingTextService = ServiceLocator.Get<IFloatingTextService>();
}

public void OnDamaged(int damage, Vector3 hitPoint)
{
    _floatingTextService.Spawn("Damage", damage.ToString(), hitPoint, Vector3.up);
}
```

## 주의사항

- 인스펙터의 `Spawners` 리스트에 key와 `MMFloatingTextSpawner` 프리팹을 등록합니다.
- 등록되지 않은 key는 아무 일도 하지 않습니다(로그 없음).
