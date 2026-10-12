# DevLog

> 에디터와 Development Build에서만 출력되는 로그 래퍼입니다. 릴리스 빌드에서는 호출 코드 자체가 빠집니다(`[Conditional]`).

## Public API

| Method | Description |
|---|---|
| `Info(object message, Object context = null)` | 일반 로그를 출력합니다. (`Debug.Log`) |
| `Warning(object message, Object context = null)` | 경고 로그를 출력합니다. (`Debug.LogWarning`) |

`context`를 넘기면 콘솔에서 로그를 클릭했을 때 그 오브젝트가 하이라이트됩니다.

## 사용 예시

```csharp
DevLog.Info($"[Inventory] Item added : {item.Name}");
DevLog.Warning($"[UIManager] Popup '{typeof(T).Name}' is not registered.", this);
```

## 주의사항

- **인자에 부작용이 있는 코드를 넣지 않습니다.** (`DevLog.Info(count++)` 등) 릴리스에서는 실행되지 않아 동작이 달라집니다.
- 에러 로그는 제공하지 않습니다. 릴리스에서도 원인을 추적해야 하는 문제(세이브 손상 등)는 `Debug.LogError` / `Debug.LogWarning`을 직접 씁니다.
