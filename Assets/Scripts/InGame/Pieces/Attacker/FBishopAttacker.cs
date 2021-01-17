using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimState))]
public class FBishopAttacker : Attacker
{
    [Header("Set in Editor")]
    [SerializeField] public GameObject projectile;

    [Header("Set in Runtime")]
    //Enemy_Health
    private bool isCoolTime = false;
    private float coolTimeCount = 0f;
    private AnimState animState;


    private void Start()
    {
        SetProperties();
        animState = GetComponent<AnimState>();
    }

    private void Update()
    {
        if(animState == null)
        {
            animState = this.GetComponent<AnimState>();
            return;
        }
        if (animState.ANIMSTATE == AnimState.ANIM_STATE.ATTACK) return;

        if (isCoolTime)
        {
            CoolTimeTikTok();
        }
        else if (isEnemyFront(Vector2.right))
        {
            animState.ChangeAnimState(2);
        }
        else
        {
            animState.ChangeAnimState(0);
        }
    }


    // Animation Event
    public void CastSpell() //Health target
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;
        //audioSource.Play();
        SFXMgr.Instance.SetSFXbyIndex(3);
        SFXMgr.Instance.PlaySFX();

        // Projectile를 만들고 
        GameObject p = Instantiate(projectile, transform.position + Vector3.right * 0.8f, Quaternion.identity);
        p.GetComponent<F_BishopProjectile>().damage = damage;
        p.GetComponent<F_BishopProjectile>().speed = 3f;
        p.GetComponent<F_BishopProjectile>().skillEffectiveness = skillEffectiveness;
        p.GetComponent<F_BishopProjectile>().range = range;
    }

    // 쿨타임을 셉니다.
    private void CoolTimeTikTok()
    {
        if (attackCoolTime > coolTimeCount)
        {
            coolTimeCount += Time.deltaTime;
        }
        else
        {
            isCoolTime = false;
            coolTimeCount = 0f;
        }
    }

}
