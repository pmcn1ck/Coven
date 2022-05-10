using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class SupportingFire : Ability
{
    public int TurnCounter = 0;
    public int Duration = 1;
    public bool CurrentlyActive = false;
    public int BloodPenalty = 5;

    public void Start()
    {
        label = "Supporting Fire";
        description = "Raise your bloodlust to attack enemies whenever your allies do until next turn! (Once per enemy)";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Supporting Fire");
        GetComponent<Unit>().BloodLust += BloodPenalty;
        GetComponent<Unit>().BloodLustSlider.value += BloodPenalty;
        CurrentlyActive = true;
        TurnCounter = 0;
        foreach (Unit u in cellGrid.GetEnemyUnits(cellGrid.Players[GetComponent<Unit>().PlayerNumber]))
        {
            if (u.Cell.GetDistance(UnitReference.Cell) <= GetComponent<Unit>().AttackRange)
            {
                u.SupportFireTarget = true;
            }
        }

        yield return 0;
    }

    public void AttackResponse(Unit unit)
    {
        if (CurrentlyActive == true)
        {
            int temp = GetComponent<Unit>().AttackFactor;
            GetComponent<Unit>().AttackFactor = GetComponent<Unit>().AttackFactor /2;
            GetComponent<Unit>().AttackHandler(unit);
            GetComponent<Unit>().AttackFactor = temp;
            Debug.Log("Supporting Fire Triggered");
            unit.SupportFireTarget = false;
        }
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            TurnCounter++;
            if (TurnCounter >= Duration)
            {
                Deactivate();
                foreach (Unit u in cellGrid.GetEnemyUnits(cellGrid.Players[GetComponent<Unit>().PlayerNumber]))
                {
                    if (u.SupportFireTarget == true)
                    {
                        u.SupportFireTarget = false;
                    }
                }
            }
        }

    }

    public void Deactivate()
    {
        Debug.Log("Supporting Fire ending");
        CurrentlyActive = false;
        TurnCounter = 0;

    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("No need to do that again");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
