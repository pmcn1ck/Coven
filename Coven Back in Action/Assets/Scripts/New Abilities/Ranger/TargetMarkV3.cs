using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using UnityEngine;
using TbsFramework.HOMMExample;


    public class TargetMarkV3 : SpellAbility
    {
        public int nJumps;
        public int Damage;
        private List<Unit> inRange;
        public int DefPenalty = 2;
        public bool CurrentlyActive;
        public Unit target;
        public int counter = 0;
        public int duration = 3;

    public void Start()
    {
        label = "Target Mark";
        description = "Pick any enemy on the field and lower their defense slightly for 3 turns";
        CancelButton = GetComponentInParent<SpellCastingAbility>().CancelButton;
    }

    public Unit SelectedTarget { get; set; }

        public override IEnumerator Act(CellGrid cellGrid)
        {
            if (CanPerform(cellGrid))
            {
                if (inRange == null)
                {
                    inRange = new List<Unit>() { SelectedTarget };
                    var currentUnit = SelectedTarget;
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

                Unit tempUnit = null;
                if (CurrentlyActive == false)
                {
                for (int i = 0; i < inRange.Count; i++)
                    {
                        Unit unitInRange = inRange[i];
                        target = unitInRange;
                        target.LowerDefenseHandler(DefPenalty);
                        counter = 0;
                        CurrentlyActive = true;
                        tempUnit = unitInRange;
                    }
                }
                else
                {
                    Debug.Log("Already have a target");
                }


                UnitReference.MarkAsAttacking(tempUnit);

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
            if (unit.PlayerNumber == UnitReference.PlayerNumber)
            {
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
        if (target != null)
        {
            counter++;
            if (counter >= duration)
            {
                target.LowerDefenseHandler(-DefPenalty);
                CurrentlyActive = false;
            }
        }
        else if (target == null)
        {
            CurrentlyActive = false;
        }
        base.OnTurnEnd(cellGrid);
    }

    public override string GetDetails()
        {
            return description;
        }
    }