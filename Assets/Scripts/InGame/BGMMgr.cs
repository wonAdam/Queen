using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMMgr : MonoBehaviour
{
    private static BGMMgr instance;
    [SerializeField] AudioClip BGM_main;
    [SerializeField] AudioClip BGM_prologue;
    [SerializeField] AudioClip BGM_fight;
    [SerializeField] AudioClip BGM_shop;
    [SerializeField] AudioClip BGM_ending;
    [SerializeField] AudioClip BGM_boss;
    AudioSource BGM;

    Coroutine fadein;
    Coroutine fadeout;

    public static BGMMgr Instance
    {
        get
        {
            return instance;
        }
        set
        {
            Instance = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        BGM = GetComponent<AudioSource>();
        BGMMgr.Instance.BGM.Play();


        DontDestroyOnLoad(gameObject);

        // 씬이 바뀔 때 호출되는 함수를 정합니다.
        SceneManager.activeSceneChanged += OnChangedActiveScene;
    }


    public void SetVolume(float v)
    {
        BGM.volume = v;
    }

    public void FadeOutBGM()
    {
        fadeout = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float f_time = 0f;
        float currVolume = BGM.volume;
        while (BGM.volume > 0)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(currVolume, 0, f_time);
            yield return null;
        }
        BGM.Pause();
    }

    public void OnChangedActiveScene(Scene current, Scene next)
    {
        int idx;

        switch (next.name)
        {
            case "Main":
                idx = 0;
                break;

            case "Prologue":
                idx = 1;
                break;

            case "Shop":
                idx = 3;
                break;

            case "WorldMap":
                idx = 0;
                break;

            case "Ending":
                idx = 4;
                break;

            case "Stage":
                idx = 
                    FindObjectOfType<GameMgr>().stageIdx == 6 ?
                    5 : 2;
                break;

            case "Tutorial":
                idx = 2;
                break;

            default: // 첫번째 게임시작시 next == ""
                idx = 0;
                break;
        }


        SetBGMbyIndex(idx);


        if(fadeout != null)
            StopCoroutine(fadeout);

        fadein = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float f_time = 0f;
        float currVolume = BGM.volume;
        BGM.volume = 0f;
        BGM.Play();
        while (BGM.volume < 1)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(0, 1, f_time);
            yield return null;
        }
        BGM.volume = 1f;
        BGM.UnPause();
    }

    public void FadeBGMbyIndex(int idx)
    {
        StartCoroutine(FadeVolume(idx));
    }

    IEnumerator FadeVolume(int idx)
    {
        float f_time = 0f;
        float currVolume = BGM.volume;
        while (BGM.volume > 0)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(currVolume, 0, f_time);
            yield return null;
        }
        BGM.Stop();
        SetBGMbyIndex(idx);
        f_time = 0f;
        BGM.Play();
        while (BGM.volume < 1)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(0, currVolume, f_time);
            yield return null;
        }
    }

    public void SetBGMbyIndex(int idx)
    {

        switch (idx)
        {
            case 0:
                BGM.clip = BGM_main;
                break;
            case 1:
                BGM.clip = BGM_prologue;
                break;
            case 2:
                BGM.clip = BGM_fight;
                break;
            case 3:
                BGM.clip = BGM_shop;
                break;
            case 4:
                BGM.clip = BGM_ending;
                break;
            case 5:
                BGM.clip = BGM_boss;
                break;
            default:
                break;
        }
    }

}
