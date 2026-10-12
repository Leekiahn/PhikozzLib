# ObservableValue

> 값이 바뀌면 구독자에게 새 값을 알려주는 래퍼입니다. HP, 골드처럼 특정 객체의 값을 UI 등이 구독할 때 씁니다.

## Public API

| Member | Description |
|---|---|
| `ObservableValue(T initialValue = default)` | 초기값을 지정해서 생성합니다. |
| `Value` | 현재 값입니다. 다른 값으로 바꾸면 구독자에게 알립니다. |
| `Subscribe(Action<T> callback)` | 값이 바뀔 때 호출될 콜백을 등록합니다. |
| `Unsubscribe(Action<T> callback)` | 콜백을 해제합니다. |

## 사용 예시

```csharp
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

// 값을 바꾸면 구독한 곳에 전달됩니다.
_status.Hp.Value -= 10;
```

## 주의사항

- **구독 해제는 `OnDestroy()`에서 합니다.** 안 하면 파괴된 오브젝트의 콜백이 남아 에러가 납니다.
- **`Subscribe()`는 현재 값을 바로 전달하지 않습니다.** 처음 표시는 구독 직후 직접 한 번 갱신합니다.
- 같은 값으로 설정하면 알림을 보내지 않습니다.
