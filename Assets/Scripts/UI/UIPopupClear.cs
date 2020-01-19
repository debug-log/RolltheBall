using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupClear : UIPopup
{
    public UIImageSwitcher[] imageStars;

    public Button btnRetry;
    public Button btnExit;
    public Button btnNext;

    public void Activate (int numStars)
    {
        this.gameObject.SetActive (true);

        for (int i = 0; i < numStars && i < imageStars.Length; i++)
        {
            imageStars[i].SetImageChanged ();
        }
    }

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
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
    }

    private void OnClickNextButton()
    {

    }
}
