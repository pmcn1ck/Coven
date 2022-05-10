using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class FocusedSights : Ability
{
    public int UseTracker = 0;
    public float BloodLustCost = 5f;
    public Unit target;
    public bool SeekingTarget = false;

    public void Start()
    {
        label = "Focused Sights";
        description = "Raise your bloodlust to perform a chained attack. The first shot does less damage than normal, but if used again on the next turn the damage is increased significantly";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Focused Sights");
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        GetComponent<Unit>().ActionPoints--;
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
        if (GetComponent<Unit>().ActionPoints > 0)
        {
            Debug.Log("Activating Focused Sights, Select Target plz");
            SeekingTarget = true;
        }
        else
        {
            Debug.Log("Not enough Action Points, bucko");
        }

    }

    public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
    {
        if (SeekingTarget == true)
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
