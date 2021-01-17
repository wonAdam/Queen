using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoneState : PointerModeState
{
    public NoneState()
    {
    }
    
    public override void Enter()
    {
        GameObject.FindObjectOfType<PointerMgr>().PointerMode = PointerMgr.Mode.None;
    }

    public override bool Process()
    {
        return true;
    }

    public override bool Click()
    {
        // 비숍이 클릭되었는지 먼저 확인하여 비숍모드가 될 여부를 결정합니다.
        // 비숍외의 다른 아군기물이 클릭되었으면 기물이동모드로 바뀝니다.
        RaycastHit2D hit2D = Physics2D.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                Camera.main.ScreenPointToRay(Input.mousePosition).direction,
                Mathf.Infinity,
                LayerMask.GetMask("Friendly"));

        if (hit2D.collider != null)
        {

            if (hit2D.collider.GetComponent<FBishopUI>() != null) // 비숍이면
            {
                // BishopSelected
                Debug.Log("NoneState BishopSelected");
                mode_selectedPiece = hit2D.collider.gameObject;
                nextState = PointerMgr.Mode.BishopSelected;
                return false;
            }
            else // 다른 아군 기물이면
            {
                if (!hit2D.collider.transform.GetComponent<MoveCoolTime>())
                {
                    // None
                    nextState = PointerMgr.Mode.None;
                    return false;
                }
                if (hit2D.collider.transform.GetComponent<MoveCoolTime>().moveReady) // 선택한 기물의 쿨타임이 다 찼다면
                {
                    // FriendlyPieceMove
                    mode_selectedPiece = hit2D.collider.gameObject;
                    nextState = PointerMgr.Mode.FriendlyPieceMove;
                    return false;
                }
                return true;
            }
        }
        return true;
    }

    public override void Exit()
    {
    }

    
}
