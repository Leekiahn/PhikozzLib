# Stat Modifier

> 기본값에 장비·버프·패시브 등의 보정치(Modifier)를 여러 개 쌓아 최종값을 계산하는 스탯 시스템입니다.  
> 최종값이 바뀌면 구독자에게 알려주므로, UI나 다른 컴포넌트가 스탯 변화를 자동으로 따라갈 수 있습니다.

---

## 주요 기능

- 기본값 + 보정치로 최종값 계산
- 보정 종류 3가지 (`Flat` / `PercentAdd` / `PercentMult`)
- 출처(Source) 단위로 보정치 일괄 제거
- 지속시간이 있는 보정치 (UniTask, 시간이 끝나면 자동 제거, 남은 시간 조회)
- 최종값 변경 시 구독자에게 알림 (`ObservableValue<float>` 사용)

---

## 장점

- **보정치 관리가 단순해짐**: 장비·버프가 몇 개든 `AddModifier()`로 넣고 빼기만 하면 최종값은 `Stat`이 알아서 계산합니다.
- **출처 단위 제거**: 장비 하나가 여러 보정치를 줘도, 그 장비 객체를 `Source`로 넘겨 한 번에 제거할 수 있습니다.
- **자동 반영**: 스탯을 구독해두면 보정치를 추가·제거하는 코드는 UI나 `NavMeshAgent.speed` 같은 반영 대상을 신경 쓸 필요가 없습니다.

---

## 계산식

```
최종값 = (기본값 + Flat 합) × (1 + PercentAdd 합) × Π(1 + PercentMult)
```

| 종류 | 의미 | 예시 |
|---|---|---|
| `Flat` | 고정 수치를 더함 | 검 장착 공격력 +10 → `10` |
| `PercentAdd` | 퍼센트끼리 **더한 뒤** 한 번 곱함 | 버프 +20%, +10% → `0.2`, `0.1` → ×1.3 |
| `PercentMult` | 각각 **따로** 곱함 | 분노 ×1.5 → `0.5` |

예) 기본 100, 검 +10, 버프 +20%·+10%, 분노 ×1.5 → `(100 + 10) × 1.3 × 1.5 = 214.5`

---

## Public API

### Stat

| Member | Description |
|---|---|
| `Stat(float baseValue)` | 기본값을 지정해서 생성합니다. |
| `BaseValue` | 기본값입니다. 바꾸면 최종값을 다시 계산합니다. |
| `Value` | 보정치가 모두 적용된 최종값입니다. |
| `Modifiers` | 현재 적용 중인 보정치 목록입니다(읽기 전용). 버프 목록·남은 시간 UI 등에 사용합니다. |
| `AddModifier(StatModifier modifier)` | 보정치를 추가하고 최종값을 다시 계산합니다. |
| `AddModifierForDuration(StatModifier modifier, float duration)` | `duration`초 동안만 보정치를 적용하고, 시간이 끝나면 제거합니다. 중간에 해제하려면 `RemoveModifier()` / `RemoveAllModifiersFromSource()`를 호출합니다. 여러 개를 동시에 걸 수 있고 각각 따로 만료됩니다. `UniTask`를 반환하므로 `await`하거나 `.Forget()`으로 호출합니다. |
| `RemoveModifier(StatModifier modifier)` | 보정치를 제거하고 최종값을 다시 계산합니다. |
| `RemoveAllModifiersFromSource(object source)` | 해당 출처의 보정치를 모두 제거합니다. 제거된 게 있을 때만 다시 계산합니다. |
| `Subscribe(Action<float> callback)` | 최종값이 바뀔 때 호출될 콜백을 등록합니다. |
| `Unsubscribe(Action<float> callback)` | 등록한 콜백을 해제합니다. |

### StatModifier

| Member | Description |
|---|---|
| `StatModifier(float value, eStatModifierType type, object source)` | 보정치를 생성합니다. |
| `Value` | 보정 수치입니다. 퍼센트는 `0.2`(=20%)처럼 비율로 넣습니다. |
| `Type` | 보정 종류(`Flat` / `PercentAdd` / `PercentMult`)입니다. |
| `Source` | 보정치를 준 출처(장비, 버프 등)입니다. |
| `Duration` | 지속시간(초)입니다. `AddModifierForDuration()`으로 추가했을 때만 값이 들어가고, 그 외에는 `0`입니다. |
| `RemainingTime` | 남은 시간(초)입니다. 매 프레임 `Time.deltaTime`만큼 줄어들고, 끝나거나 취소되면 `0`이 됩니다. |
| `SubscribeRemainingTime(Action<float> callback)` | 남은 시간이 바뀔 때마다 호출될 콜백을 등록합니다. 버프 아이콘 등 남은 시간 UI에 사용합니다. |
| `UnsubscribeRemainingTime(Action<float> callback)` | 등록한 콜백을 해제합니다. |

---

## 사용 예시

**보정치 추가 / 제거**

```csharp
using PhikozzLib;

public class Player : MonoBehaviour
{
    public Stat Attack { get; } = new(100f);

    public void Equip(Weapon sword)
    {
        Attack.AddModifier(new StatModifier(10f, eStatModifierType.Flat, sword));
        Attack.AddModifier(new StatModifier(0.05f, eStatModifierType.PercentAdd, sword));
    }

    public void Unequip(Weapon sword)
    {
        Attack.RemoveAllModifiersFromSource(sword); // 이 검이 준 보정치만 제거
    }

    public float GetDamage()
    {
        return Attack.Value; // 필요한 순간에 값만 읽으면 되는 곳은 구독 없이 Value를 바로 읽습니다.
    }
}
```

