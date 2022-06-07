using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class TargetMark : Ability
{
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = false;
    public bool CurrentlyActive = false;
    public bool seekingTarget;
    public Unit Target;
    public int DefPenalty = 2;

    public void Start()
    {
        label = "Target Mark";
        description = "Pick any enemy on the field and lower their defense slightly for 3 turns";
        ExperimentalUnit unit = GetComponent<ExperimentalUnit>();

        this.enabled = levelUnlock <= unit.level;
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Targeting " + Target.Name);
        Target.DefenceFactor -= DefPenalty;

        yield return 0;
    }

    public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
    {
        Debug.Log("<color=yellow>OnUnitClicked in TargetMask</color>");
        if (seekingTarget == true)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                Debug.Log("Maybe try targeting an enemy instead, hmm?");
            }
            else
            {
                Target = unit;
                seekingTarget = false;
                StartCoroutine(Act(cellGrid));
            }
        }
        base.OnUnitClicked(unit, cellGrid);
        Debug.Log("<color=yellow>OnUnitClicked in TargetMask</color>");
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            TurnCounter++;
            if (TurnCounter >= Duration )
            {
                Deactivate();
            }
        }

    }

    public override void Activate(CellGrid cellGrid)
    {
        Debug.Log("Target Mark Activation Command Received");
        if (CurrentlyActive == true)
        {
            Debug.Log("A Target is already marked, wait for it to wear off!");
        }
        else
        {
            seekingTarget = true;
        }

    }

    public void Deactivate()
    {
        Debug.Log("Target Mark ending");
        if (Target != null)
        {
            Target.DefenceFactor += DefPenalty;
        }
        CurrentlyActive = false;
        TurnCounter = 0;
    }

    public void Cancel()
    {
        seekingTarget = false;
        Debug.Log("No longer seeking target");
    }
}
