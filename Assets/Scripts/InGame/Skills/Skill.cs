using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] public string code;
    [SerializeField] public LayerMask damageLayer;

    
    public abstract List<Grid> GetRange(Grid clickedGrid);
    public abstract List<Vector2> GetPrefabPosition(Grid clickedGrid);
    public abstract void InvokeSkillEffect(GameObject target); // ex) FZ 스킬 1초 스턴
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
