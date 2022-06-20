using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;

public class ScatterShot : SpellAbility
{
    List<Unit> UnitsInRange = new List<Unit>();
    public float BloodCost = 15f;

    public void Start()
    {
        label = "Scatter Shot";
        description = "Raise your bloodlust significantly to deliver a light attack to up to 3 random units in your range";
        playerPicksTarget = false;
    }

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Scatter Shot");
        foreach (Unit u in cellGrid.GetEnemyUnits(cellGrid.Players[GetComponent<Unit>().PlayerNumber])){
            if (u.Cell.GetDistance(GetComponentInParent<Unit>().Cell) <= GetComponentInParent<Unit>().AttackRange)
            {
                UnitsInRange.Add(u);
            }
        }
        if (UnitsInRange == null)
        {
            Debug.Log("No Enemies in Range");
        }
        else
        {
            GetComponentInParent<Unit>().BloodLust += BloodCost;
            GetComponentInParent<Unit>().BloodLustSlider.value += BloodCost;
            int targetNum;
            if (UnitsInRange.Count >= 3)
            {
                targetNum = 3;
            }
            else
            {
                targetNum = UnitsInRange.Count;
            }
            int temp = GetComponent<Unit>().AttackFactor;
            GetComponent<Unit>().AttackFactor = temp / 2;
            for (int i = 0; i < targetNum; i++)
            {
                Unit target = UnitsInRange[Random.Range(0, UnitsInRange.Count)];
                GetComponent<Unit>().AttackHandler(target);
                UnitsInRange.Remove(target);
            }
            GetComponentInParent<Unit>().AttackFactor = temp;
            GetComponentInParent<Unit>().ActionPoints--;
        }
        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (GetComponentInParent<Unit>().ActionPoints < 1)
        {
            Debug.Log("Not Enough Action Points");
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
