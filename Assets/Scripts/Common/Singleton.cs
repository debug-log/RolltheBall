using UnityEngine;

public abstract class Singleton<T> where T : class
{
    protected static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = System.Activator.CreateInstance (typeof (T)) as T;
            }
            return _instance;
        }
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
                var singletonObject = GameObject.FindObjectOfType<T> ();
                if (singletonObject != null)
                {
                    _instance = singletonObject;
                }
                else
                {
                    var newObject = new GameObject ();
                    _instance = newObject.AddComponent<T> ();
                    newObject.name = typeof (T).ToString () + " (Singleton)";

                    DontDestroyOnLoad (newObject);
                }
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
