using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimState))]
public class RookAttacker : Attacker
{

    [Header("Set in Editor")]
    [SerializeField] public GameObject muzzleVFX;
    [SerializeField] public GameObject gunPos; //샷건 위치
    // [SerializeField] public GameObject bullet;    
    [SerializeField] public GameObject projectile;
    [SerializeField] float firingAngle = 45.0f;


    [Header("Set in Runtime")]
    private bool isCoolTime = false;
    private float coolTimeCount = 0f;
    private AnimState animState;
    private float gravity = 9.8f;


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

    //Enemy_Health
    public void ShootCanon()
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;

        // Muzzle Effect를 만들고 
        if (Target)
        {
            SFXMgr.Instance.SetSFXbyIndex(1);
            SFXMgr.Instance.PlaySFX();
            GameObject m = Instantiate(muzzleVFX, Target.transform.position, Quaternion.identity); // 타격 이펙트
            GameObject p = Instantiate(projectile, gunPos.transform.position, Quaternion.identity); // gunPos 위치에 projectile 생성
            p.GetComponent<Rook_Projectile>().damage = damage;
            //p.GetComponent<Rook_Projectile>().speed = 3f;
            p.GetComponent<Rook_Projectile>().skillEffectiveness = skillEffectiveness;
            p.GetComponent<Rook_Projectile>().range = range;
            float target_Distance = Vector2.Distance(p.transform.position, Target.transform.position); // 거리계산
            p.GetComponent<Rook_Projectile>().speed = target_Distance;

            // if(Target == null){ Destroy(m); yield break; }
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);// 속도
            p.GetComponent<Rook_Projectile>().speed = projectile_Velocity;


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


    // Enemy_Health
    private void GiveDamage()
    {
        Target?.TakeDamage(damage);
    }
}
