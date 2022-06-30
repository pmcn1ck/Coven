using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wAbilitiesUpgrade : MonoBehaviour
{
    public GameObject abilityOne;
    public Button bAbilityone;
    public GameObject abilitytwo;
    public Button bAbilitytwo;
    public Button bAbilityThree;
    public GameObject abilityThree;
    public Button bAbilityFour;
    public GameObject abiilityFour;
    public bool isUpgrade;


    private void Update()
    {
        Upgrade();
    }

    public void OnClickAbilityOne()
    {
        abilityOne.SetActive(true);
        abilitytwo.SetActive(false);
        
    }

    public void OnClickAbilitytwo()
    {
        abilityOne.SetActive(false);
        abilitytwo.SetActive(true);
    }

    public void OnClickAbilitythree()
    {
        abilitytwo.SetActive(false);
        abilityThree.SetActive(true);
        abiilityFour.SetActive(false);

    }

    public void OnClickAbilityFour()
    {
        abilityThree.SetActive(false);
        abiilityFour.SetActive(true);

    }

    public void Upgrade()
    {
        if( isUpgrade == false)
        {
            bAbilityThree.interactable = false;
            bAbilityFour.interactable = false;
        }
        else
        {
            bAbilityThree.interactable = true;
            bAbilityFour.interactable = true;
        }
        if (isUpgrade == true)
        {
            bAbilityone.interactable = false;
            bAbilitytwo.interactable = false;

        }

    }


    public void OnClickUpgrade()
    {
       
        isUpgrade = true;
        gameObject.SetActive(false);
       
    }
}
