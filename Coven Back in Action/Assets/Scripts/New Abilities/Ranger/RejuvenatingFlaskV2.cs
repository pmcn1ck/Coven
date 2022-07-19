using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using UnityEngine;
using TbsFramework.HOMMExample;


public class RejuvenatingFlaskV2 : SpellAbility
{
    public int nJumps;
    public int Damage;
    private List<Unit> inRange;
    public int HealthPenalty = 2;
    public int Healing = 5;
    public Unit target;

    public void Start()
    {
        label = "Rejuvenating Flask";
        description = "Sacrifice some of your own health to heal an ally";
        CancelButton = GetComponentInParent<SpellCastingAbility>().CancelButton;
    }

    public Unit SelectedTarget { get; set; }

    public override IEnumerator Act(CellGrid cellGrid)
    {
        
        if (CanPerform(cellGrid))
        {
            gameObject.GetComponentInParent<Unit>().animScript.runCastAnim();
            
            if (inRange == null)
            {
                inRange = new List<Unit>() { SelectedTarget };
                var currentUnit = SelectedTarget;
                var thisUnit = GetComponentInParent<Unit>();
                thisUnit.gameObject.transform.LookAt(currentUnit.transform.position);
                /*for (var i = 0; i < nJumps; i++)
                {
                    currentUnit = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer)
                                          .Where(u => !inRange.Contains(u) && u.Cell != null)
                                          .OrderBy(u => u.Cell.GetDistance(currentUnit.Cell))
                                          .FirstOrDefault();
                    if (currentUnit == null)
                    {
                        break;
                    }
                    inRange.Add(currentUnit);
                }*/
            }


            if (GetComponentInParent<Unit>().ActionPoints >= 1)
            {
                target = SelectedTarget;
                target.HitPoints += Healing;
                if (target.HitPoints > target.TotalHitPoints)
                {
                    target.HitPoints = target.TotalHitPoints;
                }
                target.HealthSlider.value = target.HitPoints;
                GetComponentInParent<Unit>().ActionPoints--;
                GetComponentInParent<Unit>().HitPoints -= HealthPenalty;
                GetComponentInParent<Unit>().HealthSlider.value = GetComponentInParent<Unit>().HitPoints;
                GetComponentInParent<ParticlePlayer>().CallHeal();

            }


            yield return base.Act(cellGrid);
        }
    }

    public override void OnUnitHighlighted(Unit unit, CellGrid cellGrid)
    {
        if (unit.PlayerNumber == UnitReference.PlayerNumber)
        {
            return;
        }

        inRange = new List<Unit>() { unit };
        var currentUnit = unit;
        /*for (var i = 0; i < nJumps; i++)
        {
            currentUnit = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer)
                                  .Where(u => !inRange.Contains(u) && u.Cell != null)
                                  .OrderBy(u => u.Cell.GetDistance(currentUnit.Cell))
                                  .FirstOrDefault();
            if (currentUnit == null)
            {
                break;
            }
            inRange.Add(currentUnit);
        }*/

        foreach (var unitInRange in inRange)
        {
            unitInRange.MarkAsReachableEnemy();
        }
    }

    public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
    {
        if (unit.PlayerNumber == UnitReference.PlayerNumber)
        {
            return;
        }

        foreach (var unitInRange in inRange)
        {
            unitInRange.UnMark();
        }
    }

    public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
    {
        if (unit.PlayerNumber != UnitReference.PlayerNumber)
        {
            Debug.Log("Cannot Heal an Enemy");
            return;
        }

        SelectedTarget = unit;

        if (CanPerform(cellGrid))
        {
            Execute(cellGrid, _ => { }, _ => cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid));
        }
    }

    public override void CleanUp(CellGrid cellGrid)
    {
        base.CleanUp(cellGrid);
        if (inRange != null)
        {
            foreach (var unit in inRange)
            {
                unit.UnMark();
            }
        }
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {

    }

    public override string GetDetails()
    {
        return description;
    }
}