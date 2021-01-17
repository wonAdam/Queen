using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WillBoostBtn : MonoBehaviour
{
    [SerializeField] ParticleSystem boostVFX;
    [SerializeField] WillBar willBar;
    [SerializeField] float boostRegenPerSec;
    [SerializeField] float boostingTime;
    public bool boosting = false;
    public float currBoostingSec = 0f;
    private bool alreadyClearedStage = false;

    public bool isUsed;
    private void OnEnable()
    {
        FindObjectOfType<GameMgr>().willBoostBtn = this;
    }
    
    // call by GameMgr
    public void InitBtn()
    {
        FindObjectOfType<GameMgr>().willBoostBtn = this;

        isUsed = false;
        boostVFX.Stop();

        // 이미깬 스테이지임.
        if (FindObjectOfType<GameMgr>().stageIdx <= PlayerDataMgr.playerData_SO.stageProgress)
        {
            alreadyClearedStage = true;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            alreadyClearedStage = false;
            if (PlayerDataMgr.playerData_SO.willItemCount > 0)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
    private void Update()
    {
        if(boosting)
        {
            currBoostingSec += Time.deltaTime;
            if(currBoostingSec >= boostingTime)
            {
                currBoostingSec = 0f;
                boosting = false;
                boostVFX.Stop();
                willBar.ResetFillRate();
            }
        }
    }
    public void OnClick_boostBtn()
    {
        isUsed = true;

        if (!alreadyClearedStage)
            PlayerDataMgr.playerData_SO.willItemCount = Mathf.Clamp(PlayerDataMgr.playerData_SO.willItemCount - 1, 0, 1);

        boosting = true;
        boostVFX.Play();
        willBar.SetFillRate(boostRegenPerSec);
        GetComponent<Button>().interactable = false;
    }
}
