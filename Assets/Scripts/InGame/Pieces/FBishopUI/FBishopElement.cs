using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FBishopElement : MonoBehaviour
{
    [Header("Set in Editor")]
    [SerializeField] Sprite[] images;
    [SerializeField] Text costText;
    [SerializeField] public string code;
    [SerializeField] public GameObject bishop;
    [SerializeField] public GameObject upgradedBishop;

    [Header ("Set in Runtime")]
    private WillBar willBar;
    private PointerMgr pointerMgr;
    private Toggle toggle;
    private int cost;
    private int pieceIdx;
    private bool preState, curState; // true : will 충분, false : will 부족

    private void Start() {
        willBar = FindObjectOfType<WillBar>();
        pointerMgr = FindObjectOfType<PointerMgr>();
        toggle = GetComponent<Toggle>();
        pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);
        cost = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].cost;
        costText.text = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].cost.ToString();
    }

    private void Update() {
        imageChange();  
    }
    private void imageChange()
    {
        preState = curState;
        curState = willBar.IsEnoughWill(cost);
        if(preState != curState) { //will이 30이 넘으면 흑백 -> 컬러
            if(curState) {
                GetComponentInChildren<Image>().sprite = images[1];
            }
            else {
                GetComponentInChildren<Image>().sprite = images[0];
                toggle.isOn = false;
            }
        }
    }
}
