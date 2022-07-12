using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wChangeUnit : MonoBehaviour
{
    public GameObject ranger;
    public GameObject spotter;
    public GameObject shotgunner;
    public GameObject shieldBearer;
    public GameObject endGame;
    public bool isBack;
    

    void Start()
    {
       
    }

    public void OnClickSpotter()
    {
        ranger.SetActive(false);
        shotgunner.SetActive(false);
        shieldBearer.SetActive(false);
        spotter.SetActive(true);

    }

    public void onClickRanger()
    {
        shotgunner.SetActive(false);
        spotter.SetActive(false);
        shieldBearer.SetActive(false);
        ranger.SetActive(true);
    }

    public void OnClickShotgunner()
    {
        ranger.SetActive(false);
        spotter.SetActive(false);
        shieldBearer.SetActive(false);
        shotgunner.SetActive(true);


    }

    public void OnClickShieldBearer()
    {
        shotgunner.SetActive(false);
        spotter.SetActive(false);
        ranger.SetActive(false);
        shieldBearer.SetActive(true);
        
    }

    public void OnClickBack()
    {
        Destroy(gameObject);
        endGame.gameObject.SetActive(true);
        isBack = true;

        
    }
}
