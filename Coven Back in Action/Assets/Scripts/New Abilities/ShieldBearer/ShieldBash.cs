using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class ShieldBash : Ability
{
    public float BloodLustCost = 5f;
    public Unit target;
    public bool SeekingTarget = false;

    public void Reset()
    {
        label = "Shield Bash";
        description = "Raise your bloodlust to attack an enemy for half damage, but stun them on their next turn";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Shield Bash");
        GetComponent<Unit>().ActionPoints--;
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        int temp = GetComponent<Unit>().AttackFactor;
        GetComponent<Unit>().AttackFactor = temp / 2;
        GetComponent<Unit>().AttackHandler(target);
        GetComponent<Unit>().AttackFactor = temp;
        target.Stun = true;

        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (GetComponent<Unit>().ActionPoints >= 1)
        {
            Debug.Log("Activating Shield Bash, Select Your Target");
            SeekingTarget = true;
        }
        else
        {
            Debug.Log("Not enough Action Points");
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
}
