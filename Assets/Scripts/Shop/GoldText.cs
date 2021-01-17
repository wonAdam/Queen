using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    [SerializeField] Text goldText;

    private void Start()
    {
        SetGoldText();
    }
    public void SetGoldText()
    {
        goldText.text = PlayerDataMgr.playerData_SO.gold.ToString();
    }

}
