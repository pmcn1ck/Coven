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

    public Image playerImage;
    public Image Stamp;
    public Sprite iRanger;
    public Sprite iSpotter;
    public Sprite iShieldBearer;
    public Sprite iShotGunner;



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


    public void OnPointEnter()
    {
        if (GameManager.gm.audioManager != null)
        GameManager.gm.audioManager.PlaySound(eSound.hover);
    }
    public void ToggleStats()
    {
        if (wStats.activeSelf == false)
        {
            wStats.SetActive(true);
            if (GameManager.gm.audioManager != null)
                GameManager.gm.audioManager.PlaySound(eSound.click);
            GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
        }
        else
        {
            if (GameManager.gm.audioManager != null)
                GameManager.gm.audioManager.PlaySound(eSound.click);
            wStats.SetActive(false);
        }
    }

    public void ToggleAbilities()
    {
        attachedUnit.gameObject.transform.GetComponent<TbsFramework.HOMMExample.SpellCastingAbility>().Display(cellGrid);



        if (wAbilities.activeSelf == false)
        {

            wAbilities.SetActive(true);
            if (AbilityOne == null)
            {
                AbilityOneButton.SetActive(false);
            }
            if (GameManager.gm.audioManager != null)
                GameManager.gm.audioManager.PlaySound(eSound.click);
        }
        else
        {

            wAbilities.SetActive(false);
            if (GameManager.gm.audioManager != null)
                GameManager.gm.audioManager.PlaySound(eSound.click);
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
                playerImage.sprite = iShotGunner;
                Stamp.color = new Color32(255, 150, 0, 100);
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
                else if (attachedUnit.GetComponent<ScatterShot>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<ScatterShot>();
                }
                break;
            case eUnitType.Spotter:
                text.text = "Spotter";
                playerImage.sprite = iSpotter;
                Stamp.color = new Color32(255, 0, 0, 100);
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
                else if (attachedUnit.GetComponent<SupportingFire>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<SupportingFire>();
                }
                break;
            case eUnitType.Ranger:
                text.text = "Ranger";
                playerImage.sprite = iRanger;
                Stamp.color = new Color32(15, 180, 0, 100);
                Ability.interactable = _isRang; 
                /*gameObject.SetActive(true);
                GetComponent<sPlayerUnitStats>().ShowStats(cellGrid.GetCurrentSelectedUnit());
                if (attachedUnit.GetComponent<TargetMark>() != null)
                {
                    AbilityZero = attachedUnit.GetComponent<TargetMark>();
                }
                if (attachedUnit.GetComponent<RejuvenatingFlask>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<RejuvenatingFlask>();
                }
                else if (attachedUnit.GetComponent<HinderingShot>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<HinderingShot>();
                }*/
                break;
            case eUnitType.ShieldBearer:
                text.text = "ShieldBearer";
                playerImage.sprite = iShieldBearer;
                Stamp.color = new Color32(0, 30, 150, 100);

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
                else if (attachedUnit.GetComponent<ShieldBash>() != null)
                {
                    AbilityOne = attachedUnit.GetComponent<ShieldBash>();
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
        if (GameManager.gm.audioManager != null)
        {
            GameManager.gm.audioManager.PlaySound(eSound.click);
        }
        GameManager.gm.isAttacking = true;

    }

    public void OnClickAbility(int level)
    {
        if (GameManager.gm.audioManager != null)
            GameManager.gm.audioManager.PlaySound(eSound.click);
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
