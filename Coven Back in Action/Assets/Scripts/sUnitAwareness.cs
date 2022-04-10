using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class sUnitAwareness : MonoBehaviour
{

    public void CheckForNearbyUnits(Trait _trait)
    {

        CellGrid cellGrid = FindObjectOfType<CellGrid>();
        var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
        var myUnits = cellGrid.GetCurrentPlayerUnits();
        List<Unit> allUnits = new List<Unit>();
        allUnits.AddRange(enemyUnits);
        allUnits.AddRange(myUnits);

        //bool allyInRange = false;
        //bool enemyInRange = false;

        Debug.Log("Focused Unit " + gameObject.name);

        foreach (Unit u in allUnits)
        {
            if (u.GetComponent<Unit>() != GetComponent<Unit>() && GetComponent<Unit>().Cell.GetDistance(u.Cell) <= _trait.Range)
            {
                if (u.GetComponent<ApplyTrait>())
                {
                    foreach (Trait trait in u.GetComponent<ApplyTrait>().trait)
                    {
                        if (trait.Activator == Trait.UnitType.Ally && u.PlayerNumber == 0)
                        {
                            trait.Apply(u);
                        }
                    }
                }
            }
        }
    }
}
