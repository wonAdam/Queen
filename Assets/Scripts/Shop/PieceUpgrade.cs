using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceUpgrade : MonoBehaviour
{
    [SerializeField] int[] upgradeCost;
    [SerializeField] string pieceCode;
    [SerializeField] Text upgradeText;
    [SerializeField] Button upgradeBtn;
    public int currUpgradeDegree;

    // Start is called before the first frame update
    void Start()
    {
        ResetUI();
        upgradeBtn.onClick.AddListener(OnClick_UpgradeBtn);
    }

    public void SetUI()
    {
        upgradeText.text = currUpgradeDegree + "/3";
        if (currUpgradeDegree < upgradeCost.Length)
        {
            upgradeBtn.GetComponentInChildren<Text>().text = upgradeCost[currUpgradeDegree].ToString();
            upgradeBtn.interactable = true;
        }
        else
        {
            upgradeBtn.GetComponentInChildren<Text>().text = "-";
            upgradeBtn.interactable = false;
        }
    }

    public void OnClick_UpgradeBtn()
    {
        if (PlayerDataMgr.playerData_SO.gold - upgradeCost[currUpgradeDegree] < 0) return;

        PlayerDataMgr.playerData_SO.gold -= upgradeCost[currUpgradeDegree];
        FindObjectOfType<GoldText>().SetGoldText();
        int idx = PlayerDataMgr.playerData_SO.GetPieceIdxByCode(pieceCode);
        currUpgradeDegree = ++PlayerDataMgr.playerData_SO.ChessPieces[idx].upgrade;
        SetUI();
        SFXMgr.Instance.SetSFXbyIndex(12);
        SFXMgr.Instance.PlaySFX();
    }

    public void ResetUI()
    {
        int idx = PlayerDataMgr.playerData_SO.GetPieceIdxByCode(pieceCode);
        currUpgradeDegree = PlayerDataMgr.playerData_SO.ChessPieces[idx].upgrade;
        SetUI();
    }
}
