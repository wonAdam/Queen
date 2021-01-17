using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelectedState : PointerModeState
{
    LayerMask gridLayer;
    LayerMask UILayer;
    WillBar willBar;
    List<Toggle> pieceToggles = new List<Toggle>();
    GameObject selectedPiece;
    string selectedPieceCode;


    int pieceIdx;
    int cost;

    public PieceSelectedState(LayerMask _gridLayer, LayerMask _UILayer, WillBar _willBar, List<Toggle> _pieceToggles, GameObject _selectedPiece, string _selectedPieceCode)
    {
        gridLayer = _gridLayer;
        UILayer = _UILayer;
        willBar = _willBar;
        selectedPiece = _selectedPiece;
        selectedPieceCode = _selectedPieceCode;

        pieceToggles.Clear();
        foreach (var t in _pieceToggles)
        {
            pieceToggles.Add(t);
        }


        // 기물인텍스, 코스트 캐시
        pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(selectedPieceCode);
        cost = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].cost;

    }
    public override void Enter()
    {
        GameObject.FindObjectOfType<PointerMgr>().PointerMode = PointerMgr.Mode.PieceSelected;
    }

    public override bool Process()
    {
        // 소환하기 충분한 의지량이 있는지 계속 체크합니다.
        return willBar.IsEnoughWill(cost);
    }

    public override bool Click()
    {
        // 아군 기물을 클릭하면 토글을 전부 해제하고 None으로 갑니다. 
        // 혹은 움직일 준비가된 아군기물이었다면 FriendlyPieceMove로 갑니다.
        RaycastHit2D hit2D = Physics2D.Raycast(
                    Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                    Camera.main.ScreenPointToRay(Input.mousePosition).direction,
                    Mathf.Infinity,
                    LayerMask.GetMask("Friendly"));

        // 아군 기물이 맞음
        if (hit2D.collider != null)
        {
            // FriendlyPieceMove
            if (hit2D.collider.transform.GetComponent<MoveCoolTime>().moveReady)
            {
                nextState = PointerMgr.Mode.FriendlyPieceMove;
                mode_selectedPiece = hit2D.collider.gameObject;
                return false;
            }
            // None
            else
            {
                nextState = PointerMgr.Mode.None;
                return false;
            }
        }


        // 클릭 위치에 grid가 맞으면 선택되어있는 기물을 소환합니다.
        hit2D = Physics2D.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition).origin,
            Camera.main.ScreenPointToRay(Input.mousePosition).direction,
            Mathf.Infinity,
            gridLayer | UILayer);

        // grid 혹은 UI Collider(아군기물 선택ui) 맞음
        if (hit2D.collider != null)
        {
            // 맞은 collider가 IULayer임
            if ((1 << hit2D.collider.gameObject.layer & UILayer) > 0) return true;
            // 맞은 collider가 gridLayer임
            else
            {
                // 소환가능한 grid 이고 Will이 충분하면 소환합니다.
                if (hit2D.collider.GetComponent<Grid>().isPlacable && hit2D.collider.GetComponent<Grid>().piece == null && willBar.IsEnoughWill(cost))
                {
                    SummonPieceAt(hit2D.collider.GetComponent<Grid>(), selectedPiece);
                    willBar.UseWill(cost);
                    nextState = PointerMgr.Mode.None;
                    return false;
                }

                // state stay
                return true;
            }
        }

        nextState = PointerMgr.Mode.None;
        return false;
    }

    public override void Exit()
    {
        foreach (var t in pieceToggles)
        {
            t.isOn = false;
        }
    }


    // StartPhase Timeline에 의해 호출됩니다. 
    private void SummonPieceAt(Grid grid, GameObject piece)
    {
        GameObject p = GameObject.Instantiate(piece, new Vector2(grid.transform.position.x, grid.transform.position.y), Quaternion.identity);
        Transform body = p.transform.Find("Body");
        body.GetComponent<SpriteRenderer>().sortingOrder = grid.transform.parent.GetComponent<Row>().rowIdx; // row 별로 기물들의 sortingOrder를 조정
        grid.GetComponent<Grid>().piece = p;
        p.GetComponent<FriendlyPieceMover>().currGrid = grid;
        p.GetComponent<Animator>().SetTrigger("Summon");
    }

    
}
