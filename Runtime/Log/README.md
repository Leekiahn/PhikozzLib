# DevLog

> 에디터와 Development Build에서만 출력되는 로그 래퍼입니다.  
> 릴리스 빌드에서는 호출 코드 자체가 컴파일에서 빠집니다.

---

## 주요 기능

- 에디터 / Development Build에서만 로그 출력
- 릴리스 빌드에서는 호출과 메시지 문자열 생성 비용까지 0

---

## 장점

- **릴리스 성능**: `Debug.Log`는 릴리스 빌드에서도 문자열 생성과 스택 트레이스 수집 비용이 듭니다. `DevLog`는 `[Conditional]` 속성으로 호출 코드 자체가 빠지므로 비용이 전혀 없습니다.
- **한 곳에서 관리**: 로그 형식을 바꾸거나 파일 기록 등을 추가할 때 `DevLog`만 수정하면 됩니다.

---

## Public API

| Method | Description |
|---|---|
| `Info(object message, Object context = null)` | 일반 로그를 출력합니다. (`Debug.Log`) |
| `Warning(object message, Object context = null)` | 경고 로그를 출력합니다. (`Debug.LogWarning`) |

`context`에 오브젝트를 넘기면 콘솔에서 로그를 클릭했을 때 해당 오브젝트가 하이라이트됩니다.

---

## 사용 예시

```csharp
using PhikozzLib;

DevLog.Info($"[Inventory] Item added : {item.Name}");
DevLog.Warning($"[UIManager] Popup '{typeof(T).Name}' is not registered.", this);
```

---

## 언제 `DevLog`를 쓰고, 언제 `Debug`를 쓰는가

| 상황 | 사용 |
|---|---|
| 개발 중 실수를 알려주는 로그 (등록 누락, 잘못된 키 등) | `DevLog.Warning` |
| 흐름 확인용 디버그 로그 | `DevLog.Info` |
| 릴리스에서도 원인을 추적해야 하는 문제 (세이브 로드 실패 등) | `Debug.LogWarning` / `Debug.LogError` |

라이브러리 내부의 `StateMachine`, `UIManager`, `CameraManager`, `EffectManager`, `AddressableManager`의 등록 누락·잘못된 키 경고는 `DevLog.Warning`을 사용하고, `SaveManager`의 세이브 손상 로그는 `Debug.LogError`를 사용합니다.

로그는 **"로그가 없으면 원인을 찾기 어려운 실패"**(잘못된 키로 조용히 `null`을 반환하는 경우 등)에만 넣습니다. 매 프레임 실행되는 코드나 정상 흐름 확인용 로그는 에디터 성능과 콘솔 노이즈 때문에 넣지 않습니다.

---

## 주의사항

- **`DevLog` 호출 안의 코드는 릴리스 빌드에서 실행되지 않습니다.** 인자에 부작용이 있는 코드(`DevLog.Info(count++)` 등)를 넣으면 릴리스에서만 동작이 달라지므로 넣지 않습니다.
- **에러 로그는 제공하지 않습니다.** 에러는 릴리스 빌드에서도 남아야 하므로 `Debug.LogError`를 직접 사용합니다.
