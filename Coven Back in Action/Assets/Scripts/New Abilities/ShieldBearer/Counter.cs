using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class Counter : Ability
{
    public int TurnCounter = 0;
    public int Duration = 1;
    public bool CurrentlyActive = false;
    public int HealthPenalty = 4;

    public void Start()
    {
        label = "Counter";
        description = "Sacrifice some health to counterattack adjacent enemies when they hit you for a turn";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Counter");
        GetComponent<Unit>().HitPoints -= HealthPenalty;
        GetComponent<Unit>().HealthSlider.value = GetComponent<Unit>().HitPoints;
        CurrentlyActive = true;
        TurnCounter = 0;

        yield return 0;
    }

    public void AttackResponse(Unit unit)
    {
        if (CurrentlyActive == true && GetComponent<Unit>().Cell.GetDistance(unit.Cell) <= 1)
        {
            int temp = GetComponent<Unit>().AttackFactor;
            GetComponent<Unit>().AttackFactor = HealthPenalty;
            GetComponent<Unit>().AttackHandler(unit);
            GetComponent<Unit>().AttackFactor = temp;
            Debug.Log("Counter Triggered");
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
            }
        }

    }

    public void Deactivate()
    {
        Debug.Log("Counter ending");
        CurrentlyActive = false;
        TurnCounter = 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("You're already set to Counter");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
