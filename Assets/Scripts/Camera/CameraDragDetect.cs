using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragDetect : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Header ("Set in Editor")]
    [Range(1f,500f)]
    [SerializeField] public float cameraSpeedDivisor;
    [SerializeField] public float maxXPos;
    [SerializeField] public float minXPos;

    [Header ("Set in Runtime")]
    [HideInInspector] public PointerMgr pointerMgr;   
    [HideInInspector] public Camera mainCam;
    [HideInInspector] public float prevXPos;
    
    private void Awake() {
        // 16:9 길쭉한 해상도
        if (Screen.width / Screen.height - 16f / 9f < Mathf.Epsilon)
        {
            Debug.Log("16:9 해상도");

            Debug.Log(Screen.width + "" + Screen.height);
            Camera.main.transform.position = new Vector3(minXPos, 0f, -10f);
            maxXPos *= 1.4f;
            minXPos *= 1.4f;
        }
        // 18:9 길쭉한 해상도
        else if (Screen.width / Screen.height - 18f / 9f < Mathf.Epsilon)
        {
            Debug.Log("18:9 해상도");
            Debug.Log(Screen.width + "" + Screen.height);
            Camera.main.transform.position = new Vector3(minXPos, 0f, -10f);
            
        }
        mainCam = Camera.main;
        pointerMgr = FindObjectOfType<PointerMgr>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        pointerMgr.isDragged = true;
        prevXPos = eventData.position.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float xOffset = (prevXPos - eventData.position.x) / cameraSpeedDivisor;
        Vector3 newPos = new Vector3(Mathf.Clamp(mainCam.transform.position.x + xOffset, minXPos, maxXPos), mainCam.transform.position.y, -10f);
        mainCam.transform.position = newPos;
        prevXPos = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

}
