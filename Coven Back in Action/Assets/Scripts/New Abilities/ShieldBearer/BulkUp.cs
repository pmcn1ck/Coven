using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class BulkUp : Ability
{
    public int Range = 2;
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = false;
    public bool CurrentlyActive = false;
    List<Unit> AffectedUnits = new List<Unit>();
    public int PlayerNumber = 0;
    public float BloodLustCost = 10f;

    public void Reset()
    {
        label = "Bulk Up";
    }

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Bulk Up");
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        PlayerNumber = GetComponent<Unit>().PlayerNumber;
        List<Unit> allyUnits = new List<Unit>();
        allyUnits.AddRange(cellGrid.GetCurrentPlayerUnits());
        var AffectedUnits = allyUnits.Where(u => u.Cell.GetDistance(UnitReference.Cell) <= Range);

        foreach (var unit in AffectedUnits)
        {
            if (unit.Equals(UnitReference) && !ApplyToSelf)
            {
                continue;
            }
            unit.DefenceFactor += 2;
        }
        CurrentlyActive = true;
        TurnCounter = 0;

        yield return 0;
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            TurnCounter++;
            if (TurnCounter >= Duration)
            {
                foreach (var unit in AffectedUnits)
                {
                    if (unit.Equals(UnitReference) && !ApplyToSelf)
                    {
                        continue;
                    }
                    unit.DefenceFactor -= 2;
                }
                Debug.Log("Bulk Up ending");
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("Bulk Up is already active, wait for it to wear off!");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
