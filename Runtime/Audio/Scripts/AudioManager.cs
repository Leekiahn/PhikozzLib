using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class AudioManager : MonoBehaviour, IAudioService, IServiceRegister
    {
        [SerializeField] private AudioDatabase _audioDatabase;
        [SerializeField] private AudioPlayer _audioPlayer;

        private TrackedPool<AudioPlayer> _pool;
        
        public void RegisterService()
        {
            ServiceLocator.Register<IAudioService>(this);
        }

        public async UniTask Load(string label)
        {
            _pool = new TrackedPool<AudioPlayer>(
                onCreate: () =>
                {
                    var player = Instantiate(_audioPlayer, transform);
                    return player;
                },
                onGet: (player) =>
                {
                    player.gameObject.SetActive(true);
                },
                onRelease: (player) =>
                {
                    player.Stop();
                    player.gameObject.SetActive(false);
                },
                onDestroy: (player) =>
                {
                    Destroy(player.gameObject);
                });
        }

    }
}