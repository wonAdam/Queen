using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopSelectedState : PointerModeState
{
    GameObject selectedBishop;
    public BishopSelectedState(GameObject _selectedBishop)
    {
        selectedBishop = _selectedBishop;
    }
    
    public override void Enter()
    {
        selectedBishop.GetComponent<FBishopUI>().OpenUpgradeUI();
        GameObject.FindObjectOfType<PointerMgr>().PointerMode = PointerMgr.Mode.BishopSelected;
    }
    public override bool Process()
    {
        // 현재 선택된 bishop이 죽었거나 업글돼서 교체됐는지 계속해서 확인합니다.
        if (selectedBishop == null || selectedBishop.GetComponent<Health>().status == Health.STATUS.DEAD)
        {
            nextState = PointerMgr.Mode.None;
            return false;
        }

        return true;
    }
    public override bool Click()
    {
        // 비숍 UI에 맞았는가
        RaycastHit2D hit2D = Physics2D.Raycast(
                    Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                    Camera.main.ScreenPointToRay(Input.mousePosition).direction,
                    Mathf.Infinity,
                    LayerMask.GetMask("UI"));
        if(hit2D.collider != null && hit2D.transform.gameObject.tag == "BishopUI")
        {
            return true;
        }

        else // 아무것도 안맞았거나 다른 UI에 맞음
        {
            FBishopUI openedFBishopUI = selectedBishop.GetComponent<FBishopUI>();

            // 다른 곳을 눌렀으면 닫아야함.
            openedFBishopUI.CloseUpgradeUI();
            nextState = PointerMgr.Mode.None;
            return false;
        }
    }

    public override void Exit()
    {
    }

    
}
