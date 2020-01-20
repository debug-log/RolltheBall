using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private PlayerData playerData;
    private int selectedStageId = 101;
    private int nextStageId = 101;

    private void InitPlayerData()
    {
        playerData = new PlayerData ();
        playerData.stageDatas = new List<StageData> ();

        var csvFile = Resources.Load ("Stages/stage_init") as TextAsset;
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
            if (cols.Length < 2)
            {
                continue;
            }

            int stageId = int.Parse (cols[0]);
            bool cleared = bool.Parse (cols[1]);
            int numStars = int.Parse (cols[2]);

            var stageData = new StageData (stageId);
            stageData.cleared = cleared;
            stageData.numStars = numStars;

            playerData.stageDatas.Add (stageData);
        }

        SavePlayerData ();
    }
    
    public void LoadPlayerData()
    {
        playerData = DataManager.LoadData<PlayerData> ("player.dat");
        if(playerData == null)
        {
            InitPlayerData ();
        }

        SetNextStageId ();
    }

    public void SavePlayerData()
    {
        DataManager.SaveData<PlayerData> (playerData, "player.dat");
    }

    public void ResetPlayerData()
    {
        InitPlayerData ();
    }

    private void SetNextStageId()
    {
        int prevStageId = 101;
        foreach (var data in playerData.stageDatas)
        {
            if (data.cleared == false)
            {
                nextStageId = prevStageId;
                break;
            }
            prevStageId = data.stageId;
        }
    }

    public int GetNextStageId()
    {
        return this.nextStageId;
    }

    public int GetNextStageMainId()
    {
        return this.nextStageId / 100;
    }
    
    public int GetPlayerSelectedStageId()
    {
        return selectedStageId;
    }

    public int GetPlayerSelectedMainStageId ()
    {
        return selectedStageId / 100;
    }

    public string GetPlayerSelectedStageName ()
    {
        return string.Format("stage{0:D4}", selectedStageId);
    }

    public void SetPlayerSelectedStageId(int stageId)
    {
        this.selectedStageId = stageId;
    }

    public List<StageData> GetPlayerStageDataList(int mainStage)
    {
        List<StageData> stageData = new List<StageData> ();

        foreach(var data in playerData.stageDatas)
        {
            if(data.stageId / 100 == mainStage)
            {
                stageData.Add (data);
            }
        }

        return stageData;
    }
}

[Serializable]
public class PlayerData
{
    public List<StageData> stageDatas;
}

[Serializable]
public class StageData
{
    public int stageId;
    public int numStars = 0;
    public bool cleared = false;

    public StageData(int stageId)
    {
        this.stageId = stageId;
        this.numStars = 0;
        this.cleared = false;
    }
}