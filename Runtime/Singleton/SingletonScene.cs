using UnityEngine;

public class SingletonScene<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    
    public static T Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
