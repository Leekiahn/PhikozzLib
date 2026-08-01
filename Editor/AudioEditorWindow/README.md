# AudioEditorWindow
> BGM, SFX, UI 사운드, 트랙 제어, 페이드 기능을 테스트하는 EditorWindow입니다.

---

## 접근 경로
- `PhikozzLib/Audio Editor Window`

## 사용 조건
- **Play Mode에서만 사용 가능**
- `AudioManager`가 필요합니다.
- 내부적으로 `PlaylistDatabase`, `SoundDatabase`, `MMSMPlaylistManager` 상태를 활용합니다.

## 주요 기능
- 채널/곡 기반 BGM 재생 및 제어
- SFX / UI 사운드 키 선택 재생
- 트랙별 재생, 정지, 음소거, 볼륨 제어
- 전체 트랙 일괄 제어
- 트랙 페이드 테스트
- 등록된 플레이리스트/SFX/UI 키 목록 조회

## 주요 섹션
- `Current BGM Status`
- `BGM Test`
- `SFX / UI Test`
- `Track / Fade Test`
- `Audio Lists`

## 참고
- `Master` 트랙은 Fade 테스트를 지원하지 않습니다.
- 데이터베이스에 등록된 키가 없으면 해당 테스트 UI는 읽기 전용 안내만 표시합니다.
