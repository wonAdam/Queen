using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [Header("Set in Editor")]
    public CanvasGroup TextBox;
    [SerializeField] CanvasGroup[] canvasGroups;
    [SerializeField] Text scriptText;
    [SerializeField] GameObject Pawn;
    [SerializeField] Grid FromGrid;
    [SerializeField] Grid ToGrid;
    [SerializeField] Animator Bishop;


    [Header("Set in Runtime")]
    private List<string> scripts;
    private int processIndex = 0;
    private float fadeTime = 0.25f;

    void Awake()
    {
        Bishop.SetBool("IsTalk", false);
        scripts = new List<string>();
        // 1 비숍의 자기소개
        scripts.Add("어서오게. 나는 화이트 왕국의 신관, 비숍이라 하네.");
        scripts.Add("사정이야 이미 알고 있겠지만, 화이트 왕국은 많이 위험한 상황이라네.");
        scripts.Add("블랙 왕국이 '저글링'과 '오크'와 손을 잡고 쳐들어오고 있어.");
        scripts.Add("아무튼 시간이 없으니 얼른 설명하겠네.");
        // 2 아군 기물 소환 토글
        scripts.Add("좌측에 보이는 것들은 아군 기물을 전장에 부르기 위한 버튼이라네."); // 4
        scripts.Add("하얗게 활성화 되면, 의지가 충분해 소환할 수 있다는 것이지."); // 4
        // 3 소환 그리드
        scripts.Add("활성화된 버튼을 누른 상태로 우측의 체스판을 클릭하면 아군 기물이 불러진다네."); // 5
        scripts.Add("좌측 4 X 4, 즉 아군 진영에만 소환이 가능하니 조심하도록. 그 외에는 안돼.");

        // **** 기물 소환 애니메이션 ! 8

        // 4 아군 기물
        scripts.Add("아군 기물의 머리 위엔 파란 막대가 있는데, 그게 가득 차면 앞으로 이동할 수 있다네."); // 7
        // 5 기물 이동
        scripts.Add("기물을 선택하고, 이동할 곳을 선택하면 돼.");
        scripts.Add("기물마다 파란 막대가 차는 시간도 다르고,");
        scripts.Add("한 번에 이동할 수 있는 거리도 다르니 전략을 잘 짜야 할 거야.");
        scripts.Add("기물에 대한 정확한 정보는 스테이지의 우측 상단의 도움말을 보도록 해.");

        // **** 기물 이동 애니메이션 ! 10

        // 6 보스존
        scripts.Add("이렇게 아군 기물을 이동시켜 우측 적장에게 도달시키면 큰 데미지를 줄 수 있지."); // 10
        scripts.Add("적장의 HP가 0이 되면 게임에서 승리한다네.");
        scripts.Add("물론 적군이 계속 몰려오기 때문에 쉽지는 않을 거야.");
        scripts.Add("우선 놈들의 낯짝을 한번 보도록 할까?");

        // **** 보스까지 카메라 왕복 애니메이션 ! 13

        scripts.Add("...정말 교활하기 그지 없는 낯짝이로군.");
        // 7 의지
        scripts.Add("아무튼 다시 한번 말하지만, 아군 기물 소환에는 '의지'가 필요하다네."); // 11
        scripts.Add("좌측 하단의 이 '의지'가 없으면 소환할 수 없으니 주의하게.");
        // 8 의지 부스트
        scripts.Add("그 옆에 있는 건 '부스트'라고 하는데, 상점에서 살 수 있다네. 의지의 상승 폭을 크게 올려주지."); // 13
        scripts.Add("상점은 한 스테이지가 끝날 때마다 들를 수 있다네.");
        // 9 스킬
        scripts.Add("중앙 하단에 보이는 것은 여왕님의 '스킬'이네."); // 15
        // 10 마나
        scripts.Add("'의지'대신 우측 하단의 '마나'를 사용한다는 것만 제외하면, 사용법은 아군 기물 소환법과 같네."); // 16
        // 11 타임바
        scripts.Add("좌측 상단의 '이것'은 소위 시계같은 것인데,"); // 17
        scripts.Add("저 깃발에 '저글링'이 도착하면 적의 지원군들이 쏟아져 나오지.");
        scripts.Add("깃발에 저글링이 도달하기 전에 얼른 전투를 마무리지어야겠지?");
        // 12 queen base zone
        scripts.Add("하지만 무엇보다 중요한 것은 여왕님을 지키는 것이야."); //20
        scripts.Add("'이 구간'을 넘어가면 여왕님을 지킬 수 없으니 조심하도록.");
        // 13 튜토리얼 시작
        scripts.Add("지금부터 자네를 외곽 전장으로 보내겠네."); //22
        scripts.Add("어린 저글링밖에 남지 않은 전장이라 해볼 만할 거야.");
        scripts.Add("아군의 자세한 정보는 게임 우측 상단의 도움말 버튼에 있으니 꼭 눌러보도록.");
        scripts.Add("조금은 도움이 될 거야.");
        scripts.Add("그럼 건투를 비네.");
    }

    void Start()
    {
        StartCoroutine(FadeBackground(TextBox, TextBox));
        TutorialProgress(0);
    }

    private void TutorialProgress(int scriptIndex)
    {
        if (scriptIndex == 4) StartCoroutine(FadeBackground(canvasGroups[0], canvasGroups[0]));
        else if (scriptIndex == 6) StartCoroutine(FadeBackground(canvasGroups[1], canvasGroups[0]));
        else if (scriptIndex == 8) // 기물 소환 애니메이션
        {
            Pawn.SetActive(true);
            FriendlyPieceMoveState.MoveToTheGrid(FromGrid, ToGrid, Pawn);
            StartCoroutine(FadeBackground(canvasGroups[2], canvasGroups[1]));
        }
        else if (scriptIndex == 9) StartCoroutine(FadeBackground(canvasGroups[3], canvasGroups[2]));
        else if (scriptIndex == 12) StartCoroutine(FadeBackground(canvasGroups[11], canvasGroups[3]));
        else if (scriptIndex == 13) // 기물 이동 애니메이션
        {
            FriendlyPieceMoveState.MoveToTheGrid(ToGrid, FromGrid, Pawn);
            StartCoroutine(FadeBackground(canvasGroups[4], canvasGroups[11]));
        }
        else if (scriptIndex == 17) // 카메라 이동 애니메이션
        {
            StartCoroutine(MainCameraMove(-1.4f, 6.5f));
        }
        else if (scriptIndex == 18) // 카메라 이동 애니메이션
        {
            StartCoroutine(MainCameraMove(6.5f, -1.4f));
        }
        else if (scriptIndex == 19) StartCoroutine(FadeBackground(canvasGroups[5], canvasGroups[4]));
        else if (scriptIndex == 20) StartCoroutine(FadeBackground(canvasGroups[6], canvasGroups[5]));
        else if (scriptIndex == 22) StartCoroutine(FadeBackground(canvasGroups[7], canvasGroups[6]));
        else if (scriptIndex == 23) StartCoroutine(FadeBackground(canvasGroups[8], canvasGroups[7]));
        else if (scriptIndex == 24) StartCoroutine(FadeBackground(canvasGroups[9], canvasGroups[8]));
        else if (scriptIndex == 27) StartCoroutine(FadeBackground(canvasGroups[10], canvasGroups[9]));
        else if (scriptIndex == 29) canvasGroups[10].alpha = 0;
        else if (scriptIndex == 31) StartCoroutine(FadeBackground(canvasGroups[11], canvasGroups[11]));

        if (scriptIndex < scripts.Count)
        {
            scriptText.text = scripts[scriptIndex];
        }
        else OnClickSkip();

    }
    IEnumerator FadeBackground(CanvasGroup fadeIn, CanvasGroup fadeOut)
    {
        Bishop.SetBool("IsTalk", true);
        float timeElapsed = 0f;
        while (timeElapsed < fadeTime)
        {
            if (fadeIn.name != fadeOut.name) fadeOut.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeTime);
            fadeIn.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeTime);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }
        fadeOut.alpha = 0f;
        fadeIn.alpha = 1f;
        Bishop.SetBool("IsTalk", false);
    }

    IEnumerator MainCameraMove(float FromCamera, float ToCamera)
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeTime * 3)
        {
            Camera.main.transform.position = new Vector3(Mathf.Lerp(FromCamera, ToCamera, timeElapsed / fadeTime), 0f, -10f);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }
        Camera.main.transform.position = new Vector3(ToCamera, 0f, -10f);
    }

    public void OnClickNextPanel()
    {
        processIndex++;
        TutorialProgress(processIndex);
    }
    public void OnClickSkip()
    {
        SceneLoader.Instance.LoadScene("WorldMap");
    }
}