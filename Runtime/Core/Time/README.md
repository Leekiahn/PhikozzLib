# TimeManager

<img width="607" height="368" alt="Image" src="https://github.com/user-attachments/assets/89b2e654-f325-42a5-b3f4-b33d8111fa8c" />

> 타임스케일 변경과 프리즈 프레임을 제어하는 서비스입니다. Feel의 `MMTimeManager`를 사용합니다. (`ITimeService`)

## Public API

| Method | Description |
|---|---|
| `SetTimeScale(eTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)` | 타임스케일을 변경합니다. `For`는 `duration` 동안(또는 `infinite`), `Reset`은 기본값으로, `Unfreeze`는 프리즈 해제입니다. |
| `FreezeFrame(float duration)` | 지정 시간 동안 프레임을 정지합니다. |
| `UnfreezeFrame()` | 프리즈를 해제합니다. |
| `ResetTimeScale()` | 타임스케일을 기본값으로 되돌립니다. |

## 사용 예시

```csharp
private ITimeService _timeService;

private void Start()
{
    _timeService = ServiceLocator.Get<ITimeService>();
}

public void OnCriticalHit()
{
    _timeService.FreezeFrame(0.1f); // 타격감용 히트 스톱
}

public void OnFinalBlow()
{
    // 2초 동안 0.3배속 슬로우 모션
    _timeService.SetTimeScale(eTimeScaleMethods.For, 0.3f, 2f, lerp: true, lerpSpeed: 5f, infinite: false);
}
```

## 주의사항

- `TimeManager` 프리팹에 `MMTimeManager`가 함께 있어야 합니다(`RequireComponent`).
