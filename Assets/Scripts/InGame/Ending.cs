using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{

    [Header("Set in Editor")]
    [SerializeField] CanvasGroup[] scenes;
    [SerializeField] Text script;


    [Header("Set in Runtime")]
    private float fadeTime = 0.8f;
    private int sceneIdx = 0;
    private List<string> scripts;
    private int scriptIdx = 0;
    private float delayTime = 0.1f;
    private bool isTyping = false;
    private bool isClicked = false;
    private bool nextScene = false;
    private bool EndingLock = false;
    private Coroutine playEnding;

    void Awake()
    {
        scripts = new List<string>();

        // 1
        scripts.Add("그렇게 공주는 피비린내나는 전장을 누비고, 마침내 승리했습니다.");
        scripts.Add("응당 휴식을 취해야 했지만, 공주는 힘들지 않았습니다.");
        scripts.Add("공주는 자신이 궁에 개선하는 모습을 머릿 속에 그렸습니다.");
        scripts.Add("......");
        scripts.Add("'하하, 언니들은 조금 질투할 지도 모르겠네.'");
        scripts.Add("'얼른 서둘러 개선하지.'");
        scripts.Add("'예-'");

        // 2
        scripts.Add("......");
        scripts.Add("...");
        scripts.Add("'...?'");
        scripts.Add("'어찌하여 반기는 이가 하나도 없느냐?'");
        scripts.Add("'공주님의 행차시다!'");
        scripts.Add("...");
        scripts.Add("쾅-!");

        // 3
        scripts.Add("...");
        scripts.Add("'...언니.'");
        scripts.Add("'언니야?'");
        scripts.Add("...");
        scripts.Add("'이, 이게... 이게 무슨 일인 것이냐, 막내야...'");

        // 4
        scripts.Add("'아바마마, 어마마마가... 언니들이... 잡혀가버렸어...'");
        scripts.Add("'잡혀가다니, 누구에게?'");
        scripts.Add("......");
        scripts.Add("...");
        scripts.Add("'#$@!%&*'");

        // 5
        scripts.Add("......");
        scripts.Add("...");
        scripts.Add("'...또 그 위험한 곳을 가려하시는 겁니까.'");
        scripts.Add("...");
        scripts.Add("'꾸물거릴 시간이 없다.'");
        scripts.Add("'당장 채비하도록.'");
        scripts.Add("......");
        scripts.Add("...");
        scripts.Add("'알겠습니다.'");
        scripts.Add("'예-'");
        scripts.Add("'따르겠습니다.'");
        scripts.Add("......");
    }

    void Start()
    {
        playEnding = StartCoroutine("PlayEnding");
    }

    public void OnClickNextPanel()
    {
        if (EndingLock) return;

        if (scriptIdx == 35)
        {
            EndingLock = true;
            StartCoroutine(FadeOutEnding());
        }
        else
        {
            if (isTyping)
            {
                StopCoroutine(playEnding);
                StartCoroutine("SkipScript");
            }
            else
            {
                if (nextScene)
                {
                    StartCoroutine(FadeBackground(scenes[sceneIdx + 1], scenes[sceneIdx]));
                    sceneIdx++;
                    nextScene = false;
                }
                else
                {
                    isClicked = true;
                    playEnding = StartCoroutine("PlayEnding");
                }
            }
        }
    }

    IEnumerator SkipScript()
    {
        script.gameObject.SetActive(true);
        script.text = scripts[scriptIdx];
        isTyping = false;
        delayTime = 0.1f;
        yield return new WaitUntil(() => isClicked);
        isClicked = false;
        if (scriptIdx == 6 || scriptIdx == 13 || scriptIdx == 18 || scriptIdx == 23)
            nextScene = true;
        scriptIdx++;
    }

    IEnumerator PlayEnding()
    {
        script.gameObject.SetActive(true);
        for (int i = 0; i < scripts[scriptIdx].Length + 1; i++)
        {
            isTyping = true;
            script.text = scripts[scriptIdx].Substring(0, i);
            yield return new WaitForSeconds(delayTime);
        }
        isTyping = false;
        delayTime = 0.1f;
        yield return new WaitUntil(() => isClicked);
        isClicked = false;
        if (scriptIdx == 6 || scriptIdx == 13 || scriptIdx == 18 || scriptIdx == 23)
            nextScene = true;
        scriptIdx++;
    }

    IEnumerator FadeBackground(CanvasGroup fadeIn, CanvasGroup fadeOut)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeTime)
        {
            fadeIn.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeTime);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }
        fadeIn.alpha = 1f;
        fadeOut.alpha = 0f;
    }

    IEnumerator FadeOutEnding()
    {
        float timeElapsed = 0f;
        while (timeElapsed < 1.6f)
        {
            scenes[4].alpha = Mathf.Lerp(1f, 0f, timeElapsed / 1.6f);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }
        scenes[4].alpha = 0f;
        script.text = "";
        timeElapsed = 0f;
        while (timeElapsed < 1.6f)
        {
            scenes[5].alpha = Mathf.Lerp(0f, 1f, timeElapsed / 1.6f);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }
        scenes[5].alpha = 1f;
        Invoke("InvokeLoad", 3.5f);
    }

    void InvokeLoad()
    {
        SceneLoader.Instance.LoadScene("Main");
    }
}