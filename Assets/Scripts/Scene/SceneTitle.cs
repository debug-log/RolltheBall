using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class SceneTitle : MonoBehaviour
{
    public UIPopup popupExit;

    public Button btnStage;
    public Button btnGame;

    public Text textStageDesc;

    void Start()
    {
        Player.Instance.LoadPlayerData ();
        StageInfo.Instance.LoadStageInfoData ();

        btnStage.onClick.AddListener (OnClickButtonStage);
        btnGame.onClick.AddListener (OnClickButtonGame);

        InitStageText ();

        UnityAdsHelper.Instance.HideBanner ();
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (popupExit.IsOpened () == true)
            {
                popupExit.Close ();
                AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
            }
            else
            {
                popupExit.Open ();
                AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
            }
        }
    }
#endif

    void OnClickButtonStage()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    void OnClickButtonGame()
    {
        Player.Instance.SetPlayerSelectedStageId (Player.Instance.GetPlayerNextStageId ());
        SceneManager.LoadScene (SceneName.SCENE_GAME);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void InitStageText ()
    {
        int nextStageId = Player.Instance.GetPlayerNextStageId ();
        string mainStageTitle = StageInfo.Instance.GetMainStageName (Player.Instance.GetNextStageMainId ());
        textStageDesc.text = string.Format ("{0} {1:D2}", mainStageTitle, nextStageId % 100);
    }
}
