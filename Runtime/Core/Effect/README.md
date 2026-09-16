# EffectManager

> 이펙트(ParticleSystem) 등록/재생/풀링 서비스를 제공합니다.  
> `IEffectService` 인터페이스를 상속받아 구현합니다.

<br>

## 주요 기능

- `key` 문자열 기준으로 이펙트 프리팹 등록
- 등록된 이펙트를 `Play()`로 즉시 재생 (오브젝트 풀 기반)
- Attach 대상 Transform에 이펙트 부착 재생 가능
- 재생 종료 후 자동으로 풀에 반환

<br>

## 데이터 소스는 패키지 밖에서 채운다

`EffectManager`는 이펙트 데이터를 어디서 가져오는지 전혀 모릅니다. `RegisterEffect(key, prefab)`만 제공하고, 실제로 무엇을 등록할지는 각 프로젝트가 결정합니다.

- 이 패키지는 git URL로 설치되므로, 특정 프로젝트의 BGDatabase 엔티티(`BG_Effect` 같은) 같은 걸 `EffectManager`가 직접 참조하면 그 타입이 없는 다른 프로젝트에서는 컴파일 자체가 깨집니다.
- 그래서 `ExampleEffectLoader.cs`는 **전체가 주석 처리된 예시 코드**로만 존재합니다. 실제로 쓰려면 이 내용을 프로젝트 쪽 스크립트로 복사해서 주석을 풀고 쓰세요.

```csharp
// ExampleEffectLoader.cs 내용
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
private static void LoadEffects()
{
    var effectService = ServiceLocator.Get<IEffectService>();

    BG_Effect.ForEachEntity(effect =>
    {
        effectService.RegisterEffect(effect.name, effect.prefab);
    });
}
```

BGDatabase가 아니어도 상관없습니다 — `ScriptableObject` 리스트든, 하드코딩이든, `RegisterEffect(key, prefab)`만 호출하면 됩니다.

<br>

## Public API

| Method | Description |
|---|---|
| `RegisterEffect(string key, ParticleSystem prefab)` | `key`로 이펙트 프리팹을 등록하고 풀을 만듭니다. 같은 `key`로 다시 호출하면 기존 풀을 `Clear()`(내부 인스턴스 Destroy 포함)한 뒤 새로 교체합니다. |
| `Play(string key, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null)` | 등록된 이펙트를 재생합니다. `duration > 0`이면 그 시간 뒤 정지 후 반환, `0`이면 파티클이 자연 종료될 때 반환합니다. `attachToTransform`이 있으면 해당 Transform의 자식으로 붙습니다. |

<br>

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
