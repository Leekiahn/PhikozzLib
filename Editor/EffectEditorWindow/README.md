# EffectEditorWindow
> 등록된 이펙트 키를 확인하고 위치 또는 대상 부착 방식으로 이펙트를 재생하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Effect Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `EffectManager` 또는 `Core.Effect` 서비스가 필요합니다.

## 주요 기능
- Effect Key 기반 파티클 재생
- 월드 좌표 기준 재생
- Transform 부착 재생
- 등록된 이펙트 키 목록 조회

## 주요 입력값
- `Effect Key`
- `Position`
- `Rotation`
- `Attach Target`

## 주요 버튼
- `Play At Position`
- `Play Attached`

## 참고
- 등록되지 않은 `Effect Key`는 재생할 수 없습니다.
- 부착 재생은 `Attach Target`이 지정되어야 합니다.
