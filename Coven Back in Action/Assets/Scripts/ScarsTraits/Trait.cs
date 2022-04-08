using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TbsFramework.Units;
using TbsFramework.Grid;
using TbsFramework.Units.Abilities;


[CreateAssetMenu]
public class Trait : Buff
{   
    public enum Stat { Attack, Defense, Heal, BloodMultiplier }
    public enum UnitType { Ally, Enemy, NoAlly, NoEnemy, HalfHealth }
    [Tooltip("Remember BloodMultiplier is 1 by default, and the equation in code is + to this factor. A player wants to see the blood multiplier go down. So when making a trait that lower's this make sure you type it as " +
        "something like '-0.04'Also this Factor is a float but other than BloodMultiplier all unit stats are int's")]
    public float Factor;
    [Tooltip("Selects which stat is affected by the Trait")]
    public Stat affectedStat;
    [Tooltip("Selects the the situation that is required for the Trait to activate")]
    public UnitType Activator;
    public string Name;
    [Tooltip("Write a description for trait")]
    [TextArea(15,20)]
    public string description;
    public int Range = 1;


    public override void Apply(Unit unit)
    {
        
       switch (affectedStat)
        {
            case Stat.Attack:
                unit.AttackFactor += (int)Factor;
                break;
            case Stat.Defense:
                unit.DefenceFactor += (int)Factor;
                break;
            case Stat.Heal:
                unit.HitPoints += (int)Factor;
                break;
            case Stat.BloodMultiplier:
                unit.BloodMultiplier += Factor;
                break;
            default:
                break;
        }
       
    }

    public override void Undo(Unit unit)
    {
        //I Have no idea how to fix what's stopping me here
        switch (affectedStat)
        {
            case Stat.Attack:
                unit.AttackFactor -= (int)Factor;
                break;
            case Stat.Defense:
                unit.DefenceFactor -= (int)Factor;
                break;
            case Stat.BloodMultiplier:
                unit.BloodMultiplier -= Factor;
                break;
            default:
                break;
        }
    }
}
