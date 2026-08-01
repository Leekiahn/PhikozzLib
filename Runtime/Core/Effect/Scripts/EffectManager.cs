using System.Collections.Generic;
using PhikozzLib;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PhikozzLib
{
    public class EffectManager : MonoBehaviour, IEffectService, IServiceRegister
    {
        [SerializeField] private EffectDatabase _effectDatabase;

        private Transform _effectParent;
        private readonly Dictionary<string, TrackedPool<ParticleSystem>> _effectPools = new();

        private void Awake()
        {
            _effectParent = transform;

            foreach (var particleValue in _effectDatabase.ParticleSystemDic)
            {
                string effectKey = particleValue.Key;
                ParticleSystem prefab = particleValue.Value;

                var pools = new TrackedPool<ParticleSystem>
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
                    onDestroy: particle => Destroy(particle.gameObject)
                );

                _effectPools.Add(effectKey, pools);
            }
        }

        public void RegisterService()
        {
            ServiceLocator.Register<IEffectService>(this);
        }

        public ParticleSystem Play(string effectKey, Vector3 position, Quaternion rotation,
            Transform attachToTransform = null)
        {
            if (_effectPools.TryGetValue(effectKey, out var pool))
            {
                var effectPlayer = pool.Get();

                var tr = effectPlayer.transform;

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

                effectPlayer.Play(true);

                ReleaseAsync(pool, effectPlayer).Forget();

                return effectPlayer;
            }

            return null;
        }

        private async UniTaskVoid ReleaseAsync(TrackedPool<ParticleSystem> pool, ParticleSystem particle)
        {
            await UniTask.WaitUntil(() => !particle.IsAlive(true));

            particle.transform.SetParent(_effectParent);
            pool.Release(particle);
        }
    }
}