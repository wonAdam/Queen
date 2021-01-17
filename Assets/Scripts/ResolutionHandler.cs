using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionHandler : MonoBehaviour
{
    private float InitScaleX;
    private float InitScaleY;
    [SerializeField] private CanvasScaler UICanvasScaler;
    
    void Start()
    {
        Application.targetFrameRate = 60;

        InitScaleX = UICanvasScaler.referenceResolution.x;
        InitScaleY = UICanvasScaler.referenceResolution.y;

        float currScreenWidth = Screen.width;
        float currScreenHeight = Screen.height;
        float screenRatio = currScreenWidth / currScreenHeight;
        float referenceResolRatio = InitScaleX / InitScaleY;
        float scaleOffset = screenRatio / referenceResolRatio;

        UICanvasScaler.referenceResolution = new Vector2(InitScaleX * scaleOffset, InitScaleY * scaleOffset);
    }

}
