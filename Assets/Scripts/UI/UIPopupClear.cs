using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupClear : UIPopup
{
    public Button btnRetry;
    public Button btnExit;
    public Button btnNext;

    private void Start ()
    {
        btnRetry.onClick.AddListener (OnClickRetryButton);
        btnExit.onClick.AddListener (OnClickExitButton);
        btnNext.onClick.AddListener (OnClickNextButton);
    }

    private void OnClickRetryButton()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void OnClickExitButton ()
    {

    }

    private void OnClickNextButton()
    {

    }
}
