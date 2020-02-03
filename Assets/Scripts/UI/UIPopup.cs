using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    private Action onOpenAction = null;

    public void Open()
    {
        this.gameObject.SetActive (true);

        if (onOpenAction != null)
        {
            onOpenAction.Invoke ();
        }
    }

    public void Close()
    {
        this.gameObject.SetActive (false);
    }

    public bool IsOpened()
    {
        return this.gameObject.activeSelf;
    }

    public void OpenWithDelay (float delay, Action onOpenAction)
    {
        this.onOpenAction = onOpenAction;

        Invoke ("Open", delay);
    }
}
