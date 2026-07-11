namespace PhikozzLib
{
    public static class Core 
    {
        public static IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
        public static ISaveService Save => ServiceLocator.Get<ISaveService>();
        public static ISceneService Scene => ServiceLocator.Get<ISceneService>();
        public static IEventService Event => ServiceLocator.Get<IEventService>();
        public static IPopupService Popup => ServiceLocator.Get<IPopupService>();
    }
}

