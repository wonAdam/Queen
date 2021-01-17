using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FBishopUI : MonoBehaviour
{
    private WillBar willBar;
    private int cost;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] Toggle[] upgrades;
    [SerializeField] public GameObject bishop;
    [SerializeField] public GameObject[] upgradedBishop;

    private void Start() {
        willBar = FindObjectOfType<WillBar>();
        cost = 30;
    }

    private void Update() {
        //check_Toggle(); //상시 토글 체크
    }

    public void OpenUpgradeUI() 
    {
        // ui 열기
        Debug.Log("UI open");
        upgradeUI.SetActive(true);   
    }

    
    // toggle function
    public void Bishop_Upgrade(int element) // cost가 충분하면 bishop을 삭제하고 upgradedbishop생성, cost지불 
    {
        if(willBar.IsEnoughWill(cost))
        {
            gameObject.SetActive(false);
            Debug.Log("upgraded");
            GameObject p = Instantiate(upgradedBishop[element], bishop.transform.position, Quaternion.identity);
            p.transform.GetComponent<Animator>().SetTrigger("Summon");
            willBar.UseWill(cost);
            bishop.GetComponent<FriendlyPieceMover>().currGrid.piece = p;
            p.GetComponent<FriendlyPieceMover>().currGrid = bishop.GetComponent<FriendlyPieceMover>().currGrid;
            Destroy(bishop);
        }
    }
    public void CloseUpgradeUI()
    {
        // ui 닫기
        Debug.Log("UI Close");
        upgradeUI.SetActive(false);
    }
}