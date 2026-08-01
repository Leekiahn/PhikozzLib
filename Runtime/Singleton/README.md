# Generic Singleton

> 싱글톤 패턴을 위한 공용 베이스 클래스를 제공합니다.  
> 전역/지연 생성/씬 단위 싱글톤을 쉽게 구현할 수 있습니다.

---

## 주요 기능

- 전역 싱글톤 구현
- 지연 생성 싱글톤 구현
- 씬 단위 싱글톤 구현
- `DontDestroyOnLoad` 지원
- 중복 인스턴스 자동 제거
- 제네릭 기반 컴포넌트 접근

---

## Public API

### SingletonGlobal<T>

| Member | Description |
|---|---|
| `Instance` | 전역에서 접근 가능한 싱글톤 인스턴스입니다. |
| `Awake()` | 최초 인스턴스를 유지하고 `DontDestroyOnLoad`로 보존합니다. |

### SingletonLazy<T>

| Member | Description |
|---|---|
| `Instance` | 필요할 때 자동 생성되는 싱글톤 인스턴스입니다. |
| `_dontDestroyOnLoad` | 씬 전환 시 파괴되지 않도록 설정합니다. |
| `Awake()` | 최초 인스턴스를 유지하고 옵션에 따라 보존합니다. |

### SingletonScene<T>

| Member | Description |
|---|---|
| `Instance` | 현재 씬에서만 유효한 싱글톤 인스턴스입니다. |
| `Awake()` | 씬 내 중복 인스턴스를 제거합니다. |
