using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Queen : MonoBehaviour
{
    //private Animator animator;
    private Animator animator;
    [SerializeField] int maxHealth;
    public int currHealth;

    QueenHealthBar queenHealthBar;
    private void Start()
    {
        currHealth = maxHealth;
        animator = GetComponent<Animator>();
        queenHealthBar = GetComponentInChildren<QueenHealthBar>();
    }


    public void TakeDamage(int damage)
    {
        currHealth = Mathf.Clamp(currHealth - damage, 0, maxHealth);
        SFXMgr.Instance.SetSFXbyIndex(14);
        SFXMgr.Instance.PlaySFX();
        animator.SetTrigger("isHit");

        if (currHealth <= 0)
        {
            // Dead
            animator.SetBool("isDead", true);
            Invoke("BGMStop", 1f);
            Invoke("FailedSoundPlay", 2f);
            // Gameover
            FindObjectOfType<GameMgr>().StartOverCinematic();
        }

        queenHealthBar.GetComponent<Image>().fillAmount = (float)currHealth / maxHealth;
    }

    public void TriggerSkillEffect()
    {
        animator.SetTrigger("Skill");
    }

    public void FailedSoundPlay()
    {
        SFXMgr.Instance.SetSFXbyIndex(17);
        SFXMgr.Instance.PlaySFX();
    }
}
