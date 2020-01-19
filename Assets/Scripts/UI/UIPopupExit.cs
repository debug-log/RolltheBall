using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupExit : UIPopup
{
    public Button btnCancel;
    public Button btnExit;

    private void Start ()
    {
        btnCancel.onClick.AddListener (OnClickCancelButton);
        btnExit.onClick.AddListener (OnClickExitButton);
    }

    private void OnClickCancelButton ()
    {
        this.Close ();
    }

    private void OnClickExitButton ()
    {
        Application.Quit ();
    }
}
