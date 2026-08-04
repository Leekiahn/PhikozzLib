# Core.Audio
> `AudioManager`는 PhikozzLib에서 **BGM**, **SFX**, **UI 사운드**, **트랙 제어**, **페이드 처리**를 담당하는 오디오 서비스입니다.  
> `IAudioService` 인터페이스를 상속받아 구현합니다.  
> `AudioManager`는 MoreMountains의 FEEL 에셋에 의존하고 있습니다. Feel 에셋에 의존하지 않는 다른 매니저로 교체가 가능합니다.  


<br>

## 주요 기능
- 채널 기반 BGM 재생
- SFX/UI 사운드 이름 기반 재생
- 특정 트랙 음소거, 정지, 재생, 볼륨 제어
- 전체 사운드 일괄 제어
- 트랙 페이드 인/아웃
- 오디오 데이터베이스(`PlaylistDatabase`, `SoundDatabase`)를 `Resources`폴더에 생성하면 자동으로 참조합니다. 이름을 바꾸지 마세요.
- FEEL에셋의 `MMSoundManager`와 `MMSMPlaylistManager`를 통해 오디오 소스를 풀링합니다.

<br>

## Public API

| Method | Description |
| --- | --- |
| `PlayBgm(int channelKey, int index)` | 채널 키에 매핑된 플레이리스트를 재생하고, 지정한 인덱스 곡으로 시작합니다. |
| `StopBgm()` | 현재 재생 중인 BGM을 정지합니다. |
| `PauseBgm()` | 현재 BGM을 일시정지합니다. |
| `ResumeBgm()` | 일시정지된 BGM을 다시 재생합니다. |
| `PlayNextBgm()` | 현재 플레이리스트의 다음 곡을 재생합니다. |
| `PlayPreviousBgm()` | 현재 플레이리스트의 이전 곡을 재생합니다. |
| `SetBgmMultiplier(float volume, float pitch, bool instantly = true)` | BGM 볼륨 배수와 피치 배수를 설정합니다. |
| `PlaySfx(string soundName, Vector3 position = default, Transform attachToTransform = null)` | SFX를 이름으로 찾아 위치 기반 또는 Transform 부착 방식으로 재생합니다. |
| `PlayUi(string soundName)` | UI 사운드를 이름으로 찾아 재생합니다. |
| `ControlTrack(eSoundTrackEventTypes type, eSoundTracks track, float volume = 1f)` | 특정 오디오 트랙에 대해 재생, 정지, 음소거, 볼륨 변경 등을 수행합니다. |
| `ControlAllTrack(eAllSoundControlEventTypes type)` | 전체 사운드에 대해 일괄 재생, 정지, 해제 등을 수행합니다. |
| `FadeTrack(eSoundTrackFadeEventModes mode, eSoundTracks track, float fadeDuration, float finalVolume, eFadeTrackTweenType fadeTween)` | 특정 트랙에 페이드 인/아웃을 적용합니다. |

<br>

## 관련 열거형 타입

### eSoundTrackEventTypes
- MuteTrack
- UnmuteTrack
- SetVolumeTrack
- PlayTrack
- PauseTrack
- StopTrack
- FreeTrack

<br>

### eSoundTracks
- Sfx
- Music
- UI
- Master
- Other

<br>

### eAllSoundControlEventTypes
- Pause
- Play
- Stop
- Free
- FreeAllButPersistent
- FreeAllLooping

<br>

### eSoundTrackFadeEventModes
- PlayFade
- StopFade

<br>

### eFadeTrackTweenType
- LinearTween
- EaseInQuadratic
- EaseOutQuadratic
- EaseInOutQuadratic
- EaseInCubic
- EaseOutCubic
- EaseInOutCubic
- EaseInQuartic
- EaseOutQuartic
- EaseInOutQuartic
- EaseInQuintic
- EaseOutQuintic
- EaseInOutQuintic
- EaseInSinusoidal
- EaseOutSinusoidal
- EaseInOutSinusoidal
- EaseInBounce
- EaseOutBounce
- EaseInOutBounce
- EaseInOverhead
- EaseOutOverhead
- EaseInOutOverhead
- EaseInExponential
- EaseOutExponential
- EaseInOutExponential
- EaseInElastic
- EaseOutElastic
- EaseInOutElastic
- EaseInCircular
- EaseOutCircular
- EaseInOutCircular
- AntiLinearTween
- AlmostIdentity

<br>

- `IAudioService`는 FEEL 에셋에 직접 의존하지 않으며,  
내부 enum을 통해 오디오 제어 요청을 추상화하고 `AudioManager`에서 실제 FEEL 타입으로 매핑합니다.

















