using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Cells;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;
using UnityEngine.UI;
using System.Linq;

namespace TbsFramework.HOMMExample
{

    public class TargetMarkV2 : SpellAbility
    {
        public int Range;
        public int TurnCounter = 0; // Counts how long the effect is active
        public int Duration = 3; // max duration of script
        public bool ApplyToSelf = false;
        public bool CurrentlyActive = false;
        public bool seekingTarget;
        public Unit Target; // this is the affected unit
        public int DefPenalty = 2; // applying this effect for the duration

        List<Cell> inRange;
        public Cell SelectedCell { get; set; }

        public void Start()
        {
            label = "Target Mark";
            description = "Pick any enemy on the field and lower their defense slightly for 3 turns";
        }
        public override IEnumerator Act(CellGrid cellGrid)
        {
            Debug.Log("Act");

            //if (CanPerform(cellGrid))
            //{
            if (inRange == null)
            {
                inRange = cellGrid.Cells.FindAll(c => c.GetDistance(SelectedCell) <= Range);
            }

            Debug.Log("Act 2");

            Unit tempUnit = null;
            foreach (var cell in inRange)
            {
                foreach (Unit unit in new List<Unit>(cell.CurrentUnits))
                {
                    unit.LowerDefenseHandler(unit, DefPenalty);
                    Debug.Log("Lowering Defense of " + unit.Name);
                    Target = unit;
                    CurrentlyActive = true;
                    if (unit != null)
                    {
                        tempUnit = unit;

                    }
                }
            }

            if (tempUnit != null)
            {
                UnitReference.MarkAsAttacking(tempUnit);
            }

            //GetComponentInParent<SpellCastingAbility>().CancelCasting(); // Must be in every spell, removes UI

            //}
            yield return base.Act(cellGrid);

            //Debug.Log("Targeting " + Target.Name);
            //Target.DefenceFactor -= DefPenalty;

            //yield return 0;
        }

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            Debug.Log("On Cell Selected");
            if (cell == null || cell.CurrentUnits.Count > 0)
            {
                return;
            }

            Debug.Log("On Cell Selected 2");

            inRange = cellGrid.Cells.FindAll(c => c.GetDistance(cell) <= Range);
            inRange.ForEach(c =>
            {
                c.MarkAsHighlighted();
                if (c.CurrentUnits.Count > 0)
                {
                    c.CurrentUnits[0].MarkAsReachableEnemy();
                    Debug.Log("On Cell Selected 3");
                }
            });
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits.Count > 0)
            {
                return;
            }
            if (inRange != null)
            {
                inRange.ForEach(c =>
                {
                    c.UnMark();
                    if (c.CurrentUnits.Count > 0)
                    {
                        if (cellGrid.GetCurrentPlayerUnits().Contains(c.CurrentUnits[0]))
                        {
                            c.CurrentUnits[0].MarkAsFriendly();
                        }
                        else
                        {
                            c.CurrentUnits[0].UnMark();
                        }
                    }
                });
            }
            
        }

        public override void OnUnitHighlighted(Unit unit, CellGrid cellGrid)
        {
            OnCellSelected(unit.Cell, cellGrid);
        }

        public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
        {
            OnCellDeselected(unit.Cell, cellGrid);
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits.Count > 0)
            {
                return;
            }

            StartCoroutine(Act(cellGrid));
            inRange.ForEach(c =>
            {
                c.UnMark();
                if (c.CurrentUnits.Count > 0)
                {
                    if (cellGrid.GetCurrentPlayerUnits().Contains(c.CurrentUnits[0]))
                    {
                        c.CurrentUnits[0].MarkAsFriendly();
                    }
                    else
                    {
                        c.CurrentUnits[0].UnMark();
                    }
                }
            });
            cellGrid.CellGridState = new CellGridStateWaitingForInput(cellGrid);
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            OnCellClicked(unit.Cell, cellGrid);
        }

        public override void OnTurnStart(CellGrid cellGrid)
        {
            inRange = null;
            SelectedCell = null;
        }

        public override void OnTurnEnd(CellGrid cellGrid)
        {
            TurnCounter++;
            if (TurnCounter >= Duration)
            {
                Debug.Log("Target Mark Cooldown Ended");
                CurrentlyActive = false;
                TurnCounter = 0;
                Target.LowerDefenseHandler(Target, -DefPenalty);
            }
        }


        public override void CleanUp(CellGrid cellGrid)
        {
            base.CleanUp(cellGrid);
            OnCellDeselected(null, cellGrid);
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            return !CurrentlyActive;
        }


        /*public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
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
        }*/

        public override string GetDetails()
        {
            return description;
        }
    }
}
