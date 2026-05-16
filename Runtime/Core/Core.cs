namespace PhikozzLib
{
    public static class Core 
    {
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
        public static ISaveService Save => ServiceLocator.Get<ISaveService>();
        public static IResourceService Resource => ServiceLocator.Get<IResourceService>();
        public static ISceneService Scene => ServiceLocator.Get<ISceneService>();
        public static IEventService Event => ServiceLocator.Get<IEventService>();
        public static IPoolService Pool => ServiceLocator.Get<IPoolService>();
        public static IUIService UI => ServiceLocator.Get<IUIService>();
    }
}

