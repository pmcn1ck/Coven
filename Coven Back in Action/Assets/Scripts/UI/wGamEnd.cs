using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class wGamEnd : MonoBehaviour
{
    public GameObject upgradeRanger;
    public GameObject upgradeShieldBearer;
    public GameObject upgradeSpotter;
    public GameObject upgradeShotgunner;
    public GameObject upgradeGrp;
    public GameObject bUnitClicked;


    public void OnClickUpgradeRanger()
    {
        upgradeRanger.SetActive(true);
        bUnitClicked.SetActive(false);
        upgradeGrp.SetActive(true);
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

    public void OnClickDestroy()
    {
        upgradeGrp.SetActive(false);
        bUnitClicked.SetActive(true);
        
        
    }


}
