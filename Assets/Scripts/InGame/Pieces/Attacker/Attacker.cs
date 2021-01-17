using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
다른 어택커 스크립트들은 이것을 상속받고
Start함수에서 
    private void Start() {
        SetProperties();
    }
하세요.
상속받은 damage, attackSpeed, range를 사용하세요.
*/


public class Attacker : MonoBehaviour
{
    [Header("Inherited from Attacker.cs")]
    [SerializeField] LayerMask attackLayer;
    [SerializeField] private string code;
    protected ChessPiece_Generic piece_gen = new ChessPiece_Generic();
    public int damage;
    public float attackCoolTime;
    public int range;
    public float skillEffectiveness;
    public Health Target = null;
    public bool isBishop = false;


    // GenericData에서 damage와 range를 받아옵니다.
    protected void SetProperties()
    {
        int pieceIdx = GenericDataMgr.genericData_SO.GetPieceIdxByCode(code);
        if(pieceIdx < 0)
        {
            Debug.Log(gameObject.name + " Attacker SetProperties Method Failed");
            return;
        }
        piece_gen = GenericDataMgr.genericData_SO.ChessPieces[pieceIdx];
        ChessPiece_Player piece_Player = PlayerDataMgr.playerData_SO.ChessPieces[pieceIdx];

        int upgrade;
        if (piece_Player != null)
        {
            upgrade = piece_Player.upgrade;
        }
        else
        {
            upgrade = 0;
        }

        // Damage
        int basic_damage = piece_gen.damage;
        int up_damage = piece_gen.upgrades[upgrade].damage;
        damage = basic_damage + up_damage;

        // Skill Effectiveness
        float basic_effectiveness = piece_gen.skillEffectiveness;
        float up_effectiveness = piece_gen.upgrades[upgrade].skillEffectiveness;
        skillEffectiveness = basic_effectiveness + up_effectiveness;

        // Range
        range = piece_gen.range;

        // AttackCoolTime
        attackCoolTime = piece_gen.attackCoolTime;

    }

    protected bool isEnemyFront(Vector2 dir)
    {
        RaycastHit2D hit2D;

        if (isBishop)
        {
            hit2D = Physics2D.Raycast(transform.position, dir, range / 2, attackLayer);
        }
        else
        {
            hit2D = Physics2D.Raycast(transform.position, dir, range, attackLayer);
        }
        Debug.DrawLine(transform.position, transform.position + (Vector3)dir * range, Color.blue, 0.1f);

        if (hit2D.collider?.GetComponent<Health>() != null &&
            hit2D.collider.GetComponent<Health>().status != Health.STATUS.DEAD)
        {
            if (attackLayer == LayerMask.GetMask("Enemy"))
            {
                Target = hit2D.transform.GetComponent<Enemy_Health>();
                return true;
            }
            else if (attackLayer == LayerMask.GetMask("Friendly"))
            {
                Target = hit2D.transform.GetComponent<Friendly_Health>();
                return true;
            }
            return false;
        }
        else
        {
            Target = null;
            return false;
        }
    }

    // Animation Event
    public void KnockBackDamageWhenFriendlySummon()
    {
        if (!GetComponent<FriendlyPieceMover>() || !GetComponent<FriendlyPieceMover>().currGrid) return;

        // grid에 있는 적군 기물들 뒤로 밀기
        List<Collider2D> enemyColliders;
        GetComponent<FriendlyPieceMover>().currGrid.GetCollidersOnThisGrid(LayerMask.GetMask("Enemy"), out enemyColliders);

        if (enemyColliders.Count == 0) return;

        foreach (var c in enemyColliders)
        {
            c.gameObject.transform.Translate(Vector2.right * 0.5f, Space.Self);
            c.transform.GetComponent<Health>().TakeDamage(damage);
        }
    }

}


