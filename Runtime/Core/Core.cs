namespace PhikozzLib
{
    public static class Core 
    {
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
        public static ISaveService Save => ServiceLocator.Get<ISaveService>();
    }
}

