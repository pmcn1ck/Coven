using System.Collections;
using System.Collections.Generic;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.UI;
public enum eUnitType { Shotgunner, Spotter, Ranger, ShieldBearer,None }
public class ExperimentalUnit : Unit
{
    public eUnitType unitType;
    public Color LeadingColor;
    public GameObject model;
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
        model.GetComponent<Renderer>().material.color = Color.green;
    }

    public override void MarkAsReachableEnemy()
    {
        model.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void MarkAsSelected()
    {
        model.GetComponent<Renderer>().material.color = Color.yellow;
    }


    public override void MarkAsFinished()
    {
        model.GetComponent<Renderer>().material.color = Color.gray;
    }

    public override void UnMark()
    {
        model.GetComponent<Renderer>().material.color = LeadingColor;
    }

}
