using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Skill
{
    public override List<Vector2> GetPrefabPosition(Grid clickedGrid)
    {
        // initialize
        List<Vector2> Positions = new List<Vector2>();

        // 범위의 grid마다 prefab을 생성해야함.
        foreach (var grid in GridMgr.grids)
        {
            if (grid.rowIdx == clickedGrid.rowIdx && grid.colIdx != 8)
            {
                Positions.Add(grid.transform.position);
            }
        }

        return Positions;
    }

    public override List<Grid> GetRange(Grid clickedGrid)
    {
        // initialize
        List<Grid> gridsInRange = new List<Grid>();

        foreach(var grid in GridMgr.grids)
        {
            if(grid.rowIdx == clickedGrid.rowIdx && grid.colIdx != 8)
            {
                gridsInRange.Add(grid);
            }
        }

        return gridsInRange;
    }

    public override void InvokeSkillEffect(GameObject target)
    {
        // no effect for FB
        return;
    }
}
