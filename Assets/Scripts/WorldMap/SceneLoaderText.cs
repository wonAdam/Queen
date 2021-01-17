using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoaderText : MonoBehaviour
{
    Text text;
    List<string> strings;
    void Awake()
    {
        text = GetComponent<Text>();
        strings = new List<string>();
        strings.Add("적의 초반 기습을 막기 위해선 폰의 강인한 체력이 필수입니다.");
        strings.Add("'부스트'는 비싸지만 훌륭한 아이템입니다.");
        strings.Add("장기간의 게임은 오히려 코로나 대비에 좋습니다...");
        strings.Add("말릴 땐 잠시 일시정지하고 숨을 가다듬는 것도 좋습니다.");
        strings.Add("'룩'은 다부져 보이지만 의외로 허약한 체질입니다.");
        strings.Add("'나이트'는 다른 라인을 지원하기에 안성맞춤입니다.");
        strings.Add("'비숍'은 업그레이드가 가능합니다.");
        strings.Add("'적군 나이트'는 빠르고 강력하지만 머리가 나쁩니다.");
    }
    void OnEnable()
    {
        text.text = strings[new System.Random().Next(0, strings.Count)];
    }
}
