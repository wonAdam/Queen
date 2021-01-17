using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{

    [Header("Set in Editor")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject readyBtn;
    [SerializeField] GameObject touchToStart;
    [SerializeField] GameObject backPanel;
    [SerializeField] GameObject makersDig;
    [SerializeField] GameObject soundDig;
    [SerializeField] Button loadGameBtn;

    void Start()
    {

        if (PlayerDataMgr.isPlayerDataExist()) //기존 데이터가 존재한다면
        {
            PlayerDataMgr.Sync_Persis_To_Cache(); //경로에서 데이터 조달

            // 스테이지 깬게 없으면 이어하기버튼은 비활
            if (PlayerDataMgr.playerData_SO.stageProgress == 0)
                loadGameBtn.interactable = false;
            else
                loadGameBtn.interactable = true;
        }
        else
        {
            PlayerDataMgr.Init_PlayerData(); // 새로운 플레이어 데이터 초기화
            loadGameBtn.interactable = false;
        }
    }

    public void OnClickMakers()
    {
        makersDig.SetActive(true);
    }

    public void OnClickLoadGame()
    {
        if (PlayerDataMgr.isPlayerDataExist())
        {
            SceneLoader.Instance.LoadScene("WorldMap");
        }
    }

    public void OnClickNewGame()
    {
        PlayerDataMgr.Init_PlayerData(); //initial playerdata를 지정경로에 만들고
        SceneLoader.Instance.LoadScene("Prologue");
    }

    public void OnClickSound()
    {
        soundDig.SetActive(true);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void OnClickTouchToStart()
    {
        readyBtn.SetActive(false);
        touchToStart.SetActive(false);
        backPanel.SetActive(true);
        animator.SetTrigger("start");

    }

    public void OnClickBackPanel()
    {
        makersDig.SetActive(false);
        soundDig.SetActive(false);
    }
}
