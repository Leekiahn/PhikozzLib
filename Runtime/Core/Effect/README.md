# EffectManager

> `ParticleSystem` 이펙트를 key로 풀링·재생하고, 재생이 끝나면 자동으로 풀에 반환하는 서비스입니다. (`IEffectService`)

## Public API

| Method | Description |
|---|---|
| `RegisterEffect(string key, ParticleSystem prefab)` | 이펙트 풀을 등록합니다. 이미 있는 key면 기존 풀을 유지합니다. |
| `UnregisterEffect(string key)` | 이펙트 풀 등록을 해제합니다. |
| `Play(string key, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null)` | 이펙트를 재생합니다. `duration > 0`이면 그 시간 뒤 방출을 멈추고, `0`이면 자연 종료 후 반환합니다. `attachToTransform`을 넘기면 그 자식으로 붙입니다. 미등록 key면 `null`. |

## 사용 예시

```csharp
private IEffectService _effectService;

private void Start()
{
    _effectService = ServiceLocator.Get<IEffectService>();
}

public void OnHit(Vector3 hitPoint)
{
    _effectService.Play("HitSpark", hitPoint, Quaternion.identity, duration: 1f);
}
```

## 주의사항

- `Load By Addressable Service`를 켜면 `Effect Label` 라벨의 프리팹이 **Addressable 주소를 key로** 자동 등록됩니다. `BootstrapConfig`에 `AddressableManager`도 등록하고, 프리팹 루트에 `ParticleSystem`이 있어야 합니다.
- 끄면 `RegisterEffect`로 직접 등록합니다. `ExampleEffectLoader.cs`(전체 주석)를 프로젝트로 복사해서 쓰면 됩니다.
- **Looping 파티클은 `duration`을 꼭 지정합니다.** `0`이면 끝나지 않아 풀에 반환되지 않습니다.
