using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour, IAudioSettings
{
    [SerializeField] private AudioMixer _audioMixer;

    private const string MasterParameter = "Master";
    private const string SfxParameter = "Sfx";
    private const string BgmParameter = "Bgm";

    private const float MinDb = -80f;
    private const float MaxDb = 20f;

    public float GetMasterVolume()
    {
        return GetNormalizedVolume(MasterParameter);
    }

    public float GetSfxVolume()
    {
        return GetNormalizedVolume(SfxParameter);
    }

    public float GetBgmVolume()
    {
        return GetNormalizedVolume(BgmParameter);
    }

    public void SetMasterVolume(float volume)
    {
        SetNormalizedVolume(MasterParameter, volume);
    }

    public void SetSfxVolume(float volume)
    {
        SetNormalizedVolume(SfxParameter, volume);
    }

    public void SetBgmVolume(float volume)
    {
        SetNormalizedVolume(BgmParameter, volume);
    }

    private float GetNormalizedVolume(string parameterName)
    {
        _audioMixer.GetFloat(parameterName, out var db);
        return Mathf.InverseLerp(MinDb, MaxDb, db);
    }

    private void SetNormalizedVolume(string parameterName, float normalizedVolume)
    {
        normalizedVolume = Mathf.Clamp01(normalizedVolume);
        float db = Mathf.Lerp(MinDb, MaxDb, normalizedVolume);
        _audioMixer.SetFloat(parameterName, db);
    }
}