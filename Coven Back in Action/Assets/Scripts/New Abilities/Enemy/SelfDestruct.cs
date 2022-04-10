using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class SelfDestruct : Ability
{
    public int Range = 2;
    public bool ApplyToSelf = true;

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activation Successful, Explosion Imminent");
        int power = GetComponent<Unit>().TotalHitPoints * 4;
        List<Unit> allUnits = new List<Unit>();
        for (int i = 0; i < cellGrid.Players.Count; i++)
        {
            List<Unit> curPlayerUnits = cellGrid.GetPlayerUnits(cellGrid.Players[i]);
            allUnits.AddRange(curPlayerUnits);
        }
        var unitsInRange = allUnits.Where(u => u.Cell.GetDistance(UnitReference.Cell) <= Range);

        foreach (var unit in unitsInRange)
        {
            if (unit.Equals(UnitReference) && !ApplyToSelf)
            {
                continue;
            }
            unit.DefendHandler(GetComponent<Unit>(), power);
        }


        yield return 0;
    }

    public override void OnUnitDestroyed(CellGrid cellGrid)
    {
        StartCoroutine(Act(cellGrid));
    }
}
