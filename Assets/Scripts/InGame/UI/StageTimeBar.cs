using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeBar : MonoBehaviour
{
    [Header("Set in Editor")]
    [SerializeField] float bigWaveFlag_minXPos;
    [SerializeField] float bigWaveFlag_maxXPos;
    [SerializeField] Image bigWaveFlag;


    [Header ("Set in Runtime")]
    private Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
    }


    public void SetTimeBar(float ratio)
    {
        mySlider.value = ratio;
    }

    public void SetBigWaveFlag(float ratio)
    {
        float newXPos = bigWaveFlag_minXPos + (bigWaveFlag_maxXPos - bigWaveFlag_minXPos) * ratio;
        bigWaveFlag.GetComponent<RectTransform>().localPosition = new Vector3(newXPos, bigWaveFlag.transform.position.y, 0f);
    }


}
