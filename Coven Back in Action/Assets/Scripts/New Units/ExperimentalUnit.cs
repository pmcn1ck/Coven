using System.Collections;
using System.Collections.Generic;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;
using UnityEngine.UI;
public enum eUnitType { Shotgunner, Spotter, Ranger, ShieldBearer,None }
public class ExperimentalUnit : Unit
{
    public eUnitType unitType;
    public Color LeadingColor;
    public GameObject model;
    public soAbility[] so_Ability;
    public Ability[] abilities;
    public Material indicator;

    public void Start()
    {
        indicator = model.GetComponent<Renderer>().sharedMaterial;
        if (unitType != eUnitType.None)
        {
            MarkAsFriendly();
        }
        else
        {
            UnMark();
        }
    }
    public override void Initialize()
    {
        base.Initialize();
        transform.localPosition -= new Vector3(0, 0, 0);
    }
    public override void MarkAsDefending(Unit aggressor)
    {

    }

    public override void MarkAsAttacking(Unit target)
    {
        Debug.Log("Player is taking damage");
    }

    public override void MarkAsDestroyed()
    {

    }

    public override void MarkAsFriendly()
    {
        indicator.color = Color.green;
        indicator.EnableKeyword("_EMISSION");
        indicator.SetColor("_EmissionColor", Color.green);
    }

    public override void MarkAsReachableEnemy()
    {
        indicator.color = Color.red;
        indicator.EnableKeyword("_EMISSION");
        indicator.SetColor("_EmissionColor", Color.red);
    }

    public override void MarkAsSelected()
    {
        indicator.color = Color.yellow;
        indicator.EnableKeyword("_EMISSION");
        indicator.SetColor("_EmissionColor", Color.yellow);
    }


    public override void MarkAsFinished()
    {
        indicator.color = Color.gray;
        indicator.EnableKeyword("_EMISSION");
        indicator.SetColor("_EmissionColor", Color.grey);
    }

    public override void UnMark()
    {
        indicator.color = LeadingColor;
        indicator.EnableKeyword("_EMISSION");
        indicator.SetColor("_EmissionColor", LeadingColor);
    }

}
