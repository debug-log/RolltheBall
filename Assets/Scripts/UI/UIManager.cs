using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    public Button btnPause;
    public Button btnReset;

    public UIImageSwitcher imagePause;
    public UIImageSwitcher[] imageStars;

    public UIPopup popupClear;
    public UIPopup popupPause;
    public Text textStageId;

    private void Start ()
    {
        btnPause.onClick.AddListener (OnClickPauseButton);
        btnReset.onClick.AddListener (OnClickResetButton);
    }

    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (popupPause.IsOpened ())
            {
                OnGameResumed ();
            }
            else
            {
                OnGamePaused ();
            }
        }
    }

    public void SetStageText(string stageName)
    {
        if(stageName.StartsWith("stage"))
        {
            var stageNum = int.Parse(stageName.Substring (5));
            textStageId.text = string.Format ("{0}-{1}", stageNum / 100, stageNum % 100);
            return;
        }
        textStageId.text = stageName;
    }

    public void GetStar(int index)
    {
        if (index >= imageStars.Length || index < 0)
            return;

        imageStars[index].SetImageChanged ();
    }

    private void OnClickResetButton()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void OnClickPauseButton()
    {
        if(imagePause.IsImageChanged() == true)
        {
            OnGameResumed ();
        }
        else
        {
            OnGamePaused ();
        }
    }

    private void OnGamePaused()
    {
        imagePause.SetImageChanged ();
        popupPause.Open ();
    }

    private void OnGameResumed()
    {
        imagePause.SetImageDefault ();
        popupPause.Close ();
    }

    public void HideTopButtons ()
    {
        btnPause.gameObject.SetActive (false);
        btnReset.gameObject.SetActive (false);
    }
}
