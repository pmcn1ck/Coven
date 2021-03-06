using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TbsFramework.Units;
using TbsFramework.Grid;
using TbsFramework.Gui;

public enum eUnitSlider {ranger, shieldBearer, spotter, shotGunner }
public enum eUnitBlood { ranger, shieldBearer, spotter, shotGunner }
public class wGamEnd : MonoBehaviour
{
   // public GameObject w_UpgradeRanger;
   // public Transform tUpgradeRanger;
    //public GameObject upgradeShieldBearer;
   // public GameObject upgradeSpotter;
   // public GameObject upgradeShotgunner;
   // public GameObject upgradeGrp;
    public GameObject bUnitClicked;
    public GameObject canvas;
    public Slider[] healthSlider;
    public Slider[] bloodLustSlider;
    public GameObject victroy;
    public Transform tVicroy;
    public GameObject gameEnd;
    public GameObject w_CangeUint;
    public Transform tChangeUnit;
    public bool isChangeUnit;


    public Text tExp;
    CellGrid CellGrid;
    Unit unit;
    

    private void Start()
    {
        //Debug.Log(" experience " + GameManager.gm.Party[0].GetComponent<Unit>().experience);
        //Debug.Log(" experience " + GameManager.gm.Party[0].GetComponent<ExperimentalUnit>().experience);
        //levelSlider.value = GameManager.gm.Party[0].GetComponent<Unit>().experience / 100;
        CellGrid = FindObjectOfType<CellGrid>();
        //List<Unit> playableUnits = CellGrid.GetCurrentPlayerUnits();
        //unit = FindObjectOfType<Unit>();
        //tExp.text = playableUnits[1].experience.ToString();
        // levelSlider.value = (float)playableUnits[1].experience /100f;
        ShowHealthBarsUi();
        //ShowBloodLustUi();
        
        StartCoroutine(VictroyUi());
    }


    void ShowHealthBarsUi()
    {
        ShowHealthBars(eUnitSlider.ranger, eUnitType.Ranger);
        ShowHealthBars(eUnitSlider.shieldBearer, eUnitType.ShieldBearer);
        ShowHealthBars(eUnitSlider.spotter, eUnitType.Spotter);
        ShowHealthBars(eUnitSlider.shotGunner, eUnitType.Shotgunner);

    }

    void ShowBloodLustUi()
    {
        ShowBloodLustBars(eUnitBlood.ranger, eUnitType.Ranger);
        ShowBloodLustBars(eUnitBlood.shieldBearer, eUnitType.ShieldBearer);
        ShowBloodLustBars(eUnitBlood.spotter, eUnitType.Spotter);
        ShowBloodLustBars(eUnitBlood.shotGunner, eUnitType.Shotgunner);

    }

    void ShowBloodLustBars(eUnitBlood _slider, eUnitType _Type)
    {
        if (!CellGrid.GetPlayableUnitReference(_Type))
        {
            bloodLustSlider[(int)_slider].maxValue = 100;
            bloodLustSlider[(int)_slider].value = 0;
        }
        else
        {
            bloodLustSlider[(int)_slider].maxValue = CellGrid.PlayableUnitReference(_Type).bloodLustMax;
            bloodLustSlider[(int)_slider].value = CellGrid.PlayableUnitReference(_Type).bloodLustMin;
        }

    }

    void ShowHealthBars(eUnitSlider _unitSlider, eUnitType _unitType)
    {
        if (!CellGrid.GetPlayableUnitReference(_unitType)) // Unit died
        {
            healthSlider[(int)_unitSlider].maxValue = 100;
            healthSlider[(int)_unitSlider].value = 0;
            
        }
        else // Unit survived
        {
            healthSlider[(int)_unitSlider].maxValue = CellGrid.GetPlayableUnitReference(_unitType).MaxHitPoints;
            healthSlider[(int)_unitSlider].value = CellGrid.GetPlayableUnitReference(_unitType).HitPoints;
            
        }     
    }

    IEnumerator VictroyUi()
    {
        //victroy.SetActive(true);
        GameObject obj = Instantiate(victroy, tVicroy);
        gameEnd.SetActive(false);
        yield return new WaitForSeconds(2);
        //victroy.SetActive(false);
        Destroy(obj);
        gameEnd.SetActive(true);
    }

    public void OnClickUpgradeRanger()
    {
       // w_UpgradeRanger.SetActive(true);
    //    levelSlider.value = unit.experience;

    }


    public void OnClickUpgradeShieldBearer()
    {
        //upgradeShieldBearer.SetActive(true);
       // bUnitClicked.SetActive(false);
    }
    public void OnClickUpgradeSpotter()
    {
       // upgradeSpotter.SetActive(true);
       // bUnitClicked.SetActive(false);
    }
    public void OnClickUpgradeShotgunner()
    {
       // upgradeShotgunner.SetActive(true);
        //bUnitClicked.SetActive(false);
    }


    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickOverWorld()
    {
   
        GameManager.gm.LoadScene(eScene.InGame);
    }

    public void OnClickChangeUnit()
    {
        //Instantiate(w_CangeUint, tChangeUnit);
        Debug.Log("this code is working");
        isChangeUnit = true;
        Destroy(gameObject);

    }





}
