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
    public Unit Target;
    public int HealthPenalty = 2;

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

    public void Activate(CellGrid cellGrid, Unit target)
    {
        Target = target;
        StartCoroutine(Act(cellGrid));
    }
}
