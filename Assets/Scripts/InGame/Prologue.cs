using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{

    [Header("Set in Editor")]
    [SerializeField] CanvasGroup scene12;
    [SerializeField] CanvasGroup scene3;
    [SerializeField] CanvasGroup scene4;
    [SerializeField] Text scriptText;
    [SerializeField] GameObject flame;

    [Header("Set in Runtime")]
    private Image[] flameImages;
    private List<string> scripts;
    private int processIndex = 0;
    private float fadeTime = 0.8f;
    bool Lock = false;

    void Awake()
    {
        scripts = new List<string>();
        // #1. 양피지 지도 위의 두 나라
        scripts.Add("체스보드 대륙에는 화이트와 블랙이라는 두 국가가 있었습니다.");
        scripts.Add("화이트는 정의와 공정함을, 블랙은 힘과 재능에 대한 열망을 주로 추구했죠.");
        scripts.Add("차이는 갈등을 만들었고, 후에 모자이크 전쟁이라 불리는 전쟁의 불씨가 되었습니다.");
        // #2. 전쟁의 과정
        scripts.Add("전쟁은 초반에 화이트가 우세했지만,");
        scripts.Add("블랙과 연합한 이종족이 가세하면서 전세는 뒤집어졌습니다.");
        // #3. 한편, 화이트의 왕궁에서는...
        scripts.Add("한편, 화이트의 왕궁에서는 7자매 중 여섯 째인 공주가 있었습니다.");
        scripts.Add("왕궁의 관심은 지적인 언니들과 귀여운 막내 동생에게 갔고,");
        scripts.Add("그렇게 모두에게 소외된 그녀에게 블랙과의 전쟁 전황이 들려옵니다.");
        scripts.Add("그녀는 전쟁에 나가기를 원했습니다.");
        scripts.Add("그러나 왕궁에서 그녀의 말을 진심으로 듣는 사람은 아무도 없었죠.");
        // #4. 여행의 시작
        scripts.Add("그녀는 왕국을 지키기 위해, 그리고 모두에게 인정받기 위해");
        scripts.Add("변방이었던, 체크벨리타로 몰래 떠납니다.");
    }

    void Start()
    {
        flameImages = flame.GetComponentsInChildren<Image>();
        for (int i = 0; i < flameImages.Length; i++)
            flameImages[i].gameObject.SetActive(false);
        PrologueProgress(0);
    }

    private void PrologueProgress(int scriptIndex)
    {
        if (scriptIndex == 3)
        {
            StartCoroutine(Flame());
        }
        else if (scriptIndex == 5)
        {
            StartCoroutine(FadeBackground(scene3, scene12));
        }
        else if (scriptIndex == 10)
        {
            StartCoroutine(FadeBackground(scene4, scene3));
        }

        if (scriptIndex < scripts.Count)
        {
            scriptText.text = scripts[scriptIndex];
        }
        else
        {
            if (!Lock)
            {
                Lock = true;
                SceneLoader.Instance.LoadScene("Tutorial");
            }

        }
    }

    IEnumerator Flame()
    {
        for (int i = 0; i < flameImages.Length; i++)
        {
            flameImages[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
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

    public void OnClickNextPanel()
    {
        // if (processIndex >= scripts.Count)
        // {
        //     SceneLoader.Instance.LoadScene("WorldMap");
        // }
        processIndex++;
        PrologueProgress(processIndex);
    }
}
