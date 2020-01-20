using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (popupExit.IsOpened () == true)
            {
                popupExit.Close ();
            }
            else
            {
                popupExit.Open ();
            }
        }
    }
#endif

    void OnClickButtonStage()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
    }

    void OnClickButtonGame()
    {
        SceneManager.LoadScene (SceneName.SCENE_GAME);
    }

    private void InitStageText ()
    {
        int nextStageId = Player.Instance.GetNextStageId ();
        string mainStageTitle = StageInfo.Instance.GetMainStageName (Player.Instance.GetNextStageMainId ());
        textStageDesc.text = string.Format ("{0} {1:D2}", mainStageTitle, nextStageId % 100);
    }
}
