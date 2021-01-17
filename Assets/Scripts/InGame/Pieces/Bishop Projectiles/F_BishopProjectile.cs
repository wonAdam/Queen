using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class F_BishopProjectile : MonoBehaviour
{
    [Header("Set in Editor")]
    [SerializeField] protected LayerMask enemyLayer; 

    [Header("Set in Runtime")] // Bishop이 Set해줘야함
    public int damage;
    public float range;
    public float speed = 3f;
    public float skillEffectiveness;
    private float projectileInitXPos;
    private void Start() {
        projectileInitXPos = transform.position.x;
    }

    protected void Update() {
        if(projectileInitXPos + range <= transform.position.x) Destroy(gameObject);
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // Enemy_Health
    protected void GiveDamage(Enemy_Health target){
        target.TakeDamage(damage);
    }
}
