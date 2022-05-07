using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TbsFramework.Units;

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
    public Unit unit;


  
    public void OnClickUpgradeRanger()
    {
        w_UpgradeRanger.SetActive(true);
        levelSlider.value = unit.experience;
        
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


}
