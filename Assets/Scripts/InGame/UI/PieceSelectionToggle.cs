using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelectionToggle : MonoBehaviour
{
    [Header("Set in Editor")]
    [SerializeField] Sprite[] images;
    [SerializeField] Text costText;
    [SerializeField] public string code;

    [Header("Set in Runtime")]
    private GameObject piece;
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
        piece = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].prefab;
        cost = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].cost;
        costText.text = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].cost.ToString();
    }

    private void Update() {
        preState = curState;
        curState = willBar.IsEnoughWill(cost);
        if(preState != curState) {
            if(curState) {
                //의지가 충분해지면 1로 돌입
                GetComponentInChildren<Image>().sprite = images[1];
            }
            else {
                //의지가 불충분해지면 0으로 돌입
                GetComponentInChildren<Image>().sprite = images[0];
                toggle.isOn = false;
            }
        }
    }

    public void OnValueChange_PieceSelectionToggle(Toggle change){

        if(change.isOn){
            if(!curState) {
                change.isOn = false;
            }
        }
        
        pointerMgr.OnValueChange_PieceSelectionToggle(change.isOn, piece, code);
    }
}
