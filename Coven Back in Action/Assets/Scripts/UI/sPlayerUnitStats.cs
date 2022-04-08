using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TbsFramework.Units;

public class sPlayerUnitStats : MonoBehaviour
{
    public TextMeshProUGUI rangeNum, attackNum, defenseNum, moveNum, multiplier;

    public void ShowStats(Unit _curUnit)
    {
        rangeNum.text = _curUnit.AttackRange.ToString();
        attackNum.text = _curUnit.AttackFactor.ToString();
        defenseNum.text = _curUnit.DefenceFactor.ToString();
        moveNum.text = _curUnit.MovementPoints.ToString();
        multiplier.text = _curUnit.BloodMultiplier.ToString();
    }
}
