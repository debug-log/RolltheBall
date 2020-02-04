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
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.Fail);
    }

    private void OnClickRetryButton ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void OnClickExitButton ()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }
}
