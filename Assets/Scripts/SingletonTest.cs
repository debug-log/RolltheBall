using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log (GameManager.Instance.name);
    }
}
