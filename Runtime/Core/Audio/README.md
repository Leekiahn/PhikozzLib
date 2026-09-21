# AudioManager

> BGM과 SFX를 key 기반으로 등록·재생하는 오디오 서비스입니다.  
> `IAudioService`를 구현하며, SFX는 `TrackedPool<AudioSource>`로 동시 재생합니다.

<br>

## 로드 방식 설정

### AudioMixer 설정

1. AudioMixer에 `Master`, `BGM`, `SFX` 그룹을 만들고 각 그룹의 Volume을 `MasterVolume`, `BgmVolume`, `SfxVolume`으로 Expose to script 합니다.
2. `AudioManager`에 AudioMixer와 BGM/SFX Mixer Group을 할당합니다. 노출 파라미터 이름을 바꿨다면 Inspector의 각 Parameter 필드에도 같은 이름을 입력합니다.

`AudioManager` Inspector의 Volume 슬라이더는 변경 즉시 AudioMixer에 반영됩니다. 런타임에서는 `SetMasterVolume`/`SetBgmVolume`/`SetSfxVolume`을 사용합니다.

### Addressables 자동 등록

`AudioManager` Inspector에서 `Load By Addressable Service`를 켜면 라벨의 `AudioClip`을 자동으로 등록합니다.

1. `BootstrapConfig`에 `AddressableManager`와 `AudioManager`를 등록합니다.
2. 재생할 `AudioClip`을 Addressables에 등록하고 하나의 라벨을 지정합니다.
3. `AudioManager`의 `Audio Clip Label`에 해당 라벨을 지정합니다.
4. Addressable key를 고유하게 설정합니다. 해당 key가 `PlayBgm`과 `PlaySfx`의 key가 됩니다.

### 외부 데이터 로더 등록

`Load By Addressable Service`를 끄면 외부 데이터 로더에서 `RegisterAudio(key, clip)`을 호출해 클립을 등록합니다. `ExampleAudioLoader.cs`는 BGDatabase 등의 프로젝트별 데이터 타입에 맞춰 복사해서 사용할 수 있도록 전체를 주석 처리한 예시입니다.

<br>

## 주요 기능

- 단일 BGM 채널 재생 및 정지
- 여러 SFX 동시 재생과 자동 풀 반환
- 2D SFX 및 지정 위치의 3D SFX 재생
- AudioMixer의 Master/BGM/SFX 그룹 볼륨 제어

<br>

## Public API

| Method | Description |
| --- | --- |
| `RegisterAudio(string key, AudioClip clip)` | key로 오디오 클립을 등록합니다. 같은 key가 이미 등록되어 있으면 기존 클립을 유지합니다. |
| `UnregisterAudio(string key)` | 등록된 클립을 제거합니다. 현재 해당 클립을 BGM으로 재생 중이면 정지합니다. |
| `PlayBgm(string key)` | key의 클립을 반복 재생합니다. 기존 BGM은 즉시 정지합니다. |
| `StopBgm()` | 현재 BGM을 정지하고 클립 참조를 제거합니다. |
| `PlaySfx(string key, float volume = 1f, float pitch = 1f)` | key의 클립을 2D SFX로 재생합니다. |
| `PlaySfxAtPosition(string key, Vector3 position, float volume = 1f, float pitch = 1f)` | key의 클립을 지정한 월드 위치에서 3D SFX로 재생합니다. |
| `SetMasterVolume(float volume)` | AudioMixer Master 그룹 볼륨을 0~1 범위로 설정합니다. |
| `SetBgmVolume(float volume)` | AudioMixer BGM 그룹 볼륨을 0~1 범위로 설정합니다. |
| `SetSfxVolume(float volume)` | AudioMixer SFX 그룹 볼륨을 0~1 범위로 설정합니다. |

<br>

## 사용 예시

```csharp
private IAudioService _audioService;

private void Start()
{
    _audioService = ServiceLocator.Get<IAudioService>();
    _audioService.PlayBgm("MainTheme");
}

public void OnHit(Vector3 hitPoint)
{
    _audioService.PlaySfxAtPosition("Hit", hitPoint);
}
```
