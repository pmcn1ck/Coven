using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;

public class SupportingFire : SpellAbility
{
    public int TurnCounter = 0;
    public int Duration = 1;
    public bool CurrentlyActive = false;
    public int BloodPenalty = 5;

    public void Start()
    {
        label = "Supporting Fire";
        description = "Raise your bloodlust to attack enemies whenever your allies do until next turn! (Once per enemy)";
        playerPicksTarget = false;
        CancelButton = GetComponentInParent<SpellCastingAbility>().CancelButton;
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Supporting Fire");
        GetComponentInParent<ParticlePlayer>().CallExtraParticle(0);
        GetComponentInParent<Unit>().BloodLust += BloodPenalty;
        GetComponentInParent<Unit>().BloodLustSlider.value += BloodPenalty;
        gameObject.GetComponentInParent<Unit>().animScript.toggleIdleTwo();
        CurrentlyActive = true;
        TurnCounter = 0;
        foreach (Unit u in cellGrid.GetEnemyUnits(cellGrid.Players[GetComponentInParent<Unit>().PlayerNumber]))
        {
            if (u.Cell.GetDistance(UnitReference.Cell) <= GetComponentInParent<Unit>().AttackRange)
            {
                Debug.Log(u.name + "Is now a support fire target");
                u.SupportFireTarget = true;
            }
        }

        yield return 0;
    }

    public void AttackResponse(Unit unit)
    {
        if (CurrentlyActive == true)
        {
            int temp = GetComponentInParent<Unit>().AttackFactor;
            GetComponentInParent<Unit>().AttackFactor = GetComponentInParent<Unit>().AttackFactor /2;
            GetComponentInParent<Unit>().AttackHandler(unit);
            GetComponentInParent<Unit>().AttackFactor = temp;
            gameObject.GetComponentInParent<Unit>().animScript.runAttackAnim();
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
                gameObject.GetComponentInParent<Unit>().animScript.toggleIdleTwo();
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

    public override string GetDetails()
    {
        return description;
    }
}
