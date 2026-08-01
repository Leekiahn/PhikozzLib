# Core.FloatingText

> 플로팅 텍스트 스폰 서비스를 제공합니다.  
> `ServiceLocator`에 등록하여 사용합니다.

---

## 주요 기능

- 타입 기반 플로팅 텍스트 스폰
- `MMFloatingTextSpawner` 연동
- 여러 타입의 텍스트를 리스트로 관리
- 위치와 방향을 지정하여 출력 가능

---

## Public API

| Method | Description |
|---|---|
| `Spawn(eFloatingTextType type, string value, Vector3 position, Vector3 direction)` | 지정한 타입의 플로팅 텍스트를 생성합니다. |
