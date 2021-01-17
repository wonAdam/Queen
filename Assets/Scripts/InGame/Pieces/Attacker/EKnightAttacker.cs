using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(AnimState))]
public class EKnightAttacker : Attacker
{

    [Header("Set in Editor")]
    [SerializeField] float stopDistance = 1f;

    [Header("Set in Runtime")]
    private bool isCoolTime = false;
    private float coolTimeCount = 0f;
    private EnemyMover enemyMover;
    private AnimState animState;


    private void Start()
    {
        SetProperties();
        enemyMover = GetComponent<EnemyMover>();
        animState = GetComponent<AnimState>();

    }

    // Update is called once per frame
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
        else if (isEnemyFront(Vector2.left))
        {
            animState.ChangeAnimState(2);
        }
        else
        {
            animState.ChangeAnimState(1);
        }
    }


    // Animation Event
    public void swingSword()
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;
        SFXMgr.Instance.SetSFXbyIndex(6);
        SFXMgr.Instance.PlaySFX();
        GiveDamage();
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

    private void GiveDamage()
    {
        Target?.TakeDamage(damage);
    }
}