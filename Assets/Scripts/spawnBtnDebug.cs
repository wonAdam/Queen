using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnBtnDebug : MonoBehaviour
{
    [SerializeField] int idx;
    [SerializeField] ToggleGroup enemySpawnToggleGroup;
    [SerializeField] EnemySpawnDebug enemySpawnDebug;

    public void OnClick_spawnBtn(){
        if(enemySpawnToggleGroup.AnyTogglesOn()){
            foreach(Toggle toggle in enemySpawnToggleGroup.ActiveToggles()){
                if(toggle.isOn){
                    string code = toggle.GetComponent<spawnToggleDebug>().code;
                    int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);
                    enemySpawnDebug.SpawnEnemyAt(idx, GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].prefab);
                    return;
                }
            }
        }

    }
}
