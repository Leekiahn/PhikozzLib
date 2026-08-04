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
        private IFloatingTextService _floatingTextService;
        
        private void Awake()
        {
            _spawner = GetComponent<MMFloatingTextSpawner>();
            _floatingTextService = ServiceLocator.Get<IFloatingTextService>();
        }

        private void Start()
        {
            _floatingTextService.RegisterFloatingText(this);
        }

        private void OnDisable()
        {
            _floatingTextService.UnRegisterFloatingText(FloatingTextKey);
        }

        private void OnDestroy()
        {
            _floatingTextService.UnRegisterFloatingText(FloatingTextKey);
        }
    }
}