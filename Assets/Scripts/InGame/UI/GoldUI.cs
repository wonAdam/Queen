using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    private Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        goldText = GetComponentInChildren<Text>();
        goldText.text = PlayerDataMgr.playerData_SO.gold.ToString();
    }


    public void AddOrRemoveGold(int amount)
    {
        PlayerDataMgr.playerData_SO.gold = (int)Mathf.Clamp(PlayerDataMgr.playerData_SO.gold + amount, 0f, float.MaxValue);
        goldText.text = PlayerDataMgr.playerData_SO.gold.ToString();
        PlayerDataMgr.Sync_Cache_To_Persis();
    }

}
