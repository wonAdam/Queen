using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyPieceMoveState : PointerModeState
{

    GameObject selectedFriendlyPiece;
    LayerMask gridLayer;
    public FriendlyPieceMoveState(GameObject _selectedFriendlyPiece, LayerMask _gridLayer)
    {
        selectedFriendlyPiece = _selectedFriendlyPiece;
        gridLayer = _gridLayer;
    }

    public override void Enter()
    {
        selectedFriendlyPiece.GetComponent<FriendlyPieceMover>().isMovingModeOfThisPiece_next = true;
        GameObject.FindObjectOfType<PointerMgr>().PointerMode = PointerMgr.Mode.FriendlyPieceMove;
    }

    public override bool Process()
    {
        // 현재 선택된 piece가 죽었는지 확인합니다.
        if(selectedFriendlyPiece == null || selectedFriendlyPiece.GetComponent<Health>().status == Health.STATUS.DEAD)
        {
            return false;
        }

        return true;
    }

    public override bool Click()
    {
        // 클릭 위치에 아군 기물이 맞았고 움직일수 있는 아군기물이라면 해당아군기물의 PieceMoveMode로 바꿉니다.
        RaycastHit2D hit2D = Physics2D.Raycast(
                    Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                    Camera.main.ScreenPointToRay(Input.mousePosition).direction,
                    Mathf.Infinity,
                    LayerMask.GetMask("Friendly"));

        // 아군 기물이 맞음
        if (hit2D.collider != null)
        {
            if (hit2D.collider.transform.GetComponent<MoveCoolTime>().moveReady)
            {
                // 다시 자기 자신을 눌렀으면 None으로
                if(selectedFriendlyPiece == hit2D.collider.gameObject)
                {
                    nextState = PointerMgr.Mode.None;
                    mode_selectedPiece = null;
                    return false;
                }
                // 다른 준비된 기물을 눌렀으면 FriendlyPieceMove로
                else
                {
                    nextState = PointerMgr.Mode.FriendlyPieceMove;
                    mode_selectedPiece = hit2D.collider.gameObject;
                    return false;
                }
            }
        }

        // 클릭 위치에 grid가 맞았고 갈수있는 grid라면 기물을 이동시킵니다.
        hit2D = Physics2D.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition).origin,
            Camera.main.ScreenPointToRay(Input.mousePosition).direction,
            Mathf.Infinity,
            gridLayer);

        if (hit2D.collider != null)
        {
            if (hit2D.collider.GetComponent<Grid>().isBlinking) // 갈수있는 grid임.
            {
                MoveToTheGrid(
                        hit2D.collider.GetComponent<Grid>(),
                        selectedFriendlyPiece.GetComponent<FriendlyPieceMover>().currGrid,
                        selectedFriendlyPiece);



                if (hit2D.collider.GetComponent<BossGrid>())
                {
                    hit2D.collider.GetComponent<BossGrid>().isBlinking = false;
                }
            }
            else // 갈수없는 grid임.
            {
                selectedFriendlyPiece.GetComponent<FriendlyPieceMover>().isMovingModeOfThisPiece_next = false;
            }

            nextState = PointerMgr.Mode.None;
            return false;
        }



        nextState = PointerMgr.Mode.None;
        return true;
    }

    public override void Exit()
    {
        selectedFriendlyPiece.GetComponent<FriendlyPieceMover>().isMovingModeOfThisPiece_next = false;
    }

    public static void MoveToTheGrid(Grid toGrid, Grid fromGrid, GameObject piece)
    {
        // 이동
        piece.transform.position = new Vector2(toGrid.transform.position.x, toGrid.transform.position.y);

        piece.GetComponent<Animator>().SetTrigger("Summon");

        // 마무리 초기화
        fromGrid.piece = null;
        toGrid.piece = piece;
        piece.GetComponent<FriendlyPieceMover>().currGrid = toGrid;
        piece.GetComponent<FriendlyPieceMover>().isMovingModeOfThisPiece_next = false;
        piece.GetComponent<MoveCoolTime>().currCoolTime = 0f;
    }

    
}
