# EffectManager

> Addressables 프리로드 여부를 선택해 `ParticleSystem` 프리팹을 이름 기준으로 풀링하고 재생하는 서비스입니다.  
> `IEffectService` 인터페이스를 구현하며, `IServiceInit`을 통해 서비스 등록 후 초기화됩니다.

<br>

## 로드 방식 설정

### Addressables 자동 등록

`EffectManager` Inspector의 `Load By Addressable Service`를 켜면 Addressables 라벨에서 이펙트를 자동 등록합니다.

1. `BootstrapConfig`에 `AddressableManager`와 `EffectManager`를 모두 등록합니다.
2. 이펙트 프리팹을 Addressables로 등록하고 같은 라벨을 지정합니다.
3. `EffectManager` Inspector의 `Effect Label`에 해당 라벨을 지정합니다.
4. 각 이펙트 프리팹의 루트에 `ParticleSystem` 컴포넌트를 추가하고, 프리팹 이름을 고유하게 설정합니다.

`Bootstrapper`는 모든 서비스를 등록한 뒤 `IServiceInit.Init()`을 호출합니다. `EffectManager`는 이 시점에 `IAddressableService`를 통해 라벨의 이펙트 프리팹을 프리로드하고, 프리팹 이름을 `Play()`의 key로 사용합니다.

### 외부 등록

`Load By Addressable Service`를 끄면 자동 프리로드를 하지 않습니다. 이 경우 프로젝트의 데이터 로더 등 외부 코드에서 `IEffectService.RegisterEffect(key, prefab)`을 호출해 풀을 등록합니다. 등록을 해제할 때는 `UnregisterEffect(key)`를 사용합니다.

<br>

## 주요 기능

- Addressables 라벨에 속한 이펙트 프리팹 자동 프리로드 및 풀 생성
- 외부 데이터 로더를 통한 수동 이펙트 등록 지원
- 프리팹 이름을 key로 사용한 이펙트 재생
- 지정한 Transform에 부착하거나, 월드 위치/회전으로 재생
- 재생 종료 후 오브젝트 풀에 자동 반환

<br>

## Public API

| Method | Description |
|---|---|
| `RegisterEffect(string key, ParticleSystem prefab)` | `key`와 프리팹으로 이펙트 풀을 등록합니다. 같은 key가 이미 등록되어 있으면 기존 풀을 유지합니다. `Load By Addressable Service`를 끈 경우 외부 로더에서 사용합니다. |
| `UnregisterEffect(string key)` | key에 해당하는 풀을 등록 목록에서 제거합니다. |
| `Play(string key, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null)` | `key`와 같은 이름의 이펙트를 재생합니다. `duration > 0`이면 해당 시간 뒤 정지 후 반환하고, `0`이면 파티클이 자연 종료될 때 반환합니다. `attachToTransform`을 지정하면 해당 Transform의 자식으로 부착합니다. 등록되지 않은 key면 `null`을 반환합니다. |

<br>

## 사용 예시

Addressables 프리팹 이름이 `HitSpark`인 경우입니다.

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

`EffectManager`가 초기화를 마친 뒤 호출해야 합니다. 일반 게임 진입 흐름에서는 `Bootstrapper`가 초기화를 완료한 이후이므로, 씬의 `Start()` 이후 호출하면 됩니다.
