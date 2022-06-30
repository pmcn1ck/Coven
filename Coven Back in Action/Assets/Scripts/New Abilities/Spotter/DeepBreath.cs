using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;
using UnityEngine.UI;
using System.Linq;

public class DeepBreath : SpellAbility
{
    public int TurnCounter = 0;
    public int Duration = 2;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int AtkGain = 3;

    public void Start()
    {
        label = "Deep Breath";
        description = "Spend an action point to increase your attack power for the next turn";
        playerPicksTarget = false;
        CancelButton = GetComponentInParent<SpellCastingAbility>().CancelButton;
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Taking a Deep Breath");
        GetComponentInParent<Unit>().HitPoints -= Duration;
        GetComponentInParent<Unit>().AttackFactor += AtkGain;
        GetComponentInParent<Unit>().ActionPoints--;

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
                Debug.Log("Letting out your deep breath");
                GetComponentInParent<Unit>().AttackFactor -= AtkGain;
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("No you can't hold your breath any longer, you'll suffocate");
        }
        else if (GetComponentInParent<Unit>().ActionPoints <= 0)
        {
            Debug.Log("No Action Points Left!");
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
