# TimeEditorWindow

<img width="421" height="335" alt="Image" src="https://github.com/user-attachments/assets/a2c627fb-bdd8-45f3-9379-4c1cfadfb4fb" />

> Time Scale, Freeze Frame, Reset 동작을 테스트하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Time Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `TimeManager` 또는 `Core.Time` 서비스가 필요합니다.

## 주요 기능
- `eTimeScaleMethods` 기반 Time Scale 적용
- Lerp / Infinite 옵션 테스트
- Freeze Frame 실행
- Freeze 해제
- Time Scale 초기화
- 현재 `Time.timeScale` 표시

## 주요 입력값
- `TimeScale Method`
- `Time Scale`
- `Duration`
- `Lerp`
- `Lerp Speed`
- `Infinite`
- `Freeze Duration`

## 주요 버튼
- `Apply Time Scale`
- `Freeze Frame`
- `Unfreeze`
- `Reset Time`

## 참고
- 창을 닫으면 `ResetTimeScale()`을 호출해 시간 값을 원복합니다.
- 음수 입력은 내부에서 0 이상 값으로 보정됩니다.
