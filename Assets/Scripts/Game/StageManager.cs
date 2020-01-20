using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public StageDataType stageDataType;
    public string stageDataPath = string.Empty;

    public BlockManager blockManager;
    public Ball ball;
    public UIPopupClear popupClear;
    public UIPopupFail popupFail;

    private int numStars = 0;

    private void Start ()
    {
        Player.Instance.LoadPlayerData ();
        StageInfo.Instance.LoadStageInfoData ();

        SetupStage ();
    }

    private void SetupStage ()
    {
#if UNITY_EDITOR
        switch(stageDataType)
        {
            case StageDataType.ReadFromScene:
                SetupStageReadFromScene ();
                break;
            case StageDataType.ReadFromCsv:
                SetupStageReadFromCsv ();
                break;
            default:
                SetupStageReadFromScene ();
                break;
        }
#else
        SetupStageReadFromCsv ();
#endif
    }

    private void SetupStageReadFromScene()
    {
        blockManager.InitFromScene ();
    }

    private void SetupStageReadFromCsv()
    {
        if(string.IsNullOrEmpty(stageDataPath) == true)
        {
            stageDataPath = Player.Instance.GetPlayerSelectedStageName ();
        }
        blockManager.Init (stageDataPath);
        UIManager.Instance.SetStageText (stageDataPath);
    }

    public void InvokeStageEndEvent()
    {
        if (this.numStars > 1)
        {
            popupClear.Activate (this.numStars);
            Player.Instance.SetPlayerSelectedStageInfo (true, this.numStars);
            Player.Instance.SavePlayerData ();
        }
        else
        {
            popupFail.Open ();
        }
    }

    public int GetNumStars()
    {
        return this.numStars;
    }

    public void IncreaseNumStars()
    {
        this.numStars++;
    }
}
