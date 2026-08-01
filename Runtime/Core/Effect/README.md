# Core.Effect

> 이펙트(ParticleSystem) 재생 및 관리 서비스를 제공합니다.  
> `IEffectService` 인터페이스를 상속받아 구현합니다.

---

## 주요 기능

- 제네릭 키 기반 이펙트 등록 및 재생
- 이펙트 구독/사용 없이 `Play()`로 즉시 재생
- Attach 대상 Transform에 이펙트 부착 가능
- 재생 종료 후 자동 반환(Pool Release)
- `EffectDatabase`를 통한 ScriptableObject 기반 관리

---

## Public API

| Method | Description |
|---|---|
| `Play(string key, Vector3 position, Quaternion rotation, Transform attachToTransform = null)` | 등록된 이펙트를 키로 찾아 재생합니다. `attachToTransform`이 있으면 해당 Transform에 붙여 재생합니다. |

---

## 사용 방법

1. `EffectDatabase`를 생성합니다.
2. 키와 `ParticleSystem` 프리팹을 등록합니다.
3. `EffectManager`에 `EffectDatabase`를 연결합니다.
