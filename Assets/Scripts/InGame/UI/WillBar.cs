using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WillBar : MonoBehaviour
{

    [Header ("Set in Editor")]
    [SerializeField] Text willText;
    [SerializeField] float maxWill;
    [SerializeField] public float fillRatePerSec;
    [Range(40f, 200f)] [SerializeField] float operationProcessSpeed;


    [Header ("Set in Runtime")]
    public float currFillRate;
    private Image myImage;
    public float currWill;
    public float goalWill;
    public List<float> willAmountOperationsQueue = new List<float>();
    public bool isOperationProcessing;
    private PointerMgr pointerMgr;
    

    private void Start()
    {
        ResetFillRate();
        myImage = GetComponentInChildren<Image>();
        myImage.fillAmount = 0f;
        currWill = 0f;
        willAmountOperationsQueue.Clear();
        isOperationProcessing = false;
    }

    public void ResetFillRate()
    {
        currFillRate = fillRatePerSec;
    }

    public void SetFillRate(float regenRate)
    {
        currFillRate = regenRate;
    }

    private void OnEnable()
    {
        pointerMgr = FindObjectOfType<PointerMgr>();
        pointerMgr.willBar = this;
    }

    private void Update() {
        Process();
    }

    public void Process(){
        // 현재 will과 최대 will을 text에 표시
        willText.text = (int)currWill + "/" + maxWill;
        // 현재 Operation이 실행 중, 
        if(isOperationProcessing){
            // 우선 채울건 채우고요.
            goalWill = Mathf.Clamp(goalWill + currFillRate * Time.deltaTime, 0f, maxWill); // 자동 의지 충전

            // currWill과 fillAmount를 갱신.
            if(goalWill < currWill){    // 현재 의지가 목표 의지에 도달하지 못했으면
                currWill = Mathf.Clamp(currWill - operationProcessSpeed * Time.deltaTime, goalWill, maxWill);
                myImage.fillAmount = currWill / maxWill; // ProcessSpeed에 맞게 currwill을 깎고, 그것을 표시

                if(Mathf.Abs(currWill - goalWill) <= Mathf.Epsilon){
                    isOperationProcessing = false; 
                } //목표 의지에 도달하면 process 종료
            }
            else{   // 현재 의지가 목표 의지보다 더 진행됐으면??? 이해가 덜 됐음
                currWill = Mathf.Clamp(currWill + operationProcessSpeed * Time.deltaTime, 0f, goalWill);
                myImage.fillAmount = currWill / maxWill;

                if(Mathf.Abs(currWill - goalWill) <= Mathf.Epsilon){
                    isOperationProcessing = false;
                } 
            }

        }
        // 현재 Operation이 안실행중
        else{
            // 실행할 Operation이 남아 있음.
            if(willAmountOperationsQueue.Count > 0){ 
                // Operation을 시작하자.
                float amount = willAmountOperationsQueue[0];
                willAmountOperationsQueue.RemoveAt(0);

                goalWill = Mathf.Clamp(goalWill + amount, 0f, maxWill); 
                isOperationProcessing = true;
            }
            // 실행할 Operation이 남아 있지 않음.
            else{
                // fillRatePerSec만큼 채우자.
                goalWill = Mathf.Clamp(goalWill + currFillRate * Time.deltaTime, 0f, maxWill);
                currWill = goalWill;
                myImage.fillAmount = currWill / maxWill;
            }
        }
        
    }

    public void UseWill(float amount){
        if(!IsEnoughWill((int)amount)) return;
        willAmountOperationsQueue.Add(-amount); 
    }

    public bool IsEnoughWill(int cost){
        // Debug.Log($"cost: {cost}");

        if(cost < 0) return true; //cost가 음수, 즉 의지에 +요인이라면 true

        float realWill = goalWill; 
        foreach(var v in willAmountOperationsQueue){
            realWill += v; 
        }
        // Debug.Log($"realWill: {realWill}");
        // realwill 계산완료
        
        if(realWill-cost > 0){
            return true;
            // 의지 충분, 큐에 넣자
        }
        else{
            return false;
            // 의지 불충분, 명령 삭제
        }
    }

}
