using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [SerializeField] Animator mainCamAnim;
    [SerializeField] GameObject[] bosses;
    [SerializeField] MoveCoolTime[] pawns;
    [SerializeField] PlayableDirector stageInitTimeline;
    [SerializeField] PlayableDirector stageClearTimeline;
    [SerializeField] PlayableDirector stageOverTimeline;
    [SerializeField] RuntimeAnimatorController pawnAnimController;
    [SerializeField] EnemySpawnMgr enemySpawnMgr;
    [SerializeField] Button stageInitTimelineSkipBtn;
    [SerializeField] GameObject[] notVisiblesDuringTimelineAnimation;
    [SerializeField] Vector3 vectorToSeeQueen;
    [SerializeField] Vector3 vectorToSeeBoss;
    GameObject activeBoss;

    public int stageIdx;
    public Transform mainCamTr;

    public WillBoostBtn willBoostBtn;
    public ManaBoostBtn manaBoostBtn;

    private void Start()
    {
        Application.targetFrameRate = 60;
        mainCamTr = mainCamAnim.transform;

        // activate timeline skip btn 
        stageInitTimelineSkipBtn.gameObject.SetActive(true);

        // pause initial 4 pawns move cooltime 
        foreach (var p in pawns)
        {
            p.PauseTiking();
        }

        // 이전 씬에서 넘어온 gameobject로부터 stage info 를 받습니다.
        StageInfo stageInfoCarrier;
        if ((stageInfoCarrier = FindObjectOfType<StageInfo>()) != null)
        {
            stageIdx = stageInfoCarrier.stageIdx;
            enemySpawnMgr.LoadStageDataNInit(stageIdx);
            Destroy(stageInfoCarrier.gameObject);
        }
        else
        {
            stageIdx = 1;
            enemySpawnMgr.LoadStageDataNInit(1);
        }

        FindObjectOfType<WillBoostBtn>().InitBtn();
        FindObjectOfType<ManaBoostBtn>().InitBtn();

        // boss setting
        foreach (var b in bosses)
        {
            b.SetActive(false);
        }
        bosses[Mathf.Clamp(stageIdx - 1, 0, bosses.Length - 1)].SetActive(true);
        activeBoss = bosses[Mathf.Clamp(stageIdx - 1, 0, bosses.Length - 1)];

        // play init timeline 
        stageInitTimeline.Play();
    }

    public void StartClearCinematic()
    {
        stageClearTimeline.Play();
    }

    public void StartOverCinematic()
    {
        stageOverTimeline.Play();
    }

    // button onclick event
    public void SkipStageInitialTimeline()
    {
        stageInitTimeline.time = stageInitTimeline.playableAsset.duration;
    }

    public void StartOfStageInitCinematic()
    {
        // stop spawning
        if (enemySpawnMgr)
            enemySpawnMgr.PauseTiking();

        // pause initial 4 pawns' moving cooltime 
        foreach (var p in pawns)
        {
            p.PauseTiking();
        }

        // ui , bosshealthbar, queenhealthbar
        foreach (var o in notVisiblesDuringTimelineAnimation)
        {
            o.SetActive(false);
        }
    }

    public void EndOfStageInitCinematic()
    {
        stageInitTimelineSkipBtn.gameObject.SetActive(false);

        Destroy(mainCamAnim);

        foreach (var p in pawns)
        {
            p.StartTiking();
        }
        stageInitTimeline.enabled = false;

        if (enemySpawnMgr)
            enemySpawnMgr.StartTiking();

        foreach (var o in notVisiblesDuringTimelineAnimation)
        {
            o.SetActive(true);
        }

    }

    public void StartOfStageClearCinematic()
    {
        StartCoroutine(MoveCameraToVector_Coroutine(vectorToSeeBoss));

        if (enemySpawnMgr)
        {
            enemySpawnMgr.PauseTiking();
            float timeTik = enemySpawnMgr.GetTimeTik();

            GooglePlayService.AddScoreToLeaderboard(timeTik, stageIdx);
        }

        BGMMgr.Instance.FadeOutBGM();

        // ui , bosshealthbar, queenhealthbar
        foreach (var o in notVisiblesDuringTimelineAnimation)
        {
            o.SetActive(false);
        }

        foreach (var e in FindObjectsOfType<Enemy_Health>())
        {
            e.gameObject.SetActive(false);
        }

        foreach (var f in FindObjectsOfType<Friendly_Health>())
        {
            f.gameObject.SetActive(false);
        }
    }


    public void EndOfStageClearCinematic()
    {
        int prevStageProgress = PlayerDataMgr.playerData_SO.stageProgress;
        PlayerDataMgr.playerData_SO.stageProgress = Mathf.Max(stageIdx, PlayerDataMgr.playerData_SO.stageProgress);

        if (prevStageProgress < PlayerDataMgr.playerData_SO.stageProgress)
        {
            if (prevStageProgress == 0) PlayerDataMgr.playerData_SO.gold += 1000;
            else if (prevStageProgress == 1) PlayerDataMgr.playerData_SO.gold += 1200;
            else if (prevStageProgress == 2) PlayerDataMgr.playerData_SO.gold += 1400;
            else if (prevStageProgress == 3) PlayerDataMgr.playerData_SO.gold += 1600;
            else if (prevStageProgress == 4) PlayerDataMgr.playerData_SO.gold += 1800;
            else if (prevStageProgress == 5) PlayerDataMgr.playerData_SO.gold += 2000;
        }



        PlayerDataMgr.Sync_Cache_To_Persis(); // 기기 내에 정보 갱신

        // Scene Load Start
        if (stageIdx == 6)
        {
            // 엔딩씬으로
            StartCoroutine(LoadSceneAsync_Coroutine("Ending"));
        }
        else
        {
            // Shop으로
            StartCoroutine(LoadSceneAsync_Coroutine("Shop"));
        }
    }

    public void StartOfStageOverCinematic()
    {
        // 패배시 부스트 사용했으면 도로마무
        if (willBoostBtn.isUsed)
            PlayerDataMgr.playerData_SO.willItemCount = 1;
        if (manaBoostBtn.isUsed)
            PlayerDataMgr.playerData_SO.manaItemCount = 1;

        StartCoroutine(MoveCameraToVector_Coroutine(vectorToSeeQueen));

        if (enemySpawnMgr)
            enemySpawnMgr.PauseTiking();

        BGMMgr.Instance.FadeOutBGM();

        // ui , bosshealthbar, queenhealthbar
        foreach (var o in notVisiblesDuringTimelineAnimation)
        {
            o.SetActive(false);
        }

        foreach (var e in FindObjectsOfType<Enemy_Health>())
        {
            e.gameObject.SetActive(false);
        }

        foreach (var f in FindObjectsOfType<Friendly_Health>())
        {
            f.gameObject.SetActive(false);
        }
    }

    public void EndOfStageOverCinematic()
    {
        // 바로 WorldMap으로
        StartCoroutine(LoadSceneAsync_Coroutine("WorldMap"));
        //BGMMgr.Instance.FadeOutBGM();
        PlayerDataMgr.Sync_Cache_To_Persis(); // 기기 내에 정보 갱신
    }

    IEnumerator LoadSceneAsync_Coroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress >= 0.9f)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

    IEnumerator MoveCameraToVector_Coroutine(Vector3 ToVector)
    {
        while (true)
        {
            mainCamTr.transform.position = Vector3.Lerp(mainCamTr.position, ToVector, 0.03f);

            if (Vector3.Distance(mainCamTr.transform.position, ToVector) <= 0.1f) break;

            yield return null;
        }
    }
}
