using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    public void Open()
    {
        this.gameObject.SetActive (true);
    }

    public void Close()
    {
        this.gameObject.SetActive (false);
    }
}
