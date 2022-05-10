using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class DoubleTap : Ability
{
    public float BloodLustCost = 5f;
    public Unit target;
    public bool SeekingTarget = false;

    public void Start()
    {
        label = "Double Tap";
        description = "Raise your bloodlust to make an extra attack on an enemy in range, but for less damage";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Double Tap");
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        int temp = GetComponent<Unit>().AttackFactor;
        GetComponent<Unit>().AttackFactor = temp / 2;
        GetComponent<Unit>().AttackHandler(target);
        GetComponent<Unit>().AttackFactor = temp;

        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        Debug.Log("Activating Double Tap, Select Target plz");
        SeekingTarget = true;
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
        }

        base.OnUnitClicked(unit, cellGrid);
    }

    public void Cancel()
    {
        SeekingTarget = false;
        Debug.Log("No longer seeking target");
    }
}
