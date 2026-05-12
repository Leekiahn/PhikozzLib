namespace PhikozzLib
{
    public interface IPoolable
    {
        void OnCreate();
        void OnGet();
        void OnRelease();
        void OnDestroy();
    }
}