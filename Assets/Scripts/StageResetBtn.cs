using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageResetBtn : MonoBehaviour
{
    public void OnClick_ResetBtn(){
        SceneManager.LoadScene("Stage");
    }
}
