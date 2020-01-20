using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupFail : UIPopup
{
    public Button btnRetry;
    public Button btnExit;

    private void Start ()
    {
        btnRetry.onClick.AddListener (OnClickRetryButton);
        btnExit.onClick.AddListener (OnClickExitButton);
    }

    private void OnClickRetryButton ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void OnClickExitButton ()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
    }
}
