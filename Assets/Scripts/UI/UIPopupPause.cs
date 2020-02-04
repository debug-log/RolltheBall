using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupPause : UIPopup
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
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void OnClickExitButton ()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }
}
