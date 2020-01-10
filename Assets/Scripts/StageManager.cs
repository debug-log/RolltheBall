using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public StageDataType stageDataType;
    public string stageDataPath = string.Empty;

    public BlockManager blockManager;
    public Ball ball;

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

    }

    public void InvokeStageEndEvent()
    {
        Debug.Log ("Success");
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
