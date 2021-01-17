using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyPieceMover : MonoBehaviour
{
    private PointerMgr pointerMgr;
    public Grid currGrid;
    private bool isMovingModeOfThisPiece;
    public bool isMovingModeOfThisPiece_next;
    private List<Grid> blinkingGrid;
    private Animator myAnim;
    private void Start()
    {
        pointerMgr = FindObjectOfType<PointerMgr>();
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isMovingModeOfThisPiece_next != isMovingModeOfThisPiece)
        {
            if(isMovingModeOfThisPiece_next && !isMovingModeOfThisPiece) // {???}Mode -> MovingMode
            {
                isMovingModeOfThisPiece = isMovingModeOfThisPiece_next;

                // 이동 가능한 grid들을 구하고
                switch (GetComponent<Health>().code) 
                {
                    case "P0":
                        GridMgr.PawnMoveOnGrid(currGrid, out blinkingGrid);
                        break;
                    case "K0":
                        GridMgr.KnightMoveOnGrid(currGrid, out blinkingGrid);
                        break;
                    case "R0":
                        GridMgr.RookMoveOnGrid(currGrid, out blinkingGrid);
                        break;
                    default:
                        break;
                }

                // 갈수 있는 grid가 없을경우
                if (blinkingGrid.Count == 0)
                {
                    pointerMgr.selectedMovingPiece = null;
                    isMovingModeOfThisPiece_next = false;
                    pointerMgr.PointerMode = PointerMgr.Mode.None;
                }

                // grid들을 blinking
                foreach (var grid in blinkingGrid)
                {
                    grid.isBlinking = true;
                }

            }
            else if(!isMovingModeOfThisPiece_next && isMovingModeOfThisPiece) // MovingMode -> {???}Mode
            {
                isMovingModeOfThisPiece = isMovingModeOfThisPiece_next;

                foreach(var grid in blinkingGrid)
                {
                    grid.isBlinking = false;
                }

                blinkingGrid.Clear();
            }

            
            myAnim.SetBool("MovingBlinking", isMovingModeOfThisPiece);
        }
    }

}
