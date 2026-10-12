# PoolManager

> 프리팹을 key별로 풀링해서 투사체·몬스터·드롭 아이템 등을 재사용하는 서비스입니다. (`IPoolService`)

## Public API

### `IPoolService`

| Method | Description |
|---|---|
| `RegisterPool<T>(string key, T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20)` | 풀을 등록합니다. 이미 있는 key면 기존 풀을 유지합니다. |
| `UnregisterPool(string key)` | 활성 인스턴스를 반환하고 풀을 비운 뒤 등록을 해제합니다. |
| `Spawn<T>(string key, Vector3 position, Quaternion rotation)` | 인스턴스를 꺼내 위치·회전을 적용합니다. 미등록 key면 `null`. |
| `Despawn<T>(string key, T instance)` | 인스턴스를 풀에 반환합니다. |
| `DespawnAll(string key)` | 해당 풀의 활성 인스턴스를 모두 반환합니다. |
| `ClearPool(string key)` / `ClearAllPools()` | 풀을 비웁니다(활성 인스턴스 반환 후 파괴). |

### `TrackedPool<T>` (key 없이 프리팹 하나만 독립적으로 풀링할 때)

| Method | Description |
|---|---|
| `Get()` | 인스턴스를 꺼냅니다. |
| `Release()` | 가장 최근에 꺼낸 인스턴스를 반환합니다. |
| `Release(T instance)` | 지정한 인스턴스를 반환합니다. 이미 반환된 인스턴스면 무시합니다. |
| `ReleaseAll()` | 활성 인스턴스를 모두 반환합니다. |
| `Clear()` | 모두 반환한 뒤 풀을 비웁니다. |

## 사용 예시

```csharp
private IPoolService _poolService;

private void Start()
{
    _poolService = ServiceLocator.Get<IPoolService>();
    _poolService.RegisterPool("FireProjectile", _fireProjectilePrefab, _projectileParent, defaultCapacity: 20, maxSize: 50);
}

public Projectile Fire(Vector3 position, Quaternion rotation)
{
    return _poolService.Spawn<Projectile>("FireProjectile", position, rotation);
}

public void ReturnProjectile(Projectile projectile)
{
    _poolService.Despawn("FireProjectile", projectile);
}
```

## 주의사항

- `RegisterPool<T>` / `Spawn<T>` / `Despawn<T>`의 `T`는 같은 구체 타입이어야 합니다.
- 씬 전환 시 자동으로 반환하지 않습니다. 필요하면 전환 전에 `DespawnAll`을 호출합니다.
