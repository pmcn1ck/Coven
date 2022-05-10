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
    public bool ApplyToSelf = false;
    public Unit Target;
    public bool seekingTarget = false;
    public int HealthPenalty = 2;

    public void Start()
    {
        label = "Rejuvenating Flask";
        description = "Sacrifice some of your own health to heal an ally";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
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
        if (seekingTarget == true)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                Target = unit;
                seekingTarget = false;
                StartCoroutine(Act(cellGrid));
            }
            else
            {
                Debug.Log("Maybe you should be healing one of your allies instead?");
            }
        }
        base.OnUnitClicked(unit, cellGrid);
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (GetComponent<Unit>().ActionPoints > 0)
        {
            Debug.Log("Seeking target for rejuvenation");
            seekingTarget = true;
        }
        else
        {
            Debug.Log("Not enough Action Points there, buckaroo");
        }

    }

    public void Cancel()
    {
        seekingTarget = false;
        Debug.Log("No longer seeking target");
    }
}
