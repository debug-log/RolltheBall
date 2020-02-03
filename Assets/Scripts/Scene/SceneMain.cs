using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class SceneMain : MonoBehaviour
{
    public Button backBtn;
    public Text textStageTitle;
    public UIStageBox[] stageBoxes;
    public UIPopupStage popupStage;

    private void Start ()
    {
        Player.Instance.LoadPlayerData ();
        StageInfo.Instance.LoadStageInfoData ();

        textStageTitle.text = StageInfo.Instance.GetMainStageName (Player.Instance.GetPlayerSelectedMainStageId ());

        var stageDatas = Player.Instance.GetPlayerStageDataList (Player.Instance.GetPlayerSelectedMainStageId ());
        for (int i = 0; i < stageDatas.Count && i < stageBoxes.Length; i++) 
        {
            var stage = stageDatas[i];
            stageBoxes[i].Init (stage.stageId % 100, stage.numStars, stage.cleared);
        }

        for (int i = 0; i < stageBoxes.Length; i++) 
        {
            int boxId = i + 1;
            stageBoxes[i].button.onClick.AddListener (() => OnClickStageBox (boxId));
        }
        backBtn.onClick.AddListener (OnClickBackButton);

        UnityAdsHelper.Instance.ShowBanner ();
    }

    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBackButton ();
        }
    }

    private void OnClickStageBox(int boxId)
    {
        int stageId = Player.Instance.GetPlayerSelectedMainStageId () * 100 + boxId;
        if (Player.Instance.IsPlayerStageDataCleared(stageId) == false)
        {
            popupStage.SetStageContentTextDisabled (stageId);
            popupStage.Open ();
            return;
        }

        Player.Instance.SetPlayerSelectedStageId (stageId);

        popupStage.SetStageContentText (stageId);
        popupStage.Open ();
    }

    private void OnClickBackButton()
    {
        SceneManager.LoadScene (SceneName.SCENE_TITLE);
    }
}
