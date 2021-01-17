using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSpear : Skill
{
    public override List<Vector2> GetPrefabPosition(Grid clickedGrid)
    {
        // initialize
        List<Vector2> Positions = new List<Vector2>();

        // 범위의 grid의 가운데에만 한개 소환하면 됨.
        Positions.Add(new Vector2(clickedGrid.transform.position.x - 0.21f, 0f));

        return Positions;
    }

    public override List<Grid> GetRange(Grid clickedGrid)
    {
        // initialize
        List<Grid> gridsInRange = new List<Grid>();

        // grid들중에 세로 grid들만 담습니다.
        foreach(var grid in GridMgr.grids)
        {
            if(grid.colIdx == clickedGrid.colIdx && grid.colIdx != 8)
            {
                gridsInRange.Add(grid);
            }
        }

        return gridsInRange;
    }

    public override void InvokeSkillEffect(GameObject target)
    {
        target.GetComponent<EnemyMover>().BindForSeconds(1f);
    }
}
