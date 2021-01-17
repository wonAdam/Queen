using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToBtn_Debug : MonoBehaviour
{
    public GameObject BackToEnd;
    void Start()
    {
        if (FindObjectOfType<StageSelectMgr>().stageProgess == 6)
        {
            BackToEnd.SetActive(true);
        }
    }

    public void OnClick_BackToMainBtn()
    {
        SceneLoader.Instance.LoadScene("Main");
    }
}
