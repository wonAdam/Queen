using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrid : Grid
{
    //[HideInInspector] public int rowIdx;
    //[SerializeField] public int colIdx;
    //[SerializeField] public bool isPlacable;
    //public Row row;
    //public Collider2D myCollider;
    //public GameObject piece;
    //public GridMgr gridMgr;
    //public Animator myAnim;
    //public bool isBlinking = false;
    // Start is called before the first frame update

    public BossHealth bossHealth;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myAnim = GetComponent<Animator>();

        gridMgr = FindObjectOfType<GridMgr>();
        GridMgr.grids[0, colIdx] = this;
        GridMgr.grids[1, colIdx] = this;
        GridMgr.grids[2, colIdx] = this;
        GridMgr.grids[3, colIdx] = this;

    }

    // Update is called once per frame
    void Update()
    {
        myAnim.SetBool("isBlinking", isBlinking);


        if(piece != null)
        {
            bossHealth.Hit(1);
            Destroy(piece);
        }
    }
}
