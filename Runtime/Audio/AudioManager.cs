using UnityEngine;
using UnityEngine.Audio;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;

        private const string MasterParameter = "Master";
        private const string SfxParameter = "Sfx";
        private const string BgmParameter = "Bgm";

        private const float MinDb = -80f;
        private const float MaxDb = 20f;

        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        #region AudioSource

        public void PlaySfx(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayBgm(AudioClip clip, bool loop = true)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            _bgmSource.Play();
        }

        public void PauseBgm()
        {
            _bgmSource.Pause();
        }

        public void ResumeBgm()
        {
            _bgmSource.UnPause();
        }

        public void StopBgm()
        {
            _bgmSource.Stop();
        }

        #endregion

        #region AudioMixer

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

        #endregion
    }
}