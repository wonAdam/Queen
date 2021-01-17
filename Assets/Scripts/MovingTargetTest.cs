using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetTest : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] LayerMask friendlyLayer;
    [SerializeField] float stopDistance;
    [SerializeField] float stopSec;
    bool isFriendlyInfront = false;

    // Update is called once per frame
    void Update()
    {
        CountDownStopSec();
        if(stopSec > Mathf.Epsilon)
            return;

        RayCastExample();
        if(!isFriendlyInfront)
            MoveLeft();
    }


    public void RayCastExample(){
        
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.left, stopDistance, friendlyLayer);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * stopDistance, Color.red, 0.1f);

        if(hit2D.collider != null){
            isFriendlyInfront = true;
        }
        else{
            isFriendlyInfront = false;
        }
    }

    private void CountDownStopSec(){
        stopSec = Mathf.Clamp(stopSec - Time.deltaTime, 0f, Mathf.Infinity);
    }
    public void MoveLeft(){
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    public void BindForSeconds(float sec){
        stopSec = sec;
    }
}
