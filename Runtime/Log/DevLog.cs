using UnityEngine;

namespace PhikozzLib
{
    public static class DevLog
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Info(object message, Object context = null)
        {
            Debug.Log(message, context);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Warning(object message, Object context = null)
        {
            Debug.LogWarning(message, context);
        }
    }
}
