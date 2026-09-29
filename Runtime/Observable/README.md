# ObservableValue

> 값이 바뀌면 구독자에게 알려주는 옵저버 패턴 래퍼입니다.  
> HP, 골드처럼 특정 객체에 속한 값을 UI 등 여러 곳에서 구독할 때 사용합니다.

---

## 주요 기능

- 값 변경 시 구독자에게 새 값 전달
- 같은 값으로 설정하면 알림을 보내지 않음
- 구독 / 구독 해제

---

## 장점

- **갱신 누락 방지**: 값을 바꾸는 곳(피격, 회복, 로드 등)이 여러 군데여도 `Value`만 바꾸면 자동으로 알림이 가서, UI 갱신 호출을 빠뜨릴 일이 없습니다.
- **결합도 감소**: HP 텍스트, HP 바, 사망 판정 등 보는 쪽이 늘어나도 각자 구독만 하면 되고, 값을 가진 쪽은 누가 보는지 몰라도 됩니다.
- **불필요한 호출 없음**: 실제로 값이 바뀔 때만 알림이 가므로, `Update()`에서 매 프레임 확인하는 방식보다 가볍습니다.
- **대상이 명확함**: 전역 이벤트와 달리 특정 객체의 값을 직접 구독하므로, 캐릭터가 여러 명이어도 누구의 값인지 따로 거를 필요가 없습니다.

> 값을 바꾸는 곳도 하나, 보는 곳도 하나뿐이라면 직접 호출하는 편이 더 단순합니다.

---

## Public API

| Member | Description |
|---|---|
| `Value` | 현재 값입니다. 다른 값으로 설정하면 구독자에게 새 값을 전달합니다. |
| `ObservableValue(T initialValue = default)` | 초기값을 지정해서 생성합니다. |
| `Subscribe(Action<T> callback)` | 값이 바뀔 때 호출될 콜백을 등록합니다. |
| `Unsubscribe(Action<T> callback)` | 등록한 콜백을 해제합니다. |

---

## `EventManager`와의 차이

| | `EventManager` | `ObservableValue<T>` |
|---|---|---|
| 다루는 것 | "무슨 일이 일어났다" (사건) | "지금 값이 얼마다" (상태) |
| 범위 | 전역 | 특정 객체에 속한 값 하나 |
| 예시 | `PlayerDiedEvent` | 캐릭터의 HP, 보유 골드 |

---

## 사용 예시

```csharp
using PhikozzLib;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public ObservableValue<int> Hp { get; } = new(100);
}

public class HpText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;

    private PlayerStatus _status;

    public void Init(PlayerStatus status)
    {
        _status = status;
        _status.Hp.Subscribe(UpdateHpText);
        UpdateHpText(_status.Hp.Value);
    }

    private void OnDestroy()
    {
        _status.Hp.Unsubscribe(UpdateHpText);
    }

    private void UpdateHpText(int hp)
    {
        _hpText.text = hp.ToString();
    }
}
```

```csharp
// 값을 바꾸면 구독한 곳에 새 값이 전달됩니다.
_status.Hp.Value -= 10;
```

---

## 주의사항

- **구독 해제는 `OnDestroy()`에서 합니다.** 해제하지 않으면 파괴된 오브젝트의 콜백이 계속 남아서, 값이 바뀔 때 `MissingReferenceException`이 나거나 메모리에서 풀리지 않습니다.
- **`Subscribe()`는 현재 값을 바로 전달하지 않습니다.** 처음 표시가 필요하면 위 예시처럼 구독 직후 `Value`로 한 번 직접 갱신합니다.
- 값 비교는 `EqualityComparer<T>.Default`를 사용하므로 `int`, `float` 등 값 타입도 박싱(GC 할당) 없이 비교합니다.
