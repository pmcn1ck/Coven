using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class DeepBreath : Ability
{
    public int TurnCounter = 0;
    public int Duration = 2;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int AtkGain = 3;

    public void Reset()
    {
        label = "Deep Breath";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Taking a Deep Breath");
        GetComponent<Unit>().HitPoints -= Duration;
        GetComponent<Unit>().AttackFactor += AtkGain;
        GetComponent<Unit>().ActionPoints--;

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
                GetComponent<Unit>().AttackFactor -= AtkGain;
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
        else if (GetComponent<Unit>().ActionPoints <= 0)
        {
            Debug.Log("No Action Points Left!");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
