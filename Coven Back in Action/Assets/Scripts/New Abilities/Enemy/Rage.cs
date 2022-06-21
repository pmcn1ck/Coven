using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine.UI;
using System.Linq;

public class Rage : Ability
{
    public bool ApplyToSelf = true;
    public int AtkGain = 1;

    public void Start()
    {
        label = "Rage";
        description = "Spend an action point and a hit point to permanently raise your attack by 1";
    }
    public override IEnumerator Act(CellGrid cellGrid)
    {
        Debug.Log("RRRRAAAAAAAARRRRGHHHBLEH (An enemy is raging)");
        GetComponent<ParticlePlayer>().CallExtraParticle(0);
        GetComponent<Unit>().HitPoints -= AtkGain;
        GetComponent<Unit>().HealthSlider.value = GetComponent<Unit>().HitPoints;
        GetComponent<Unit>().AttackFactor += AtkGain;
        GetComponent<Unit>().ActionPoints--;


        yield return 0;
    }

    public override void Activate(CellGrid cellGrid)
    {
        if (GetComponent<Unit>().ActionPoints <= 0)
        {
            Debug.Log("No Action Points Left!");
        }
        else
        {
            StartCoroutine(Act(cellGrid));
        }

    }
}
