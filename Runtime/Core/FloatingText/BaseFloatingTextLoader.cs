using System;
using MoreMountains.Feedbacks;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace PhikozzLib
{
    public abstract class BaseFloatingTextLoader : MonoBehaviour
    {
        private MMFloatingTextSpawner _spawner;

        public abstract string FloatingTextKey { get; }
        
        public MMFloatingTextSpawner Spawner => _spawner;
        private IFloatingTextService FloatingTextService => ServiceLocator.Get<IFloatingTextService>();
        
        private void Awake()
        {
            _spawner = GetComponent<MMFloatingTextSpawner>();
        }

        private void Start()
        {
            FloatingTextService.RegisterFloatingText(this);
        }

        private void OnDisable()
        {
            FloatingTextService.UnRegisterFloatingText(FloatingTextKey);
        }

        private void OnDestroy()
        {
            FloatingTextService.UnRegisterFloatingText(FloatingTextKey);
        }
    }
}