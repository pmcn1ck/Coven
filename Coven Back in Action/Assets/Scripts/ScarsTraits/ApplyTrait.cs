using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;

public class ApplyTrait : Ability
{
    /*
    Currently this code works just like the script "AoeBuffAbility"
    The goal is to get it to activate if X amount of enemies are within the set range
    Should probably rename the script maybe even make Public Buff become a list so we can put multiple traits that are work similarly within the same script so it can just be plugged in
    Maybe put Range int into Trait Script so all the info can just be plugged into the list.
    */

    public TraitLists listAssign;
    public List<Trait> trait = new List<Trait>();
    public int Range = 1;
    public CellGrid cellGridHolder;
    

    public override IEnumerator Act(CellGrid cellGrid)
    {
        cellGridHolder = cellGrid;
        var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
        var myUnits = cellGrid.GetCurrentPlayerUnits();
        List<Unit> allUnits = new List<Unit>();
        allUnits.AddRange(enemyUnits);
        allUnits.AddRange(myUnits);
        
        var unitsInRange = allUnits.Where(u => u.Cell.GetDistance(UnitReference.Cell) <= Range);
        bool allyInRange = false;
        bool enemyInRange = false;
        /*
        int currentHealth;
        int maxHealth;
        maxHealth = cellGrid.CurrentPlayer.GetComponent<Unit>().TotalHitPoints;
        currentHealth = cellGrid.CurrentPlayer.GetComponent<Unit>().HitPoints;
        */

        Debug.Log("Focused Unit " + gameObject.name);
        for (int i = 0; i < trait.Count; i++)
        {
            foreach (var unit in unitsInRange)
            {
                Debug.Log("Unit " + unit.name + " " + unit.Cell);
                if (unit.PlayerNumber  == 0 && unit.name != gameObject.name)
                {
                    var distance = GetComponent<Unit>().Cell.GetDistance(unit.Cell);
                    Debug.Log("Range of Unit " + distance);
                   
                    allyInRange = true;
                }
                else if (unit.PlayerNumber == 1 || unit.PlayerNumber == 2)
                {
                    enemyInRange = true;
                }

            }
            if (allyInRange && trait[i].Activator == Trait.UnitType.Ally)
            {
                Debug.Log("Buff Added to Ally");
                this.gameObject.GetComponent<Unit>().AddBuff(trait[i]);
            }
            if (enemyInRange && trait[i].Activator == Trait.UnitType.Enemy)
            {
                this.gameObject.GetComponent<Unit>().AddBuff(trait[i]);
            }
            if (!allyInRange && trait[i].Activator == Trait.UnitType.NoAlly)
            {
                this.gameObject.GetComponent<Unit>().AddBuff(trait[i]);
            }
            if (!enemyInRange && trait[i].Activator == Trait.UnitType.NoEnemy)
            {
                this.gameObject.GetComponent<Unit>().AddBuff(trait[i]);
            }

            yield return 0;
        }
    }
    
    private void OnEnable()
    {
        GetComponent<Unit>().UnitMoved += OnUnitMoved;
    }

    private void OnDisable()
    {
        GetComponent<Unit>().UnitMoved -= OnUnitMoved;
    }
    

    public override void OnTurnStart(CellGrid cellGrid)
    {
        //StartCoroutine(Act(cellGrid));
    }
    
    private void OnUnitMoved(object sender, MovementEventArgs e)
    {
        Debug.Log("Unit Moved - insert code below");
        cellGridHolder = FindObjectOfType<CellGrid>();
        StartCoroutine(Act(cellGridHolder));
        foreach (var trait in trait)
        {
         GetComponent<sUnitAwareness>().CheckForNearbyUnits(trait);
        }
        
    }
    
}
