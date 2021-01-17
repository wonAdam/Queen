using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyZone : MonoBehaviour
{
    private Queen queen;

    private void Start()
    {
        queen = FindObjectOfType<Queen>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            queen.TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
