using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : Singleton<StageInfo>
{
    public enum StageEnum
    {
        STAGE_01 = 0,

        Count,
    }

    private static Dictionary<StageEnum, string> keyValuePairs = new Dictionary<StageEnum, string> ()
    {
        { StageEnum.STAGE_01, "고양이가 많은 골목" },
    };

    public string GetMainStageName(int mainStageId)
    {
        StageEnum key = (StageEnum) (mainStageId - 1);

        if (keyValuePairs.ContainsKey (key))
        {
            return keyValuePairs[key];
        }

        return string.Empty;
    }

    public int GetTotalStageCount()
    {
        return (int) StageEnum.Count;
    }
}
