using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagement : MonoBehaviour
{
    public GameObject partyManager;
    public Transform partyList;
    public Image characterPortrait;
    public Button characterButton;
    public Button traitRemoveOpen;
    public List<GameObject> buttonList;
    [Header("Trait Removal Window")]
    public GameObject traitRemovalManager;
    public GameObject traitList;
    public Text traitDescription;
    public Text bloodlustRemove;
    public Button traitButton;
    public Button removeTrait;
    public Text traitTextName;

    public GameObject wButtonUnit;

    public void InitUI()
    {
        partyManager.SetActive(true);
        foreach (GameObject item in GameManager.gm.Party)
        {
            wButtonUnit scr = Instantiate(wButtonUnit, partyList).GetComponent<wButtonUnit>();
            scr.InitUI(item.GetComponent<ExperimentalUnit>(), this);
            //Instantiate(characterButton, partyList.transform);
            //characterButton.GetComponentInChildren<Text>().text = item.GetComponent<ExperimentalUnit>().Name;
            //buttonList.Add(item);
            //characterButton.onClick.AddListener(delegate { UnitSelect(item); });
        }
    }

    public void UnitSelect(ExperimentalUnit unit)
    {
        traitRemoveOpen.onClick.AddListener(delegate { OpenTraitRemoval(unit.gameObject); });
        characterPortrait.sprite = unit.characterPortrait;
    }

    public void OpenTraitRemoval(GameObject unit)
    {
        traitRemovalManager.SetActive(true);
        Debug.Log("Unit: " + unit);
        for (int i = 0; i < unit.GetComponent<ApplyTrait>().trait.Count; i++)
        {
            Trait curTrait = unit.GetComponent<ApplyTrait>().trait[i];
            Instantiate(traitButton, traitList.transform);
            traitButton.onClick.AddListener(delegate { SelectTrait(unit, curTrait); });
            traitTextName.text = curTrait.Name;
            
        }
        /*foreach (Trait item in unit.GetComponent<ApplyTrait>().trait)
        {
            ApplyTrait curTrait = unit.GetComponent<ApplyTrait>().trait[item];
            Instantiate(traitButton, traitList.transform);
            traitButton.text = unitTraits.
        }
        */
    }

    public void SelectTrait(GameObject unit, Trait selectedTrait)
    {
        traitDescription.text = selectedTrait.description;
        bloodlustRemove.text = selectedTrait.BloodLustRemove.ToString();
        removeTrait.onClick.RemoveAllListeners();
        removeTrait.onClick.AddListener(delegate { RemoveTrait(unit, selectedTrait); });
    }

    public void RemoveTrait(GameObject unit, Trait selectedTrait)
    {
        unit.GetComponent<ApplyTrait>().trait.Remove(selectedTrait);
    }

    public void CloseUI()
    {
        /*foreach (Button item in partyList)
        {

        }
        */
        partyManager.SetActive(false);
    }

}
