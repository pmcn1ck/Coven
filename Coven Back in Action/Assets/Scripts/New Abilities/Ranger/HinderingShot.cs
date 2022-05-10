using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class HinderingShot : Ability
{
    public float BloodLustCost = 5f;
    public Unit target;
    public bool SeekingTarget = false;
    public bool currentlyActive = true;
    public float enemyMoveStore;
    public int TurnCounter = 0;
    public int Duration = 2;
    public int movePenalty = 2;

    public void Start()
    {
        label = "Hindering Shot";
        description = "Raise your bloodlust to attack an enemy and slow their movement for 2 turns";
    }

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Hindering Shot");
        GetComponent<Unit>().BloodLust += BloodLustCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodLustCost;
        GetComponent<Unit>().AttackHandler(target);
        if (target != null)
        {
            enemyMoveStore = target.MovementPoints;
            target.MovementPoints = enemyMoveStore - movePenalty;
            if (target.MovementPoints < 0)
            {
                target.MovementPoints = 0;
            }
            currentlyActive = true;
            TurnCounter = 0;
        }


        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (GetComponent<Unit>().ActionPoints < 1)
        {
            Debug.Log("Not enough Action Points!");
        }
        else if (currentlyActive == false)
        {
            Debug.Log("Activating Hindering Shot, Select Target plz");
            SeekingTarget = true;
            GetComponent<Unit>().ActionPoints--;
        }
        else
        {
            Debug.Log("Enemy is already slowed");
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

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        TurnCounter++;
        if (target == null)
        {
            currentlyActive = false;
        }
        if (TurnCounter >= Duration)
        {
            target.MovementPoints = enemyMoveStore;
            currentlyActive = false;
        }
        base.OnTurnEnd(cellGrid);
    }

    public void Cancel()
    {
        SeekingTarget = false;
        Debug.Log("No longer seeking target");
    }
}
