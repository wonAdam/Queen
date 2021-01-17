using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    float volume;
    Slider mySlider;

    void OnEnable()
    {
        mySlider = GetComponent<Slider>();
        volume = mySlider.value;
        mySlider.onValueChanged.AddListener((x)=>{ SFXMgr.Instance.SetVolume(x); volume = mySlider.value;});
    }
}
