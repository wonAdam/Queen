using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeTxt : MonoBehaviour
{
    private float currtime = 0f;
    public bool isTiktoking = false;
    // Start is called before the first frame update
    void Start()
    {
        currtime = 0f;
        GetComponent<Text>().text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if (isTiktoking)
        {
            currtime += Time.deltaTime;
            SetTimeText(currtime);
        }
    }

    public void SetTimeText(float time){
        int minten = (int)((time / 600f)%10);
        int minone = (int)((time / 60f)%10);
        int sec = (int)(time % 60f);
        string secStr = sec < 10 ? "0" + sec : sec.ToString();
        string minStr = minten > 0 ? "0" + minone : minten.ToString() + minone.ToString();

        GetComponent<Text>().text = minStr+":"+secStr;
    }
}
