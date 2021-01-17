using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToEndBtn : MonoBehaviour
{
    public void OnClick_BackToEnding()
    {
        SceneLoader.Instance.LoadScene("Ending");
    }
}
