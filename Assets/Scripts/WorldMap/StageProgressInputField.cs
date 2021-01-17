using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageProgressInputField : MonoBehaviour
{
    public int stageProgess;
    StageSelectMgr stageSelectMgr;
    private void Start() {
        stageProgess = PlayerDataMgr.playerData_SO.stageProgress;
        GetComponent<InputField>().text = stageProgess.ToString();

        stageSelectMgr = FindObjectOfType<StageSelectMgr>();

        GetComponent<InputField>().onValueChanged.AddListener(OnValueChange_StageProgressInputField);
    }

    public void OnValueChange_StageProgressInputField(string newValue){
        int newStageProgress;

        if(int.TryParse(newValue, out newStageProgress) && newStageProgress >= 0 && newStageProgress < 7){
            stageSelectMgr.SetStageSelectBtn(newStageProgress);
        }
        else{
            GetComponent<InputField>().text = "";
        }
    }
}
