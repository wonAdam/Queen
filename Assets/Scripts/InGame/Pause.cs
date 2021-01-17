using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject Menu;
    public bool IsClickPause;
    void Awake()
    {
        IsClickPause = false;
    }
    public void OnClickRetry()
    {
        Time.timeScale = 1f;

        if (FindObjectOfType<WillBoostBtn>().isUsed)
            PlayerDataMgr.playerData_SO.willItemCount = 1;
        if (FindObjectOfType<ManaBoostBtn>().isUsed)
            PlayerDataMgr.playerData_SO.manaItemCount = 1;

        PlayerDataMgr.Sync_Cache_To_Persis();

        SceneLoader.Instance.LoadScene("WorldMap");
    }
    public void OnClickPause()
    {
        IsClickPause = IsClickPause ? false : true;
        if (IsClickPause) Time.timeScale = 0f;
        else Time.timeScale = 1f;
        Menu.SetActive(IsClickPause);
    }
}
