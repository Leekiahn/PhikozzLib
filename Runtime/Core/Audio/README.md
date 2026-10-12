# AudioManager

> BGM과 SFX를 key로 등록·재생하고 AudioMixer 볼륨을 제어하는 오디오 서비스입니다. (`IAudioService`)

## Public API

| Method | Description |
|---|---|
| `RegisterAudio(string key, AudioClip clip)` | 클립을 key로 등록합니다. 이미 있는 key면 기존 클립을 유지합니다. |
| `UnregisterAudio(string key)` | 클립 등록을 해제합니다. 재생 중인 BGM이면 정지합니다. |
| `PlayBgm(string key)` | BGM을 반복 재생하고 `AudioSource`를 반환합니다. 기존 BGM은 정지합니다. |
| `StopBgm()` | BGM을 정지합니다. |
| `PlaySfx(string key, Vector3 position, float spatialBlend = 0f, float volume = 1f, float pitch = 1f)` | SFX를 재생하고 `AudioSource`를 반환합니다. `spatialBlend` 0이면 2D, 1이면 3D입니다. |
| `SetMasterVolume(float volume)` / `SetBgmVolume(float volume)` / `SetSfxVolume(float volume)` | 각 Mixer 그룹 볼륨을 0~1로 설정합니다. |

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
    _audioService.PlaySfx("Hit", hitPoint, spatialBlend: 1f);
}
```

## 주의사항

- **AudioMixer 설정이 필요합니다.** `Master` / `BGM` / `SFX` 그룹을 만들고 각 Volume을 `MasterVolume` / `BgmVolume` / `SfxVolume`으로 Expose한 뒤, `AudioManager`에 Mixer와 그룹을 할당합니다.
- `Load By Addressable Service`를 켜면 `Audio Clip Label` 라벨의 클립이 Addressable 주소를 key로 자동 등록됩니다. 이때 `BootstrapConfig`에 `AddressableManager`도 등록해야 합니다.
- 끄면 `RegisterAudio`로 직접 등록합니다. `ExampleAudioLoader.cs`(전체 주석)를 프로젝트로 복사해서 쓰면 됩니다.
- `UnregisterAudio`는 이미 재생 중인 SFX를 멈추지 않습니다.
