using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class StanceChange : Ability
{
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int DefCost = 3;
    public int AtkGain = 3;

    public void Reset()
    {
        label = "Stance Change";
        description = "Sacrifice some health to temporarily lower your defense but boost your attack";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Stance Change");
        GetComponent<Unit>().HitPoints -= Duration;
        GetComponent<Unit>().DefenceFactor -= DefCost;
        GetComponent<Unit>().AttackFactor += DefCost;
        
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
                Debug.Log("Stance Change ending");
                GetComponent<Unit>().DefenceFactor += DefCost;
                GetComponent<Unit>().AttackFactor -= DefCost;
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }
    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("Stance Change is already active, wait for it to wear off!");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
