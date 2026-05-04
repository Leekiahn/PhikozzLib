using System;
using UnityEngine;

public class SingletonGlobal<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    
    public static T Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
