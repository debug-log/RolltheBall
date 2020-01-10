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

    private void Start ()
    {
        btnPause.onClick.AddListener (OnClickPauseButton);
        btnReset.onClick.AddListener (OnClickResetButton);
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
    }

    private void OnGameResumed()
    {
        imagePause.SetImageDefault ();
    }
}
