using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;
using UnityEngine.UI;
using System.Linq;

public class LightFooted : SpellAbility
{
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = true;
    public bool CurrentlyActive = false;
    public int PlayerNumber = 0;
    public float BloodCost = 2f;
    public int MoveGain = 2;

    public void Start()
    {
        label = "Light-Footed";
        description = "Raise your bloodlust to move further for 3 turns. While active, cannot use Hunker Down";
        playerPicksTarget = false;
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Light-Footed");
        GetComponentInParent<Unit>().BloodLust += BloodCost;
        GetComponentInParent<Unit>().BloodLustSlider.value += BloodCost;
        GetComponentInParent<Unit>().MovementPoints += MoveGain;
        GetComponentInParent<Unit>().MovementAnimationSpeed += MoveGain;

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
                GetComponentInParent<Unit>().MovementPoints -= MoveGain;
                GetComponentInParent<Unit>().MovementAnimationSpeed -= MoveGain;
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("Light-Footed is already active, wait for it to wear off!");
        }
        else if (GetComponent<HunkerDown>() != null && GetComponent<HunkerDown>().CurrentlyActive == true)
        {
            Debug.Log("Cannot Activate Light-Footed when Hunker Down is Active");
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
