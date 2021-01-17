using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageData-to-add
// Stage Data에 추가되는 항목은 여기에도 추가하세요.

// 여러개의 스테이지 데이터를 담습니다.
[System.Serializable]
public class StageDataContainer{
    public List<StageData> stageDatas = new List<StageData>();

    public StageDataContainer(){
        stageDatas.Add(new StageData());
    }
}


// 한개의 스테이지 데이터를 담습니다.
[System.Serializable]
public class StageData
{
    public float normalTime;
    public List<SpawnData> spawnDatasInNormal = new List<SpawnData>();
    public float bigPatternTime;
    public List<SpawnData> bigPattern = new List<SpawnData>();


    public StageData(){
        normalTime = 0f;
        bigPatternTime = 0f;
        spawnDatasInNormal.Add(new SpawnData());
        bigPattern.Add(new SpawnData());
    }
}

[System.Serializable]
public class SpawnData{
    public float spawnTime;
    public string spawnPieceCode;
    public int spawnLineIdx;

    public SpawnData(){
        spawnTime = 0f;
        spawnPieceCode = "";
        spawnLineIdx = 0;
    }
}
