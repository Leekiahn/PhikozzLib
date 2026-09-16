# Generic Singleton

> 싱글톤 패턴을 위한 공용 베이스 클래스를 제공합니다.  
> 전역/지연 생성/씬 단위 싱글톤을 쉽게 구현할 수 있습니다.

---

## 어떤 걸 언제 쓰나

| Type | 특징 | 언제 쓰나 |
|---|---|---|
| `SingletonGlobal<T>` | 씬에 미리 배치해두면 `DontDestroyOnLoad`로 앱 전체에서 유지 | 앱 시작부터 끝까지 살아있어야 하는 매니저 |
| `SingletonLazy<T>` | 처음 `Instance`를 참조하는 순간 씬에 없으면 자동 생성 | 미리 씬에 두기 귀찮거나, 실제로 쓰일 때만 만들고 싶은 경우 |
| `SingletonScene<T>` | `DontDestroyOnLoad` 없음, 현재 씬에서만 유효 | 씬이 바뀌면 같이 사라져야 하는 것 (예: `CameraManager`) |

이 라이브러리의 서비스(`SaveManager` 등)는 `ServiceLocator` + `Bootstrapper`로 관리되고, 이 Singleton 베이스들은 그와 별개로 **씬에 직접 배치해서 쓰는 매니저**(예: `CameraManager`)를 위한 것입니다.

---

## Public API

### SingletonGlobal<T>

| Member | Description |
|---|---|
| `Instance` | 전역에서 접근 가능한 싱글톤 인스턴스입니다. |
| `Awake()` | 최초 인스턴스를 유지하고 `DontDestroyOnLoad`로 보존합니다. 이미 인스턴스가 있으면 자신을 파괴합니다. |

### SingletonLazy<T>

| Member | Description |
|---|---|
| `Instance` | 필요할 때 자동 생성되는 싱글톤 인스턴스입니다. 씬에 이미 있으면 그걸 쓰고, 없으면 새 GameObject를 만들어 붙입니다. |
| `_dontDestroyOnLoad` | 씬 전환 시 파괴되지 않도록 설정합니다(인스펙터에서 토글). |
| `Awake()` | 최초 인스턴스를 유지하고 옵션에 따라 보존합니다. |

### SingletonScene<T>

| Member | Description |
|---|---|
| `Instance` | 현재 씬에서만 유효한 싱글톤 인스턴스입니다. |
| `Awake()` | 씬 내 중복 인스턴스를 제거합니다. |

---

## 사용 예시

```csharp
public class GameSettings : SingletonGlobal<GameSettings>
{
    public float MasterVolume { get; private set; } = 1f;
}

// 다른 곳에서
GameSettings.Instance.MasterVolume;
```

`protected virtual void Awake()`를 상속받는 쪽에서 override할 경우, 반드시 `base.Awake()`를 먼저 호출해야 인스턴스 등록이 정상적으로 이뤄집니다:

```csharp
public class CameraManager : SingletonScene<CameraManager>
{
    protected override void Awake()
    {
        base.Awake();
        // 이후 초기화
    }
}
```

---

## Reload Domain을 끄고 반복 재생해도 안전한 이유

`Instance`는 `T : Component` 타입이라, Unity의 `UnityEngine.Object`가 오버라이드하는 `==` 연산자 덕분에 "파괴된 오브젝트는 null과 같다"는 게 정확히 동작합니다. 그래서 Enter Play Mode Options에서 Reload Domain을 꺼서 정적 필드가 세션 사이에 초기화되지 않아도, 이전 세션에서 파괴된 인스턴스는 다음 `Awake()`의 `if (Instance == null)` 체크에서 정확히 `true`로 잡혀 새 인스턴스로 교체됩니다. (다만 `ServiceLocator`처럼 인터페이스 타입으로 참조를 들고 있는 경우는 이 보호를 못 받습니다 — `SeviceLocator/README.md` 참고.)
