# Stat Modifier

> 기본값에 장비·버프 등의 보정치를 쌓아 최종값을 계산하고, 값이 바뀌면 구독자에게 알려주는 스탯 시스템입니다.

## Public API

### `Stat`

| Member | Description |
|---|---|
| `Stat(float baseValue)` | 기본값으로 생성합니다. |
| `BaseValue` | 기본값입니다. 바꾸면 다시 계산합니다. |
| `Value` | 보정치가 적용된 최종값입니다. |
| `Modifiers` | 적용 중인 보정치 목록(읽기 전용)입니다. |
| `AddModifier(StatModifier modifier)` | 보정치를 추가합니다. |
| `AddModifierForDuration(StatModifier modifier, float duration)` | `duration`초 동안만 적용합니다. `.Forget()`으로 호출합니다. |
| `RemoveModifier(StatModifier modifier)` | 보정치를 제거합니다(지속시간 버프 중간 해제 포함). |
| `RemoveAllModifiersFromSource(object source)` | 해당 출처의 보정치를 모두 제거합니다. |
| `Subscribe(Action<float> callback)` / `Unsubscribe(...)` | 최종값 변경을 구독 / 해제합니다. |

### `StatModifier`

| Member | Description |
|---|---|
| `StatModifier(float value, eStatModifierType type, object source)` | 보정치를 생성합니다. 퍼센트는 `0.2`(=20%)처럼 비율로 넣습니다. |
| `Duration` / `RemainingTime` | 지속시간 / 남은 시간(초). 지속시간 버프일 때만 값이 있습니다. |
| `SubscribeRemainingTime(Action<float> callback)` / `UnsubscribeRemainingTime(...)` | 남은 시간 변경을 구독 / 해제합니다(버프 아이콘 UI용). |

**계산식**: `(기본값 + Flat 합) × (1 + PercentAdd 합) × Π(1 + PercentMult)` (결과가 음수면 `0`)

| Type | 의미 |
|---|---|
| `Flat` | 고정 수치를 더합니다. |
| `PercentAdd` | 퍼센트끼리 더한 뒤 한 번 곱합니다. |
| `PercentMult` | 각각 따로 곱합니다. |

## 사용 예시

```csharp
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
        Attack.RemoveAllModifiersFromSource(sword);
    }

    public void ApplyAttackBuff()
    {
        // 10초 동안 공격력 +20%
        Attack.AddModifierForDuration(new StatModifier(0.2f, eStatModifierType.PercentAdd, this), 10f).Forget();
    }

    private void OnDestroy()
    {
        Attack.RemoveAllModifiersFromSource(this);
    }
}
```

```csharp
// UI에서 구독
_player.Attack.Subscribe(UpdateAttackText);
UpdateAttackText(_player.Attack.Value); // 구독만으로는 현재 값이 오지 않으므로 처음 1번 직접 표시
```

## 주의사항

- **구독 해제는 `OnDestroy()`에서 합니다.**
- 보정치를 연달아 추가하면 그 횟수만큼 알림이 갑니다(값이 실제로 바뀔 때만).
- 지속시간은 `Time.timeScale`을 따릅니다. 오브젝트가 파괴될 때는 `RemoveAllModifiersFromSource`로 지속시간 버프를 정리합니다.
- `Modifiers`를 직접 수정하지 않습니다. 추가·제거는 반드시 `AddModifier` / `RemoveModifier` 등으로 합니다.
- 같은 버프 재적용 시 시간 초기화나 중첩 규칙은 지원하지 않습니다. 필요하면 별도 버프 시스템에서 이 API를 호출해서 구현합니다.
