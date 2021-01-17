using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnBtn : MonoBehaviour
{
    public PieceUpgrade[] pieceUpgrade;
    public SkillUpgrade[] skillUpgrade;
    public ItemUpgrade[] itemUpgrade;

    public void OnClick_ReturnBtn()
    {
        PlayerDataMgr.Sync_Persis_To_Cache();
        SFXMgr.Instance.SetSFXbyIndex(12);
        SFXMgr.Instance.PlaySFX();

        FindObjectOfType<GoldText>().SetGoldText();
        for (int i = 0; i < 4; i++)
        {
            pieceUpgrade[i].ResetUI();
            if (i < 3) skillUpgrade[i].ResetUI();
            if (i < 2) itemUpgrade[i].ResetUI();
        }
    }
}