**스탯 변화에 반응하기 (구독)**

```csharp
public class StatWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _attackText;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        _player.Attack.Subscribe(UpdateAttackText);
        UpdateAttackText(_player.Attack.Value); // 구독만으로는 현재 값이 오지 않으므로 처음 1번 직접 표시
    }

    private void OnDestroy()
    {
        _player.Attack.Unsubscribe(UpdateAttackText);
    }

    private void UpdateAttackText(float attack)
    {
        _attackText.text = attack.ToString("0.#");
    }
}
```

**지속시간이 있는 보정치**

```csharp
using Cysharp.Threading.Tasks;

public void ApplyAttackBuff()
{
    // 10초 동안 공격력 +20%
    var modifier = new StatModifier(0.2f, eStatModifierType.PercentAdd, this);
    Attack.AddModifierForDuration(modifier, 10f).Forget();
}

private void OnDestroy()
{
    Attack.RemoveAllModifiersFromSource(this); // 대기 중인 지속시간 버프 정리
}
```

중간에 해제(디스펠)할 때는 일반 보정치와 똑같이 `RemoveModifier()` 한 번만 호출합니다. 대기 중이던 타이머는 다음 프레임에 알아서 끝납니다.

```csharp
private StatModifier _attackBuff;

public void ApplyAttackBuff()
{
    _attackBuff = new StatModifier(0.2f, eStatModifierType.PercentAdd, this);
    Attack.AddModifierForDuration(_attackBuff, 10f).Forget();
}

public void DispelAttackBuff()
{
    Attack.RemoveModifier(_attackBuff); // 즉시 보정치 제거
}
```

**버프 남은 시간 표시**

`Update()`에서 매 프레임 읽지 않고, 남은 시간을 구독해서 바뀔 때마다 갱신합니다.

```csharp
public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private TextMeshProUGUI _remainingText;

    private StatModifier _modifier;

    public void Init(StatModifier modifier)
    {
        _modifier = modifier;
        _modifier.SubscribeRemainingTime(UpdateRemainingTime);
        UpdateRemainingTime(_modifier.RemainingTime);
    }

    private void OnDestroy()
    {
        _modifier.UnsubscribeRemainingTime(UpdateRemainingTime);
    }

    private void UpdateRemainingTime(float remainingTime)
    {
        _cooldownImage.fillAmount = remainingTime / _modifier.Duration;
        _remainingText.text = Mathf.CeilToInt(remainingTime).ToString();
    }
}
```

---

## `_isDirty`(Dirty Flag)를 쓰지 않는 이유

처음에는 Dirty Flag 패턴으로 설계했습니다. 보정치가 바뀌면 `_isDirty = true`로 표시만 해두고, 누군가 `Value`를 **읽을 때** 다시 계산하는 방식입니다. 아무도 읽지 않으면 계산도 하지 않는다는 장점이 있습니다.

하지만 `ObservableValue`로 변경 알림을 주려면 값이 **바뀌는 순간** 새 값을 알아야 하므로, 보정치가 바뀔 때 바로 계산해야 합니다. "읽을 때까지 계산을 미룬다"와 "바뀌면 바로 알린다"는 동시에 성립할 수 없어서, 즉시 계산 방식을 택하고 `_isDirty`를 제거했습니다.

- 보정치가 바뀌는 건 장착·버프 같은 드문 순간이라, 매번 계산해도 비용이 거의 없습니다.
- 계산 결과는 `ObservableValue` 안에 저장되므로, `Value`를 매 프레임 읽어도 다시 계산하지 않습니다. Dirty Flag가 하던 캐싱 역할도 그대로 유지됩니다.
- 값이 실제로 바뀐 경우에만 알림이 갑니다(`ObservableValue`의 같은 값 비교).

---

## 주의사항

- **구독 해제는 `OnDestroy()`에서 합니다.** 해제하지 않으면 파괴된 오브젝트의 콜백이 남아 에러가 납니다.
- **보정치를 연달아 추가하면 중간값 알림도 그만큼 갑니다.** 예) 보정치 3개 추가 → 알림 3번. UI 갱신 정도라면 문제없습니다.
- **지속시간 버프 해제에 `CancellationToken`은 필요 없습니다.** `RemoveModifier()` / `RemoveAllModifiersFromSource()`가 제거한 보정치의 `RemainingTime`을 0으로 만들어서, 대기 중이던 타이머가 다음 프레임에 스스로 끝납니다.
- **오브젝트가 파괴될 때는 `OnDestroy()`에서 `RemoveAllModifiersFromSource()` 등으로 지속시간 버프를 정리합니다.** 정리하지 않아도 지속시간이 끝나면 타이머가 멈추지만, 그때까지는 파괴된 뒤에도 타이머가 계속 돕니다.
- **지속시간은 `Time.timeScale`을 따릅니다.** 일시정지(`timeScale = 0`) 중에는 버프 시간도 멈춥니다.
- **`Modifiers`는 읽기 전용으로만 공개합니다.** 목록을 직접 수정하면 최종값이 다시 계산되지 않으므로, 추가·제거는 반드시 `AddModifier()` / `RemoveModifier()` 등을 사용합니다.
- **같은 버프 재적용 시 시간 초기화, 중첩 규칙은 지원하지 않습니다.** 이런 규칙이 필요하면 버프를 객체로 관리하는 별도 버프 시스템에서 `AddModifier()` / `RemoveAllModifiersFromSource()`를 호출하는 방식으로 구현합니다.
- `Stat`은 서비스가 아니라 일반 C# 클래스입니다. 캐릭터마다 각자 가지고 씁니다.
