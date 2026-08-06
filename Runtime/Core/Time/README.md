# TimeManager

<img width="607" height="368" alt="Image" src="https://github.com/user-attachments/assets/89b2e654-f325-42a5-b3f4-b33d8111fa8c" />

> 게임 시간 제어 서비스를 제공합니다.  
> `ITimeService` 인터페이스를 상속받아 구현합니다.

---

## 주요 기능

- 시간 배율 설정
- 특정 시간 동안 타임스케일 변경
- 프리즈 프레임 처리
- 타임스케일 해제
- 타임스케일 초기화
- `MoreMountains.Feedbacks` 기반 이벤트 트리거

---

## Public API

| Method | Description |
|---|---|
| `SetTimeScale(eTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)` | 타임스케일을 설정합니다. |
| `FreezeFrame(float duration)` | 지정 시간 동안 프레임을 정지합니다. |
| `UnfreezeFrame()` | 프리즈 상태를 해제합니다. |
| `ResetTimeScale()` | 타임스케일을 기본값으로 되돌립니다. |
