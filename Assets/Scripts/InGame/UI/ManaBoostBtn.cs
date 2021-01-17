using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBoostBtn : MonoBehaviour
{
    [SerializeField] ParticleSystem boostVFX;
    [SerializeField] ManaBar manaBar;
    [SerializeField] float boostRegenPerSec;
    [SerializeField] float boostingTime;
    public bool boosting = false;
    public float currBoostingSec = 0f;

    public bool isUsed;
    private bool alreadyClearedStage = false;

    private void OnEnable()
    {
        FindObjectOfType<GameMgr>().manaBoostBtn = this;
    }
    private void Start()
    {
        FindObjectOfType<GameMgr>().manaBoostBtn = this;

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
            if (PlayerDataMgr.playerData_SO.manaItemCount > 0)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }

    // call by GameMgr
    public void InitBtn()
    {
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
            if (PlayerDataMgr.playerData_SO.manaItemCount > 0)
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
                manaBar.ResetFillRate();
            }
        }
    }
    public void OnClick_boostBtn()
    {
        

        isUsed = true;

        if(!alreadyClearedStage)
            PlayerDataMgr.playerData_SO.willItemCount -= 1;

        boosting = true;
        boostVFX.Play();
        manaBar.SetFillRate(boostRegenPerSec);
        GetComponent<Button>().interactable = false;
    }
}
