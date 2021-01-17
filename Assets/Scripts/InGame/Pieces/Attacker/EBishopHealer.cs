using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(AnimState))]
public class EBishopHealer : Attacker
{

    [Header("Set in Editor")]
    [SerializeField] public GameObject projectile;
    [SerializeField] float stopDistance = 1f;
    [SerializeField] LayerMask selfLayer = 9;
    [SerializeField] GameObject healVFX;
    [SerializeField] GameObject healGO;

    [Header("Set in Runtime")]
    private bool isCoolTime = false;
    private float coolTimeCount = 0f;
    private EnemyMover enemyMover;
    // public List<Health> Targets = new List<Health>();
    public HashSet<Health> Targets = new HashSet<Health>();
    private AnimState animState;

    private void Start()
    {
        SetProperties();
        animState = GetComponent<AnimState>();
        enemyMover = GetComponent<EnemyMover>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (animState == null)
        {
            animState = this.GetComponent<AnimState>();
            return;
        }
        if (animState.ANIMSTATE == AnimState.ANIM_STATE.ATTACK) return;

        isBishop = true;

        if (isEnemyFront(Vector2.left))
        { // 사거리안 앞에 적이 있고

            isBishop = false;

            if (isCoolTime)
            { // 쿨타임중이면
                CoolTimeTikTok();
                animState.ChangeAnimState(0);
            }
            else if (FindAllyInFront())
            { // 힐을 받을 아군이있다면
                animState.ChangeAnimState(2);
            }
            else
            {
                animState.ChangeAnimState(0);
            }
        }
        else
        {  // 사거리안 앞에 적이 없고
            if (isCoolTime)
            { // 쿨타임중이면
                CoolTimeTikTok();
                animState.ChangeAnimState(0);
            }
            else if (FindAllyInFront())
            { // 힐을 받을 아군이 있다면
                animState.ChangeAnimState(2);
                healGO.SetActive(true);
            }
            else
            {
                animState.ChangeAnimState(1);
            }
        }
    }
    //private void darkHeal(List<Health> targets)
    private void darkHeal()
    {
        // 쿨타임을 돌게 합니다.
        isCoolTime = true;
        coolTimeCount = 0f;
        SFXMgr.Instance.SetSFXbyIndex(7);
        SFXMgr.Instance.PlaySFX();
        var heal = Instantiate(healGO, transform.position, Quaternion.identity);
        Destroy(heal, 2f);

        // Targets에 담겨있는 Health들을 하나하나 h에 담고 Heal
        foreach (Health h in Targets)
        {
            if (h.status == Health.STATUS.DEAD) continue;

            h?.Heal(damage);
            var vfx = Instantiate(healVFX, h.transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

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

    protected bool FindAllyInFront()
    {
        //Targets를 우선 비우고.
        Targets.Clear();

        RaycastHit2D[] hits_left = Physics2D.RaycastAll(transform.position, Vector2.left, range, selfLayer); // left
        RaycastHit2D[] hits_right = Physics2D.RaycastAll(transform.position, Vector2.right, range, selfLayer); // left

        Debug.DrawLine(transform.position, transform.position + (Vector3)Vector2.left * range, Color.blue, 0.1f);
        Debug.DrawLine(transform.position, transform.position + (Vector3)Vector2.right * range, Color.blue, 0.1f);

        for (int i = 0; i < hits_left.Length; i++)
        {
            if (hits_left[i].collider?.GetComponent<Health>() != null &&
                hits_left[i].collider.GetComponent<Health>().status != Health.STATUS.DEAD)
            {
                if (selfLayer == LayerMask.GetMask("Enemy") && hits_left[i].collider != GetComponent<Collider2D>() && hits_left[i].collider.gameObject.name != "EBishop") // 자신의 콜라이더도 아니여야함.
                {
                    // 조건이 충족되면 Targets에 push
                    Targets.Add(hits_left[i].transform.GetComponent<Enemy_Health>());
                }
            }
        }
        for (int i = 0; i < hits_right.Length; i++)
        {
            if (hits_right[i].collider?.GetComponent<Health>() != null &&
                hits_right[i].collider.GetComponent<Health>().status != Health.STATUS.DEAD)
            {
                if (selfLayer == LayerMask.GetMask("Enemy") && hits_right[i].collider != GetComponent<Collider2D>() && hits_right[i].collider.gameObject.name != "EBishop") // 자신의 콜라이더도 아니여야함.
                {
                    // 조건이 충족되면 Targets에 push
                    Targets.Add(hits_right[i].transform.GetComponent<Enemy_Health>());
                }
            }
        }




        // Targets가 0개가 아니면 true
        // 0개면 false
        return Targets.Count > 0 ? true : false;

    }
}


