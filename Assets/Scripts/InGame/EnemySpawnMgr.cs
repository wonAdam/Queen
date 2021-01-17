using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EnemySpawnMgr : MonoBehaviour
{
    public enum WAVESTATE
    {
        NORMAL, BIG
    }

    [Header("Set in Editor")]
    // 위쪽 라인부터 순서대로 넣으세요.
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] int stageIdx;

    [Header("Set in Runtime")]
    public WAVESTATE state = WAVESTATE.NORMAL;
    public StageTimeBar stageTimeBar;
    public StageTimeText stageTimeText;
    public int currSpawnIdx;
    public StageData stageData;
    public float normalWaveTime;
    public float bigWaveTime;
    public float currTime;
    public bool isTiking = false;

    private void Awake()
    {
        isTiking = false;
    }

    void Update()
    {
        if (isTiking)
            ProcessStageData();
    }

    public void LoadStageDataNInit(int idx)
    {
        currTime = 0f;
        currSpawnIdx = 0;

        stageData = StageDataMgr.LoadSingleStageData(idx);

        normalWaveTime = stageData.normalTime;
        bigWaveTime = stageData.bigPatternTime;
        stageData.spawnDatasInNormal.Sort((a, b) => (a.spawnTime.CompareTo(b.spawnTime)));
        stageData.bigPattern.Sort((a, b) => (a.spawnTime.CompareTo(b.spawnTime)));


        stageTimeBar = FindObjectOfType<StageTimeBar>();
        stageTimeText = FindObjectOfType<StageTimeText>();

        stageTimeBar.SetBigWaveFlag(normalWaveTime / (normalWaveTime + bigWaveTime));
        float ratio = currTime / (normalWaveTime + bigWaveTime);
        stageTimeBar.SetTimeBar(ratio);


    }

    public void StartTiking()
    {
        isTiking = true;
        stageTimeText.isTikToking = true;
    }

    public void PauseTiking()
    {
        isTiking = false;
        stageTimeText.isTikToking = false;
    }

    public float GetTimeTik()
    {
        if (stageTimeText)
            return stageTimeText.currTime;
        else 
            return float.MaxValue;
    }

    private void ProcessStageData()
    {

        currTime += Time.deltaTime;

        if (state == WAVESTATE.NORMAL)
        {
            float ratio = currTime / (normalWaveTime + bigWaveTime);
            stageTimeBar.SetTimeBar(ratio);

            if (currTime >= normalWaveTime && stageData.spawnDatasInNormal.Count <= currSpawnIdx)
            {
                state = WAVESTATE.BIG;
                currTime = 0f;
                currSpawnIdx = 0;
                return;
            }
            else if (currSpawnIdx < stageData.spawnDatasInNormal.Count && stageData.spawnDatasInNormal[currSpawnIdx].spawnTime <= currTime)
            {
                string code = stageData.spawnDatasInNormal[currSpawnIdx].spawnPieceCode;
                int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);
                Instantiate(
                    GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].prefab,
                    spawnPositions[stageData.spawnDatasInNormal[currSpawnIdx].spawnLineIdx].position,
                    Quaternion.identity);
                currSpawnIdx++;

            }
        }
        else
        {
            float ratio = (normalWaveTime + currTime) / (normalWaveTime + bigWaveTime);
            stageTimeBar.SetTimeBar(ratio);

            if (currTime >= bigWaveTime && stageData.bigPattern.Count <= currSpawnIdx)
            {
                state = WAVESTATE.NORMAL;
                currTime = 0f;
                currSpawnIdx = 0;
                return;
            }
            else if (currSpawnIdx < stageData.bigPattern.Count && stageData.bigPattern[currSpawnIdx].spawnTime <= currTime)
            {
                string code = stageData.bigPattern[currSpawnIdx].spawnPieceCode;
                int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);

                Instantiate(
                    GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].prefab,
                    spawnPositions[stageData.bigPattern[currSpawnIdx].spawnLineIdx].position,
                    Quaternion.identity);
                currSpawnIdx++;

            }
        }

    }

    // stageData-n.json을 처음 만들때 사용하는 함수입니다.
    // 개발 기간에만 사용할 함수입니다.
    public void MakeStageDataJSON()
    {

        StageData stageData = new StageData();

        string JsonData = JsonUtility.ToJson(stageData, true);
        string path = StageDataMgr.GetPathFromSaveFile();
        using (FileStream stream = File.Open(path, FileMode.Create))
        {

            byte[] byteData = Encoding.UTF8.GetBytes(JsonData);

            stream.Write(byteData, 0, byteData.Length);

            stream.Close();

            Debug.Log("EnemySpawnMgr: Single StageData MAKE COMPLETE \n" + path);
        }
    }
}
