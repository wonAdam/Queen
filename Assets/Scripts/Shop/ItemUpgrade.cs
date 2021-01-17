using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUpgrade : MonoBehaviour
{
    public enum ITEM
    {
        WILL, MANA
    }
    [SerializeField] int cost;
    [SerializeField] ITEM itemKind;
    [SerializeField] Text buyText;
    [SerializeField] Button buyBtn;
    public int currItemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResetUI();
        buyBtn.onClick.AddListener(OnClick_BuyBtn);
    }

    public void SetUI()
    {
        buyText.text = currItemCount + "/1";
        if (currItemCount == 0)
        {
            buyBtn.GetComponentInChildren<Text>().text = cost.ToString();
            buyBtn.interactable = true;
        }
        else
        {
            buyBtn.GetComponentInChildren<Text>().text = "-";
            buyBtn.interactable = false;
        }
    }

    public void OnClick_BuyBtn()
    {
        if (PlayerDataMgr.playerData_SO.gold - cost < 0) return;

        PlayerDataMgr.playerData_SO.gold -= cost;
        FindObjectOfType<GoldText>().SetGoldText();
        if (itemKind == ITEM.WILL)
        {
            currItemCount = ++PlayerDataMgr.playerData_SO.willItemCount;
        }
        else
        {
            currItemCount = ++PlayerDataMgr.playerData_SO.manaItemCount;
        }
        SetUI();
        SFXMgr.Instance.SetSFXbyIndex(12);
        SFXMgr.Instance.PlaySFX();
    }

    public void ResetUI()
    {
        if (itemKind == ITEM.WILL)
        {
            currItemCount = PlayerDataMgr.playerData_SO.willItemCount;
        }
        else
        {
            currItemCount = PlayerDataMgr.playerData_SO.manaItemCount;
        }
        SetUI();
    }
}
