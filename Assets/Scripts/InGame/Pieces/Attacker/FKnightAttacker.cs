using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimState))]
public class FKnightAttacker : Attacker
{
    [Header("Set in Editor")]
    [SerializeField] public GameObject swordVFX;


    [Header("Set in Runtime")]
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
    public void SwingSword()
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;
        SFXMgr.Instance.SetSFXbyIndex(2);
        SFXMgr.Instance.PlaySFX();

        // Muzzle Effect를 만들고 
        if (Target)
        {
            GameObject m = Instantiate(swordVFX, Target.transform.position, Quaternion.identity);
            Destroy(m, 1f);
        }
        // target에 데미지를 주고
        GiveDamage();
        // Muzzle Effect는 적당한 짧은 시간 안에 파괴시킵니다.
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


    // Animation Event
    private void GiveDamage()
    {
        Target?.TakeDamage(damage);
    }
}
