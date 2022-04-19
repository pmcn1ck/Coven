using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using TbsFramework.Units.Abilities;
using TbsFramework.Cells;
using TbsFramework.Grid;

public class wPlayer : MonoBehaviour
{
    public TMP_Text text;
    public Button Ability;
    public TMP_Text AbilityZeroName;
    public GameObject AbilityOneButton;
    public TMP_Text AbilityOneName;
    private ExperimentalUnit attachedUnit;
    public Ability AbilityZero;
    public Ability AbilityOne;
    public CellGrid cellGrid;
    public GameObject wStats;
    public GameObject wAbilities;
    TbsFramework.Units.Unit curUnit;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            Debug.Log("unit = " + attachedUnit);
            for (int i = 0; i < cellGrid.GetCurrentPlayerUnits().Count; i++)
            {
                curUnit = cellGrid.GetCurrentPlayerUnits()[i];
                curUnit.SetState(new TbsFramework.Units.UnitStates.UnitStateMarkedAsFriendly(curUnit));
                curUnit.curState = eState.friendly;
                cellGrid.CellGridState = new TbsFramework.Grid.GridStates.CellGridStateWaitingForInput(cellGrid);
            }
            gameObject.SetActive(false);
        }
    }



    public void ToggleStats()
    {
        if (wStats.activeSelf == false)
        {
            wStats.SetActive(true);
            GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
        }
        else
        {
            wStats.SetActive(false);
        }
    }

    public void ToggleAbilities()
    {
        if (wAbilities.activeSelf == false)
        {
            wAbilities.SetActive(true);
            if (AbilityOne == null)
            {
                AbilityOneButton.SetActive(false);
            }
        }
        else
        {
            wAbilities.SetActive(false);
        }
    }


    public void InitUI(eUnitType _UnitType,bool _isRang, ExperimentalUnit unit, CellGrid cellGridIn)
    {
        attachedUnit = unit;
        cellGrid = cellGridIn;
        switch (_UnitType)
        {
            case eUnitType.None:
                gameObject.SetActive(false);
                break;
            case eUnitType.Shotgunner:
                text.text = "Shotgunner";
                Ability.interactable = _isRang;
                gameObject.SetActive(true);
                GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
                if (attachedUnit.GetComponent<LightFooted>() != null)
                {
                    AbilityZero = attachedUnit.GetComponent<LightFooted>();
                }
                if (attachedUnit.GetComponent<DoubleTap>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<DoubleTap>();
                }
                break;
            case eUnitType.Spotter:
                text.text = "Spotter";
                Ability.interactable = _isRang;
                gameObject.SetActive(true);
                GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
                if (attachedUnit.GetComponent<DeepBreath>() != null)
                {
                    AbilityZero = attachedUnit.GetComponent<DeepBreath>();
                }
                if (attachedUnit.GetComponent<FocusedSights>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<FocusedSights>();
                }
                break;
            case eUnitType.Ranger:
                text.text = "Ranger";
                Ability.interactable = _isRang;
                gameObject.SetActive(true);
                GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
                if (attachedUnit.GetComponent<TargetMark>() != null)
                {
                    AbilityZero = attachedUnit.GetComponent<TargetMark>();
                }
                if (attachedUnit.GetComponent<RejuvenatingFlask>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<RejuvenatingFlask>();
                }
                break;
            case eUnitType.ShieldBearer:
                text.text = "ShieldBearer";
                Ability.interactable = _isRang;
                gameObject.SetActive(true);
                GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
                if (attachedUnit.GetComponent<BulkUp>() != null)
                {
                    AbilityZero = attachedUnit.GetComponent<BulkUp>();
                }
                if (attachedUnit.GetComponent<Counter>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<Counter>();
                }
                break;
        }
        if (AbilityZero != null)
        {
            AbilityZeroName.text = AbilityZero.label;
        }
        if (AbilityOne != null)
        {
            AbilityOneName.text = AbilityOne.label;
        }
    }

    public void OnclickAttack()
    {
        GameManager.gm.isAttacking = true;
    }

    public void OnClickAbility(int level)
    {
        if (level == 0 && AbilityZero != null)
        {
            AbilityZero.Activate(cellGrid); ;
        }
        else if (level == 1 && AbilityOne != null)
        {
            AbilityOne.Activate(cellGrid);
        }

        wAbilities.SetActive(false);
    }

}
