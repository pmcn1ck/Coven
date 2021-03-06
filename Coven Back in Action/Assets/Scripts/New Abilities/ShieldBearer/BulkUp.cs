using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.HOMMExample;
using UnityEngine.UI;
using System.Linq;

public class BulkUp : SpellAbility
{
    public int Range = 2;
    public int TurnCounter = 0;
    public int Duration = 3;
    public bool ApplyToSelf = false;
    public bool CurrentlyActive = false;
    List<Unit> AffectedUnits = new List<Unit>();
    public int PlayerNumber = 0;
    public float BloodLustCost = 10f;

    public void Start()
    {
        label = "Bulk Up";
        description = "Raise your bloodlust to increase the defense of allies within 2 spaces";
        playerPicksTarget = false;
        CancelButton = GetComponentInParent<SpellCastingAbility>().CancelButton;
    }

    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("Activating Bulk Up");
        GetComponentInParent<Unit>().BloodLust += BloodLustCost;
        GetComponentInParent<Unit>().BloodLustSlider.value += BloodLustCost;
        PlayerNumber = GetComponentInParent<Unit>().PlayerNumber;
        List<Unit> allyUnits = new List<Unit>();
        allyUnits.AddRange(cellGrid.GetCurrentPlayerUnits());
        var AffectedUnits = allyUnits.Where(u => u.Cell.GetDistance(UnitReference.Cell) <= Range);
        gameObject.GetComponentInParent<Unit>().animScript.runCastAnim();

        foreach (var unit in AffectedUnits)
        {
            if (unit.Equals(UnitReference) && !ApplyToSelf)
            {
                continue;
            }
            unit.DefenceFactor += 2;
            unit.GetComponent<ParticlePlayer>().CallExtraParticle(1);
        }
        CurrentlyActive = true;
        TurnCounter = 0;

        yield return 0;
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            TurnCounter++;
            if (TurnCounter >= Duration)
            {
                foreach (var unit in AffectedUnits)
                {
                    if (unit.Equals(UnitReference) && !ApplyToSelf)
                    {
                        continue;
                    }
                    unit.DefenceFactor -= 2;
                }
                Debug.Log("Bulk Up ending");
                CurrentlyActive = false;
                TurnCounter = 0;
            }
        }

    }

    public override void Activate(CellGrid cellGrid)
    {
        if (CurrentlyActive == true)
        {
            Debug.Log("Bulk Up is already active, wait for it to wear off!");
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
