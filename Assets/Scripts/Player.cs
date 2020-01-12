using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private PlayerData playerData;
    private int selectedStageId = 101;

    private void InitPlayerData()
    {
        playerData = new PlayerData ();
        playerData.stageDatas = new List<StageData> ();

        //나중에 파일로 초기화하도록 변경
        for(int stageId = 101; stageId <= 130; stageId++)
        {
            playerData.stageDatas.Add (new StageData (stageId));
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
    }

    public void SavePlayerData()
    {
        DataManager.SaveData<PlayerData> (playerData, "player.dat");
    }

    public int GetPlayerSelectedStageId()
    {
        return selectedStageId;
    }

    public string GetPlayerSelectedStageName ()
    {
        return string.Format("stage{0:D4}", selectedStageId);
    }

    public void SetPlayerSelectedStageId(int stageId)
    {
        this.selectedStageId = stageId;
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