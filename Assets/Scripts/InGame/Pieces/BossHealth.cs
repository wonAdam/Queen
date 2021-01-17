using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    private Animator myAnim;
    public int currHealth;
    BossHealthBar bossHealthBar;
    private void Start()
    {
        myAnim = GetComponent<Animator>();
        currHealth = maxHealth;
        bossHealthBar = FindObjectOfType<BossHealthBar>();
    }
    private void OnEnable()
    {
        FindObjectOfType<BossGrid>().bossHealth = this;
    }
    public void Hit(int damage)
    {
        currHealth -= damage;
        SFXMgr.Instance.SetSFXbyIndex(15);
        SFXMgr.Instance.PlaySFX();

        if (currHealth <= 0)
        {
            myAnim.SetInteger("AnimState", 3);
            myAnim.SetTrigger("AnimTrigger");
            
            Invoke("ClearSoundPlay", 0.8f);


            FindObjectOfType<GameMgr>().StartClearCinematic();
        }
        else
        {
            myAnim.SetTrigger("Hit");
        }

        bossHealthBar.GetComponent<Image>().fillAmount = (float)currHealth / maxHealth;
    }
    public void ClearSoundPlay()
    {
        SFXMgr.Instance.SetSFXbyIndex(16);
        SFXMgr.Instance.PlaySFX();
    }
}
