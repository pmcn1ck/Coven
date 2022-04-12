using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class FocusedSights : Ability
{
    public string label = "Focused Sights";
    public int UseTracker = 0;
    public float BloodLustCost = 5f;
    public Unit target;
    public bool SeekingTarget = false;

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Focused Sights");
        GetComponent<Unit>().ActionPoints -= 1;
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        int temp = GetComponent<Unit>().AttackFactor;
        if (UseTracker >= 1)
        {
            GetComponent<Unit>().AttackFactor = (temp - 1) * 3;
            UseTracker = 0;
        }
        else
        {
            GetComponent<Unit>().AttackFactor = temp - 1;
            UseTracker = 2;
        }
        GetComponent<Unit>().AttackHandler(target);
        GetComponent<Unit>().AttackFactor = temp;

        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if(GetComponent<Unit>().ActionPoints > 0)
        {
            Debug.Log("Activating Focused Sights, Select Target plz");
            SeekingTarget = true;
        }
        else
        {
            Debug.Log("Out of Action Points there, buddy");
        }
    }

    public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
    {
        if (unit.PlayerNumber != GetComponent<Unit>().PlayerNumber && GetComponent<Unit>().Cell.GetDistance(unit.Cell) <= GetComponent<Unit>().AttackRange)
        {
            target = unit;
            StartCoroutine(Act(cellGrid));
            SeekingTarget = false;
        }
        else
        {
            Debug.Log("Invalid Target");
        }
        base.OnUnitClicked(unit, cellGrid);
    }

    public void Cancel()
    {
        SeekingTarget = false;
        Debug.Log("No longer seeking target");
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (UseTracker > 0)
        {
            UseTracker--;
        }
        base.OnTurnEnd(cellGrid);
    }
}
