namespace PhikozzLib
{
    public static class Core 
    {
        public static IAddressableService Addressable => ServiceLocator.Get<IAddressableService>();
        
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
        
        public static IUIService UI => ServiceLocator.Get<IUIService>();
        public static IPopupService Popup => ServiceLocator.Get<IPopupService>();
        public static IDialogService Dialog => ServiceLocator.Get<IDialogService>();
        
        public static DataManager Data => ServiceLocator.Get<DataManager>();
        
        public static IEventService Event => ServiceLocator.Get<IEventService>();
        
        public static IPoolService Pool => ServiceLocator.Get<IPoolService>();
            
            
            
            
        
        public static ISaveService Save => ServiceLocator.Get<ISaveService>();
        public static ISceneService Scene => ServiceLocator.Get<ISceneService>();

    }
}

