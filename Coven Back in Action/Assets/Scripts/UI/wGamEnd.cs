using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TbsFramework.Units;
using TbsFramework.Grid;

public class wGamEnd : MonoBehaviour
{
    public GameObject w_UpgradeRanger;
   // public Transform tUpgradeRanger;
    public GameObject upgradeShieldBearer;
    public GameObject upgradeSpotter;
    public GameObject upgradeShotgunner;
    public GameObject upgradeGrp;
    public GameObject bUnitClicked;
    public Slider levelSlider;
    public GameObject victroy;
    public GameObject w_CangeUint;
    public Transform tChangeUnit;


    public Text tExp;
    CellGrid CellGrid;

    private void Start()
    {
        //Debug.Log(" experience " + GameManager.gm.Party[0].GetComponent<Unit>().experience);
        //Debug.Log(" experience " + GameManager.gm.Party[0].GetComponent<ExperimentalUnit>().experience);
        //levelSlider.value = GameManager.gm.Party[0].GetComponent<Unit>().experience / 100;
        CellGrid = FindObjectOfType<CellGrid>();
        List<Unit> playableUnits = CellGrid.GetCurrentPlayerUnits();
        //tExp.text = playableUnits[1].experience.ToString();
       // levelSlider.value = (float)playableUnits[1].experience /100f;
        StartCoroutine(VictroyUi());
    }

    IEnumerator VictroyUi()
    {
        victroy.SetActive(true);
        yield return new WaitForSeconds(2);
        victroy.SetActive(false);
    }

    public void OnClickUpgradeRanger()
    {
        w_UpgradeRanger.SetActive(true);
    //    levelSlider.value = unit.experience;
        
        
    }


    public void OnClickUpgradeShieldBearer()
    {
        upgradeShieldBearer.SetActive(true);
        bUnitClicked.SetActive(false);
    }
    public void OnClickUpgradeSpotter()
    {
        upgradeSpotter.SetActive(true);
        bUnitClicked.SetActive(false);
    }
    public void OnClickUpgradeShotgunner()
    {
        upgradeShotgunner.SetActive(true);
        bUnitClicked.SetActive(false);
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
        this.gameObject.SetActive(false);
        Debug.Log("this is change is working");
        Instantiate(w_CangeUint, tChangeUnit);
        //GameManager.gm.SetCam();


    }

    



}
