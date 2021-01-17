using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeText : MonoBehaviour
{
    [Header ("Set in Editor")]
    [Header ("Set in Runtime")]
    public bool isTikToking = false;
    public float currTime = 0f;


    private void Update() {
        if (isTikToking) 
            TikToking();
    }

    private void TikToking()
    {
        currTime += Time.deltaTime;
        SetTimeText(currTime);
    }

    public void SetTimeText(float time)
    {
        int minten = (int)((time / 600f) % 10);
        int minone = (int)((time / 60f) % 10);
        int sec = (int)(time % 60f);
        string secStr = sec < 10 ? "0" + sec : sec.ToString();
        string minStr = minten > 0 ? "0" + minone : minten.ToString() + minone.ToString();

        GetComponent<Text>().text = minStr + ":" + secStr;
    }

    public void StartTikTok()
    {
        isTikToking = true;
    }

    public void StopTikTok()
    {
        isTikToking = false;
    }

}
