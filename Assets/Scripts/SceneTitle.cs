using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitle : MonoBehaviour
{
    void Start()
    {
        Player.Instance.LoadPlayerData ();
    }
}
