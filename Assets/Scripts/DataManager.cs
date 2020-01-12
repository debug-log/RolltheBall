using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager
{
    public static void SaveData<T> (T t, string filePath) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream stream = new FileStream (Path.Combine (Application.persistentDataPath, filePath), FileMode.Create);
        formatter.Serialize (stream, t);
        stream.Close ();
    }

    public static T LoadData<T> (string filePath) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream stream = new FileStream (Path.Combine (Application.persistentDataPath, filePath), FileMode.OpenOrCreate);
        if(stream.Length == 0)
        {
            stream.Close ();
            return null;
        }

        T t = (T) formatter.Deserialize (stream);
        stream.Close ();

        return t;
    }
}