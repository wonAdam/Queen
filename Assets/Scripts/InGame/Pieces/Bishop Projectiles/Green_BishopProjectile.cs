using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green_BishopProjectile : F_BishopProjectile
{
    [SerializeField] int bindNum = 3;
    private void OnTriggerEnter2D(Collider2D other) {
        // ENemy_Health
        if(Mathf.Pow(2, other.gameObject.layer) == (int)enemyLayer && other.GetComponent<Enemy_Health>() != null){
            Debug.Log("if");
            //Enemy_Health
            GiveDamage(other.GetComponent<Enemy_Health>());
            bindNum--;

            other.GetComponent<EnemyMover>().BindForSeconds(skillEffectiveness);
            if(bindNum <= 0){
                Destroy(gameObject);
            }
        }
    }

}
