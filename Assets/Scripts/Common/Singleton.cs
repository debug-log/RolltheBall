using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _shuttingDown = false;
    public static T _instance;

    public static T Instance
    {
        get
        {
            if(_shuttingDown)
            {
                Debug.LogWarning ("[Singleton] Instance '" + typeof (T) + "' already destroyed. Returning null.");
                return null;
            }

            if(_instance == null)
            {
                var singletonObject = new GameObject ();
                _instance = singletonObject.AddComponent<T> ();
                singletonObject.name = typeof (T).ToString () + " (Singleton)";

                DontDestroyOnLoad (singletonObject);
            }

            return _instance;
        }
    }

    private void OnApplicationQuit ()
    {
        _shuttingDown = true;
    }

    private void OnDestroy ()
    {
        _shuttingDown = true;
    }
}
