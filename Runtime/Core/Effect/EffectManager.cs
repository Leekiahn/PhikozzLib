using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace PhikozzLib
{
    public class EffectManager : MonoBehaviour, IEffectService, IServiceRegister
    {
        [SerializeField] private EffectDatabase _effectDatabase;
        
        [PropertySpace(SpaceBefore = 20f)]
        [Title("Effect Pool")]
        [SerializeField] private int _effectPoolCapacity = 10;
        [SerializeField] private int _effectPoolMaxSize = 20;

        private Transform _effectParent;
        private readonly Dictionary<string, Dictionary<string, TrackedPool<ParticleSystem>>> _effectPools = new();

        private void Awake()
        {
            _effectParent = transform;

            foreach (var effectGroup in _effectDatabase.EffectGroups)
            {
                string effectKey = effectGroup.Key;
                var particlePools = new Dictionary<string, TrackedPool<ParticleSystem>>();
            
                foreach (var particleData in effectGroup.Particles)
                {
                    string particleKey = particleData.ParticleKey;
                    ParticleSystem prefab = particleData.ParticleSystem;
                    
                    particlePools.Add(particleKey, CreatePool(prefab));
                }
                
                _effectPools.Add(effectKey, particlePools);
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

        [PropertySpace(SpaceBefore = 20f)]
        [Button(ButtonSizes.Medium, ButtonStyle.Box)]
        public ParticleSystem Play(string categoryKey, string particleKey, Vector3 position, Quaternion rotation, float duration = 0f, Transform attachToTransform = null)
        {
            if (_effectPools.TryGetValue(categoryKey, out var particlePools) && particlePools.TryGetValue(particleKey, out var pool))
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