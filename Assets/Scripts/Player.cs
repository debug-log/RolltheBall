using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private PlayerData playerData;
    private int selectedStageId = 101;
    private int nextStageId = 101;
    private bool loadedPlayerData = false;

    private void InitPlayerData()
    {
        playerData = new PlayerData ();
        playerData.stageDatas = new Dictionary<int, StageData> ();

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

            playerData.stageDatas.Add (stageId, stageData);
        }

        SavePlayerData ();
    }

    private void UpdatePlayerStageInfos (StageInfoCsv stageInfoCsv)
    {
        int prevStageId = 101;
        foreach (var mainStageId in stageInfoCsv.stageCountInfos.Keys)
        {
            for (int subStageId = 1; subStageId <= stageInfoCsv.stageCountInfos[mainStageId]; subStageId++)
            {
                int stageId = mainStageId * 100 + subStageId;

                if (playerData.stageDatas.ContainsKey (stageId))
                {
                    prevStageId = stageId;
                    continue;
                }

                var stageData = new StageData (stageId);
                if (playerData.stageDatas[prevStageId].numStars >= 1)
                {
                    stageData.cleared = true;
                }
                playerData.stageDatas.Add (stageId, stageData);
                prevStageId = stageId;
            }
        }

        playerData.stageDataVersion = stageInfoCsv.stageInfoVersion;
        SavePlayerData ();
    }
        
    public void LoadPlayerData()
    {
        if (loadedPlayerData == true)
        {
            return;
        }

        playerData = DataManager.LoadData<PlayerData> ("player.dat");
        if(playerData == null)
        {
            InitPlayerData ();
        }

        var stageInfoCsv = ReadStageInfoCsv ();
        if (stageInfoCsv.stageInfoVersion > playerData.stageDataVersion)
        {
            UpdatePlayerStageInfos (stageInfoCsv);
        }

        loadedPlayerData = true;
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

    private StageInfoCsv ReadStageInfoCsv ()
    {
        var csvFile = Resources.Load ("Stages/stage_infos") as TextAsset;
        if (csvFile == null)
        {
            return null;
        }

        StageInfoCsv stageInfoCsv = new StageInfoCsv ();
        stageInfoCsv.stageCountInfos = new Dictionary<int, int> ();

        var lines = csvFile.text.Split ('\n');
        for (int lineCount = 0; lineCount < lines.Length; lineCount++)
        {
            var line = lines[lineCount];
            if (string.IsNullOrEmpty (line))
            {
                continue;
            }

            if (lineCount == 0)
            {
                stageInfoCsv.stageInfoVersion = int.Parse (line);
                continue;
            }

            var cols = line.Split (',');
            if (cols.Length < 2)
            {
                continue;
            }

            int mainStageId = int.Parse (cols[0]);
            int stageCount = int.Parse (cols[1]);

            stageInfoCsv.stageCountInfos.Add (mainStageId, stageCount);
        }
        return stageInfoCsv;
    }

    private void SetNextStageId()
    {
        int prevStageId = 101;
        foreach (var data in playerData.stageDatas)
        {
            if (data.Value.cleared == false)
            {
                break;
            }
            prevStageId = data.Value.stageId;
            nextStageId = prevStageId;
        }
    }

    public int GetPlayerNextStageId()
    {
        return this.nextStageId;
    }

    public int GetPlayerHasNextStage (int stageId)
    {
        var stageData = playerData.stageDatas;
        if (stageData.ContainsKey (stageId + 1) == true)
        {
            return stageId + 1;
        }

        var nextMainStageId = (stageId / 100) + 1;
        if (stageData.ContainsKey (nextMainStageId * 100 + 1))
        {
            return nextMainStageId * 100 + 1;
        }

        return 0;
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

    public void SetPlayerSelectedStageInfo (bool cleared, int numStars)
    {
        var stageData = GetPlayerStageData (this.selectedStageId);
        if (stageData != null)
        {
            //check next stage is opened
            int nextStageId = GetPlayerHasNextStage (stageData.stageId);
            if (nextStageId != 0)
            {
                var nextStageData = GetPlayerStageData (nextStageId);
                if (nextStageData.cleared == false)
                {
                    nextStageData.cleared = true;
                    this.nextStageId = nextStageId;
                }
            }

            stageData.cleared = cleared;
            stageData.numStars = numStars;
        }
    }

    public List<StageData> GetPlayerStageDataList(int mainStage)
    {
        List<StageData> stageData = new List<StageData> ();

        foreach(var data in playerData.stageDatas)
        {
            if(data.Value.stageId / 100 == mainStage)
            {
                stageData.Add (data.Value);
            }
        }

        return stageData;
    }

    public StageData GetPlayerStageData (int stageId)
    {
        if (playerData.stageDatas.ContainsKey (stageId))
        {
            return playerData.stageDatas[stageId];
        }

        return null;
    }

    public bool IsPlayerStageDataCleared (int stageId)
    {
        var stageData =  GetPlayerStageData (stageId);
        if (stageData != null)
        {
            return stageData.cleared;
        }

        return false;
    }

    public bool GetShowOrNotUnityAdsPlay ()
    {
        return UnityEngine.Random.value > 0.5f;
    }
}

public class StageInfoCsv
{
    public int stageInfoVersion;
    public Dictionary<int, int> stageCountInfos;
}

[Serializable]
public class PlayerData
{
    public int stageDataVersion;
    public Dictionary<int, StageData> stageDatas;
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