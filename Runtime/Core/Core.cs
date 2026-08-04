namespace PhikozzLib
{
    public static class Core 
    {
        public static IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
        public static IUIService UI => ServiceLocator.Get<IUIService>();
        public static IDataService Data => ServiceLocator.Get<IDataService>();
        public static IEventService Event => ServiceLocator.Get<IEventService>();
        public static ISceneService Scene => ServiceLocator.Get<ISceneService>();
        public static IEffectService Effect => ServiceLocator.Get<IEffectService>();
        public static ISaveService Save => ServiceLocator.Get<ISaveService>();
        public static ITimeService Time => ServiceLocator.Get<ITimeService>();
        public static ILocalizationService Local => ServiceLocator.Get<ILocalizationService>();
        public static IFloatingTextService FloatingText => ServiceLocator.Get<IFloatingTextService>();
    }
}

