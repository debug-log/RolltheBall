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

    private Dictionary<StageEnum, string> keyValuePairs = new Dictionary<StageEnum, string> ();

    public void LoadStageInfoData ()
    {
        if (keyValuePairs.Count != 0)
        {
            return;
        }

        var csvFile = Resources.Load ("Stages/stage_name") as TextAsset;
        if (csvFile == null)
        {
            return;
        }

        var lines = csvFile.text.Split ('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty (line))
            {
                continue;
            }

            var cols = line.Split (',');
            if (cols.Length < 1)
            {
                continue;
            }

            int mainStageId = int.Parse (cols[0]);
            StageEnum key = (StageEnum) (mainStageId - 1);
            string strTitle = cols[1];

            keyValuePairs.Add (key, strTitle);
        }
    }

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
