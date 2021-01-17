using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneBtn : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        PlayerDataMgr.Sync_Cache_To_Persis(); // 기기 내에 정보 갱신
        SceneLoader.Instance.LoadScene("WorldMap");
        //tartCoroutine(LoadSceneAsync_Coroutine(sceneName));
    }
}
