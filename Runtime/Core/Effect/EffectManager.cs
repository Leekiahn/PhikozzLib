using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class EffectManager : MonoBehaviour, IEffectService, IServiceRegister, IServiceInit
    {
        [SerializeField] private bool _loadByAddressableService;

        [ShowIf("_loadByAddressableService")]
        [SerializeField] private AssetLabelReference _effectLabel;

        [PropertySpace(SpaceBefore = 20f)]
        [Title("Effect Pool")]
        [SerializeField] private int _effectPoolCapacity = 10;
        [SerializeField] private int _effectPoolMaxSize = 20;

        private Transform _effectParent;
        private readonly Dictionary<string, TrackedPool<ParticleSystem>> _effectPools = new();
        private IAddressableService _addressableService;

        private void Awake()
        {
            _effectParent = transform;
        }

        public async UniTask InitAsync()
        {
            if (_loadByAddressableService)
            {
                _addressableService = ServiceLocator.Get<IAddressableService>();

                try
                {
                    await PreloadEffectByAddressableService();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"EffectManager initialization failed: {ex}");
                }
            }
        }

        private async UniTask PreloadEffectByAddressableService()
        {
            await _addressableService.PreloadLocations<GameObject>(_effectLabel.labelString);
            await _addressableService.PreloadAssets<GameObject>(_effectLabel.labelString);

            var effectPrefabs = _addressableService.GetAll<GameObject>(_effectLabel.labelString);

            foreach (var prefab in effectPrefabs)
            {
                var key = prefab.name;
                if (!_effectPools.ContainsKey(key))
                {
                    _effectPools[key] = CreatePool(prefab.GetComponent<ParticleSystem>());
                }
            }
        }

        public void RegisterEffect(string key, ParticleSystem prefab)
        {
            if (!_effectPools.ContainsKey(key))
            {
                _effectPools[key] = CreatePool(prefab);
            }
        }

        public void UnregisterEffect(string key)
        {
            if (_effectPools.ContainsKey(key))
            {
                _effectPools.Remove(key);
            }
        }

        private TrackedPool<ParticleSystem> CreatePool(ParticleSystem prefab)
        {
            return new TrackedPool<ParticleSystem>
            (
                onCreate: () =>
                {
                    var particle = Instantiate(prefab, _effectParent);
                    particle.gameObject.SetActive(false);
                    return particle;
                },
                onGet: particle =>
                {
                    particle.gameObject.SetActive(true);
                    particle.Clear(true);
                },
                onRelease: particle => particle.gameObject.SetActive(false),
                onDestroy: particle => Destroy(particle.gameObject),
                defaultCapacity: _effectPoolCapacity,
                maxSize: _effectPoolMaxSize
            );
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IEffectService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<IEffectService>();
        }

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public ParticleSystem Play(string key, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null)
        {
            if (_effectPools.TryGetValue(key, out var pool))
            {
                var particle = pool.Get();

                var tr = particle.transform;

                if (attachToTransform != null)
                {
                    tr.SetParent(attachToTransform);
                    tr.localPosition = Vector3.zero;
                    tr.localRotation = Quaternion.identity;
                }
                else
                {
                    tr.SetParent(_effectParent);
                    tr.SetPositionAndRotation(position, rotation);
                }

                particle.Play(true);

                if (duration > 0f)
                {
                    ReleaseAfterDurationAsync(pool, particle, duration).Forget();
                }
                else
                {
                    ReleaseAsync(pool, particle).Forget();
                }

                return particle;
            }

            return null;
        }


        private async UniTaskVoid ReleaseAsync(TrackedPool<ParticleSystem> pool, ParticleSystem particle)
        {
            await UniTask.WaitUntil(() => particle == null || !particle.IsAlive(true));

            if (particle == null) return;
            particle.transform.SetParent(_effectParent);
            pool.Release(particle);
        }

        private async UniTaskVoid ReleaseAfterDurationAsync(TrackedPool<ParticleSystem> pool, ParticleSystem particle, float duration)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            await UniTask.WaitUntil(() => particle == null || !particle.IsAlive(true));

            if (particle == null) return;
            particle.transform.SetParent(_effectParent);
            pool.Release(particle);
        }
    }
}
