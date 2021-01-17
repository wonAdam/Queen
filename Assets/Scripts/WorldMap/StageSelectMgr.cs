using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectMgr : MonoBehaviour
{
    public int stageProgess; // if 0 : clear 스테이지 없음 // if 1 : 스테이지 1 clear => 스테이지1,2 open
    [SerializeField] List<Button> stageSelBtns; // 순서대로 넣으세요.
    [SerializeField] Sprite[] stageClear;

    void Start()
    {
        stageProgess = PlayerDataMgr.playerData_SO.stageProgress;
        SetStageSelectBtn(stageProgess);
    }

    public void SetStageSelectBtn(int _stageProgress)
    {
        for (int i = 0; i < stageSelBtns.Count; i++)
        {
            stageSelBtns[i].interactable = (i <= _stageProgress) ? true : false;
            if (i < _stageProgress)
                stageSelBtns[i].gameObject.GetComponent<Image>().sprite = stageClear[i];
        }
    }

    public void OnClick_StageSelBtn(int stageIdx)
    {
        // 다음 스테이지에 넘겨줄 정보를 게임오브젝트로 넘겨줍니다.
        GameObject stageInfoCarrier = new GameObject();
        stageInfoCarrier.AddComponent(typeof(StageInfo));
        stageInfoCarrier.GetComponent<StageInfo>().stageIdx = stageIdx;
        DontDestroyOnLoad(stageInfoCarrier);

        DisableAllSelBtns();
        // 씬 전환
        SceneLoader.Instance.LoadScene("Stage");
    }

    public void DisableAllSelBtns()
    {
        for (int i = 0; i < stageSelBtns.Count; i++)
        {
            stageSelBtns[i].interactable = false;
        }
    }

}
