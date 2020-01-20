using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupStage : UIPopup
{
    public Button btnStart;
    public Button btnCancel;
    public Button btnOk;

    public Text textContent;
    bool stageEnabled = false;

    private void Start ()
    {
        btnStart.onClick.AddListener (OnClickStartButton);
        btnCancel.onClick.AddListener (OnClickCancelButton);

        btnOk.onClick.AddListener (OnClickOkButton);
    }

    private void OnEnable ()
    {
        if (stageEnabled == true)
        {
            btnStart.gameObject.SetActive (false);
            btnCancel.gameObject.SetActive (false);

            btnOk.gameObject.SetActive (true);
        }
        else
        {
            btnStart.gameObject.SetActive (true);
            btnCancel.gameObject.SetActive (true);

            btnOk.gameObject.SetActive (false);
        }
    }

    private void OnClickStartButton ()
    {
        SceneManager.LoadScene (SceneName.SCENE_GAME);
    }

    private void OnClickCancelButton ()
    {
        Close ();
    }

    private void OnClickOkButton ()
    {
        Close ();
    }

    public void SetStageContentTextDisabled (int stageId)
    {
        stageEnabled = true;
        textContent.text = string.Format ("스테이지 {0}-{1}는 아직 플레이할 수 없어요.", stageId / 100, stageId % 100);
    }

    public void SetStageContentText(int stageId)
    {
        stageEnabled = false;
        textContent.text = string.Format ("스테이지 {0}-{1}을 시작할까요?", stageId / 100, stageId % 100);
    }
}
