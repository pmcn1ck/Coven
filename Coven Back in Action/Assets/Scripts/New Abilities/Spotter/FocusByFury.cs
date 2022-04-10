using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class FocusByFury : Ability
{
    public string label = "Focus By Fury";
    public int TurnCounter = 0;
    public int Duration = 2;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int PlayerNumber = 0;
    public float BloodCost = 2f;
    public int MoveGain = 2;
    public int AtkGain = 3;

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Focus By Fury");
        GetComponent<Unit>().BloodLust += BloodCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodCost;
        GetComponent<Unit>().MovementPoints += MoveGain;
        GetComponent<Unit>().MovementAnimationSpeed += MoveGain;
        GetComponent<Unit>().AttackFactor += AtkGain;

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
                Debug.Log("Focus By Fury ending");
                GetComponent<Unit>().MovementPoints -= MoveGain;
                GetComponent<Unit>().MovementAnimationSpeed -= MoveGain;
                GetComponent<Unit>().AttackFactor -= AtkGain;
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }

    public void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("Focus By Fury is already active, wait for it to wear off!");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }
    }
}
