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

    private int numStars = 0;

    private void Start ()
    {
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
        popupClear.Activate (this.numStars);
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
