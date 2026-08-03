using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace PhikozzLib
{
    public abstract class BaseFloatingTextLoader : MonoBehaviour
    {
        public abstract string FloatingTextKey { get; }
        private MMFloatingTextSpawner _spawner;

        public MMFloatingTextSpawner Spawner => _spawner;
        
        private void Awake()
        {
            _spawner = GetComponent<MMFloatingTextSpawner>();
        }

        private void Start()
        {
            Core.FloatingText.RegisterFloatingText(this);
        }

        private void OnDisable()
        {
            Core.FloatingText.UnRegisterFloatingText(FloatingTextKey);
        }

        private void OnDestroy()
        {
            Core.FloatingText.UnRegisterFloatingText(FloatingTextKey);
        }
    }
}