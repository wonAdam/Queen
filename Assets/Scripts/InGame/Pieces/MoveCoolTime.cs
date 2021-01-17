using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCoolTime : MonoBehaviour
{
    [SerializeField] Image coolTimeBar;
    private float maxCoolTime;
    public float currCoolTime = 0f;

    public bool isTiking = true;
    public bool moveReady = false;
    [SerializeField] string code;

    private void Start() {
        // 비숍
        if (!coolTimeBar) return;

        // 처음엔 꼭 꺼놓으세요.
        coolTimeBar.gameObject.SetActive(true);

        int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);

        maxCoolTime = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].moveCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        // 비숍
        if (!coolTimeBar) return;


        if (isTiking)
        {
            coolTimeBar.enabled = true;

            currCoolTime = Mathf.Clamp(currCoolTime + Time.deltaTime, 0f, maxCoolTime);
            coolTimeBar.fillAmount = currCoolTime / maxCoolTime;

            if (currCoolTime == maxCoolTime) moveReady = true;
            else moveReady = false;
        }
        else
        {
            coolTimeBar.enabled = false;
        }
        
    }


    public void StartTiking()
    {
        isTiking = true;
    }
    public void PauseTiking()
    {
        isTiking = false;
    }
}
