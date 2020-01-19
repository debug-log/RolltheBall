﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMain : MonoBehaviour
{
    public Button backBtn;
    public Text textStageTitle;
    public UIStageBox[] stageBoxes;

    private void Start ()
    {
        Player.Instance.ResetPlayerData ();
        Player.Instance.LoadPlayerData ();

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
        Debug.Log (boxId);
    }

    private void OnClickBackButton()
    {
        SceneManager.LoadScene (SceneName.SCENE_TITLE);
    }
}
