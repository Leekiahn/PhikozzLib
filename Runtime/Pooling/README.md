# PoolManager

> `TrackedPool<T>`를 key 기반으로 관리하는 범용 오브젝트 풀 서비스입니다.  
> `IPoolService`를 통해 투사체, 몬스터, 드롭 아이템처럼 여러 시스템에서 반복 생성되는 `Component`를 재사용합니다.

<br>

## 설정

1. `PoolManager`를 포함한 프리팹을 만들고 `BootstrapConfig`에 등록합니다.
2. 사용할 프리팹과 부모 Transform을 준비합니다.
3. 게임 시작 시 `RegisterPool()`로 고유 key와 프리팹을 등록합니다.

`PoolManager`는 `IServiceRegister`를 구현하므로, 등록 뒤에는 `ServiceLocator.Get<IPoolService>()`로 조회합니다.

<br>

## 주요 기능

- key 기반 풀 등록과 해제
- `TrackedPool<T>` 기반 활성 인스턴스 추적
- 위치와 회전을 지정한 인스턴스 생성
- 특정 인스턴스 또는 풀 전체 반환
- 풀 전체 초기화

<br>

## `IPoolService` Public API

| Method | Description |
| --- | --- |
| `RegisterPool<T>(string key, T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 20)` | key와 프리팹으로 풀을 등록합니다. 같은 key가 이미 있으면 기존 풀을 유지합니다. |
| `UnregisterPool(string key)` | 해당 풀의 활성 인스턴스를 반환하고, 풀을 비우고, 등록을 해제합니다. |
| `Spawn<T>(string key, Vector3 position, Quaternion rotation)` | key의 풀에서 `T` 인스턴스를 꺼내 위치·회전을 적용합니다. 등록되지 않은 key면 `null`을 반환합니다. |
| `Despawn<T>(string key, T instance)` | 지정한 인스턴스를 해당 풀에 반환합니다. |

`PoolManager` 클래스에는 관리용으로 `DespawnAll(string key)`, `ClearPool(string key)`, `ClearAllPools()`도 제공됩니다. 이 메서드들은 `IPoolService`가 아닌 구체 `PoolManager`에만 있으므로, 일반 게임 로직에서는 `IPoolService`의 등록·생성·반환 API만 사용하는 것을 권장합니다.

<br>

## 사용 예시

```csharp
using PhikozzLib;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Projectile _fireProjectilePrefab;
    [SerializeField] private Transform _projectileParent;

    private IPoolService _poolService;

    private void Start()
    {
        _poolService = ServiceLocator.Get<IPoolService>();
        _poolService.RegisterPool(
            "FireProjectile",
            _fireProjectilePrefab,
            _projectileParent,
            defaultCapacity: 20,
            maxSize: 50);
    }

    public Projectile Fire(Vector3 position, Quaternion rotation)
    {
        return _poolService.Spawn<Projectile>(
            "FireProjectile",
            position,
            rotation);
    }

    public void ReturnProjectile(Projectile projectile)
    {
        _poolService.Despawn("FireProjectile", projectile);
    }
}
```

`RegisterPool<T>()`, `Spawn<T>()`, `Despawn<T>()`의 `T`는 같은 구체 타입이어야 합니다. 예를 들어 `Projectile`로 등록한 풀은 `Spawn<Projectile>()`과 `Despawn("FireProjectile", projectile)`로 사용합니다.

<br>

## `TrackedPool<T>`

`PoolManager` 내부에서는 `TrackedPool<T>`를 사용합니다. 특정 시스템에서 key 기반 공용 서비스 없이, 하나의 프리팹만 독립적으로 풀링해야 한다면 `TrackedPool<T>`를 직접 사용해도 됩니다.

| Method | Description |
| --- | --- |
| `Get()` | 풀에서 인스턴스를 꺼내 활성 목록에 추가합니다. |
| `Release()` | 가장 최근에 활성화된 인스턴스를 반환합니다. |
| `Release(T instance)` | 지정한 인스턴스를 반환합니다. |
| `ReleaseAll()` | 활성 인스턴스를 모두 반환합니다. |
| `Clear()` | 활성 인스턴스를 반환한 뒤 풀을 비웁니다. |
