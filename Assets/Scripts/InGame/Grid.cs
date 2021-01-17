using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [HideInInspector] public int rowIdx;
    [SerializeField] public int colIdx;
    [SerializeField] public bool isPlacable;
    public Row row;
    public Collider2D myCollider;
    public GameObject piece;
    public GridMgr gridMgr;
    public Animator myAnim;
    public bool isBlinking = false;
    private void Start()
    {
        row = GetComponentInParent<Row>();
        rowIdx = row.rowIdx;
        myCollider = GetComponent<Collider2D>();
        myAnim = GetComponent<Animator>();

        gridMgr = FindObjectOfType<GridMgr>();
        GridMgr.grids[rowIdx, colIdx] = this;
    }
    private void Update() {
        myAnim.SetBool("isBlinking", isBlinking);
    }

    //public void GiveDamageOnThisGrid(int damage, )
    //{
    //    // Grid의 collider와 겹치는 layerMask에 해당하는 collider들을 받아옵니다.
    //    // setup
    //    ContactFilter2D contactFilter2D = new ContactFilter2D();
    //    contactFilter2D.SetLayerMask(layerMask);
    //    List<Collider2D> resultColliders = new List<Collider2D>();

    //    // get
    //    int n = myCollider.OverlapCollider(contactFilter2D, resultColliders);

    //    // 겹치는 collider가 하나도 없으면 return;
    //    if (n <= 0) return;

    //    // 각 collider에 Health에 접근하여 Damage를 줍니다.
    //    foreach(var collider in resultColliders)
    //    {
    //        Debug.Log("Damage on " + collider.gameObject.name);
    //        Debug.Log(collider.transform.GetComponent<Health>());
    //        Debug.Log(collider.transform.GetComponent<Enemy_Health>());
    //        if (collider.transform.GetComponent<Health>() != null)
    //        {
    //            Debug.Log("Damage : " + damage);
    //            collider.transform.GetComponent<Health>().TakeDamage(damage);
    //        }
    //    }


    //}


    public void GetCollidersOnThisGrid(LayerMask layerMask, out List<Collider2D> resultColliders)
    {
        // Grid의 collider와 겹치는 layerMask에 해당하는 collider들을 받아옵니다.
        // setup
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);
        resultColliders = new List<Collider2D>();

        // get
        int n = myCollider.OverlapCollider(contactFilter2D, resultColliders);

    }
}
