namespace PhikozzLib
{
    public static class Core 
    {
        public static IAudioService Audio => ServiceLocator.Get<IAudioService>();
    }
}

