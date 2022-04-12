using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class RejuvenatingFlask : Ability
{
    public string label = "Rejuvenating Flask";
    public bool ApplyToSelf = false;
    public bool SeekingTarget;
    public Unit Target;
    public int HealthPenalty = 2;

    public override IEnumerator Act(CellGrid cellGrid)
    {
        GetComponent<Unit>().ActionPoints -= 1;
        Debug.Log("Rejuvenating " + Target.Name);
        Target.HitPoints += HealthPenalty;
        if (Target.HitPoints > Target.TotalHitPoints)
        {
            Target.HitPoints = Target.TotalHitPoints;
        }
        Target.HealthSlider.value = Target.HitPoints;
        GetComponent<Unit>().HitPoints -= HealthPenalty;
        GetComponent<Unit>().HealthSlider.value = GetComponent<Unit>().HitPoints;

        yield return 0;
    }

    public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
    {
        if (SeekingTarget == true)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                Target = unit;
                SeekingTarget = false;
                StartCoroutine(Act(cellGrid));
            }
            else
            {
                Debug.Log("Why the heck would you try to heal an enemy");
            }
        }
        base.OnUnitClicked(unit, cellGrid);
    }

    public override void Activate(CellGrid cellGrid)
    {
        Debug.Log("Rejuvenating Flask Activation Command Received");
        if (GetComponent<Unit>().ActionPoints > 0)
        {
        SeekingTarget = true;
        }
        else
        {
            Debug.Log("No Action Points Left!");
        }


    }

    public void Cancel()
    {
        SeekingTarget = false;
        Debug.Log("No longer seeking target");
    }
}
