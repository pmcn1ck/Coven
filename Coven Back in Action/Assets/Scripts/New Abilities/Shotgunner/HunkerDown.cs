using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class HunkerDown : Ability
{
    public string label = "Light-Footed";
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int PlayerNumber = 0;
    public float BloodCost = 2f;
    public int MoveLoss = 2;
    public int AtkGain = 2;

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Hunker Down");
        GetComponent<Unit>().BloodLust += BloodCost;
        GetComponent<Unit>().BloodLustSlider.value += BloodCost;
        GetComponent<Unit>().MovementPoints -= MoveLoss;
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
                Debug.Log("Light-Footed ending");
                GetComponent<Unit>().MovementPoints += MoveLoss;
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
            Debug.Log("Hunker Down is already active, wait for it to wear off!");
        }
        else if (GetComponent<LightFooted>() != null && GetComponent<LightFooted>().CurrentlyActive == true)
        {
            Debug.Log("Cannot Activate Hunker Down when Light-Footed is Active");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }
    }
}
