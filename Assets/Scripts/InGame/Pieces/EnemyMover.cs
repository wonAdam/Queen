using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] public string code;
    [SerializeField] public LayerMask friendlyLayer;
    public float leftSecForBinding = 0f;
    public float speed = 1f;
    public float currSpeed;
    

    private void Start() {
        int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);

        //Speed
        speed = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx].speed;
        currSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(leftSecForBinding > Mathf.Epsilon)
            CountDownLeftSecForBinding();
        else
            MoveLeft();
    }

    private void MoveLeft() // target 왼쪽으로 이동
    {
        transform.Translate(Vector2.left * currSpeed * Time.deltaTime, Space.World);
    }

    public void Stop(){
        currSpeed = 0f;
    }

    public void UnStop(){
        currSpeed = speed;
    }
   
    public void BindForSeconds(float sec)
    {
        leftSecForBinding = sec;
    }

    public void CountDownLeftSecForBinding(){
        leftSecForBinding = Mathf.Clamp(leftSecForBinding - Time.deltaTime, 0f, Mathf.Infinity);
    }

}
