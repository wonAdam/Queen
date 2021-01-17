using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    [Header ("Set in Editor")]
    [SerializeField] Text manaText;
    [SerializeField] float maxMana;
    [SerializeField] float fillRatePerSec;
    [Range(40f, 200f)] [SerializeField] float operationProcessSpeed;


    [Header ("Set in Runtime")]
    public float currFillRate;
    private Image myImage;
    public float currMana;
    public float goalMana;
    public List<float> willAmountOperationsQueue = new List<float>();
    public bool isOperationProcessing;
    private PointerMgr pointerMgr;

    private void Start()
    {
        ResetFillRate();
        myImage = GetComponentInChildren<Image>();
        myImage.fillAmount = 0f;
        currMana = 0f;
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
        pointerMgr.manaBar = this;
    }

    private void Update() {
        Process();
    }

    public void Process(){
        // 현재 mana과 최대 mana을 text에 표시
        manaText.text = (int)currMana + "/" + maxMana;
        // 현재 Operation이 실행중
        if(isOperationProcessing){
            // 우선 채울건 채우고요.
            goalMana = Mathf.Clamp(goalMana + currFillRate * Time.deltaTime, 0f, maxMana);

            // currMana과 fillAmount를 갱신.
            if(goalMana < currMana){
                currMana = Mathf.Clamp(currMana - operationProcessSpeed * Time.deltaTime, goalMana, maxMana);
                myImage.fillAmount = currMana / maxMana;

                if(Mathf.Abs(currMana - goalMana) <= Mathf.Epsilon){
                    isOperationProcessing = false;
                }
            }
            else{
                currMana = Mathf.Clamp(currMana + operationProcessSpeed * Time.deltaTime, 0f, goalMana);
                myImage.fillAmount = currMana / maxMana;

                if(Mathf.Abs(currMana - goalMana) <= Mathf.Epsilon){
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

                goalMana = Mathf.Clamp(goalMana + amount, 0f, maxMana); 
                isOperationProcessing = true;
            }
            // 실행할 Operation이 남아 있지 않음.
            else{
                // fillRatePerSec만큼 채우자.
                goalMana = Mathf.Clamp(goalMana + currFillRate * Time.deltaTime, 0f, maxMana);
                currMana = goalMana;
                myImage.fillAmount = currMana / maxMana;
            }
        }
        
    }

    public void UseMana(float amount){
        if(!IsEnoughMana((int)amount)) return;
        willAmountOperationsQueue.Add(-amount);
    }

    public bool IsEnoughMana(int cost){

        if(cost < 0) return true;

        float realWill = goalMana;
        foreach(var v in willAmountOperationsQueue){
            realWill += v;
        }
        
        if(realWill-cost > 0){
            return true;
        }
        else{
            return false;
        }
    }

}
