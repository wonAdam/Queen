using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDebug : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPos;
    public void SpawnEnemyAt(int lineIdx, GameObject enemyPrefab){
        Instantiate(enemyPrefab, spawnPos[lineIdx].position, Quaternion.identity);
    }


}
