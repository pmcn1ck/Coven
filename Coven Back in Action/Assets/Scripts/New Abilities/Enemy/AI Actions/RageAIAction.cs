using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players.AI.Evaluators;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;

namespace TbsFramework.Players.AI.Actions
{
    public class RageAIAction : AIAction
    {
        private Dictionary<Unit, string> unitDebugInfo;
        private List<(Unit unit, float value)> unitScores;

        public override bool ShouldExecute(Player player, Unit unit, CellGrid cellGrid)
        {
            if (unit.GetComponent<Rage>() == null)
            {
                return false;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(player);
            var isEnemyinRange = enemyUnits.Select(u => unit.IsUnitAttackable(u, unit.Cell))
                                           .Aggregate((result, next) => result || next);

            return !isEnemyinRange && unit.ActionPoints > 0 && unit.HitPoints > 1;
        }
        public override void Precalculate(Player player, Unit unit, CellGrid cellGrid)
        {
            Debug.Log("If the rage had a target, it would be calculated here");
        }
        public override IEnumerator Execute(Player player, Unit unit, CellGrid cellGrid)
        {
            unit.GetComponent<Rage>().Activate(cellGrid);
            yield return new WaitForSeconds(0.5f);
        }
        public override void CleanUp(Player player, Unit unit, CellGrid cellGrid)
        {
            foreach (var enemy in cellGrid.GetEnemyUnits(player))
            {
                enemy.UnMark();
            }
            unitScores = null;
        }
        public override void ShowDebugInfo(Player player, Unit unit, CellGrid cellGrid)
        {
            (cellGrid.CellGridState as CellGridStateAITurn).UnitDebugInfo = unitDebugInfo;

            if (unitScores == null)
            {
                return;
            }

            var minScore = unitScores.DefaultIfEmpty().Min(e => e.value);
            var maxScore = unitScores.DefaultIfEmpty().Max(e => e.value);
            foreach (var (u, value) in unitScores)
            {
                var color = Color.Lerp(Color.red, Color.green, value >= 0 ? value / maxScore : value / minScore * (-1));
                u.SetColor(color);
            }
        }
    }
}
