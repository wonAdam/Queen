using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimState))]
public class PawnAttacker : Attacker
{

    [Header("Set in Editor")]
    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] public GameObject projectile;
    [SerializeField] Transform bowPos;
    [SerializeField] float arrowVelFactor;


    [Header("Set in Runtime")]
    //Enemy_Health
    private float gravity = 9.8f;
    private bool isCoolTime = false;
    private float coolTimeCount = 0f;
    private AnimState animState;

    private void Start()
    {
        SetProperties();
        animState = this.GetComponent<AnimState>();
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

    // Animation Event <Attack>
    private void ShootArrow()
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;
        SFXMgr.Instance.SetSFXbyIndex(0);
        SFXMgr.Instance.PlaySFX();
        StartCoroutine(SimulateProjectile(Target));
    }

    // 쿨타임을 셉니다.
    private void CoolTimeTikTok()
    {
        if (attackCoolTime > coolTimeCount)
        {
            isCoolTime = true;
            coolTimeCount += Time.deltaTime;
        }
        else
        {
            isCoolTime = false;
            coolTimeCount = 0f;
        }
    }

    // 화살의 궤도를 계산하여 투사합니다.
    // Enemy_Health
    IEnumerator SimulateProjectile(Health target)
    {
        // 투사체를 생성.
        GameObject p = Instantiate(projectile, bowPos.position, Quaternion.identity);

        // Move projectile to the position of throwing object + add some offset if needed.
        p.transform.position = transform.position + new Vector3(0f, 0f, 0f);

        // target이 사라졌을 시 화살 파괴
        if (target == null) { Destroy(p); yield break; }
        // Calculate distance to target, the distance is target_distance
        float target_Distance = Vector2.Distance(p.transform.position, target.transform.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        // velocity is speed
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        // velocity의 x, y component 추출
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);


        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // 재계산
        // todo 
        // 0.01f 를 EnemyMover.cs에서 movespeed가져와서 넣기

        // target이 사라졌을 시
        if (target == null) { Destroy(p); yield break; }

        float target_Distance_Fixed = Vector2.Distance(p.transform.position, target.transform.position + Vector3.left * 0.01f * 80f * flightDuration); // 0.01f는 타겟의 속도, 80f는 factor
        float projectile_Velocity_Fixed = target_Distance_Fixed / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
        float Vx_Fixed = Mathf.Sqrt(projectile_Velocity_Fixed) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy_Fixed = Mathf.Sqrt(projectile_Velocity_Fixed) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration_Fixed = target_Distance_Fixed / Vx_Fixed;


        float elapse_time = 0;

        while (elapse_time < flightDuration_Fixed)
        {
            // Rotate projectile to face the target.
            float zAngle = Mathf.Atan2((Vy_Fixed - (gravity * elapse_time)) * Time.deltaTime * arrowVelFactor, Vx_Fixed * Time.deltaTime * arrowVelFactor);
            p.transform.rotation = Quaternion.AngleAxis(zAngle * 80f, Vector3.forward);

            p.transform.Translate(Vx_Fixed * Time.deltaTime * arrowVelFactor, (Vy_Fixed - (gravity * elapse_time)) * Time.deltaTime * arrowVelFactor, 0, Space.World);

            elapse_time += Time.deltaTime * arrowVelFactor;

            yield return null;
        }

        // target이 사라졌을 시
        if (target == null) { Destroy(p); yield break; }

        GiveDamage();
        Destroy(p);
    }


    //Enemy_Health
    private void GiveDamage()
    {
        Target?.TakeDamage(damage);
    }
}
