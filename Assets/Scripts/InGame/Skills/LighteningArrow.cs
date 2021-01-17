using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighteningArrow : Skill
{
    public override List<Vector2> GetPrefabPosition(Grid clickedGrid)
    {
        // initialize
        List<Vector2> Positions = new List<Vector2>();

        // 클릭한 grid 한곳에만 소환하면 됨.
        Positions.Add(clickedGrid.transform.position);

        return Positions;
    }

    public override List<Grid> GetRange(Grid clickedGrid)
    {
        // initialize
        List<Grid> gridsInRange = new List<Grid>();

        // 해당 클릭된 grid만 담습니다.
        gridsInRange.Add(clickedGrid);

        return gridsInRange;
    }

    public override void InvokeSkillEffect(GameObject target)
    {
        // no effect for LA
        return;
    }
}
