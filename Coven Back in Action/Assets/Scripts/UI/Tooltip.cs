using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Units;

public class Tooltip : MonoBehaviour
{
    public string message;
    public int defense;
    public int attack;
    


    private void Start()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;

    }

    private void Update()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;
    }
    private void OnMouseEnter()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;
        TooltipManager._instance.SetAndShowToolTip(attack, defense);
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideToolTip();
    }
}
