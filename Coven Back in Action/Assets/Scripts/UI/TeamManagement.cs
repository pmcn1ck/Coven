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
    public GameObject wTraitSelect;

    public void InitUI()
    {
        partyManager.SetActive(true);
        traitRemoveOpen.interactable = false;
        for (int i = 0; i < partyList.transform.childCount; i++)
        {
            Object.Destroy(partyList.transform.GetChild(i).gameObject);
        }
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
        traitRemoveOpen.onClick.RemoveAllListeners();
        traitRemoveOpen.onClick.AddListener(delegate { OpenTraitRemoval(unit.gameObject); });
        traitRemoveOpen.interactable = true;
        characterPortrait.sprite = unit.characterPortrait;
    }

    public void OpenTraitRemoval(GameObject unit)
    {


        traitRemovalManager.SetActive(true);
        removeTrait.interactable = false;
        Debug.Log("Unit: " + unit);
        for (int i = 0; i < traitList.transform.childCount; i++)
        {
            Object.Destroy(traitList.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < PersistantSave.ps.unit.Length; i++)
        {
            if(unit.GetComponent<ApplyTrait>().listAssign == PersistantSave.ps.unit[i].unitAssign)
            {
                var persistentList = PersistantSave.ps.unit[i];
                for (int b = 0; b < persistentList.trait.Count; b++)
                {
                    Trait curTrait = persistentList.trait[b];
                    wTraitSelectButton scr = Instantiate(wTraitSelect, traitList.transform).GetComponent<wTraitSelectButton>();
                    scr.InitUI(unit, curTrait, this);
                }
            }
        }
        //current code, commented out for testing
        /*
        for (int i = 0; i < unit.GetComponent<ApplyTrait>().trait.Count; i++)
        {
            Trait curTrait = unit.GetComponent<ApplyTrait>().trait[i];
            wTraitSelectButton scr = Instantiate(wTraitSelect, traitList.transform).GetComponent<wTraitSelectButton>();
            scr.InitUI(unit, curTrait, this);
            Debug.Log("cur trait is: " + curTrait);
            
            //traitButton.onClick.AddListener(delegate { SelectTrait(unit, curTrait); });
            //traitTextName.text = curTrait.Name;
            
        }
        */
        //old code depreciated
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
        Debug.Log("Trait selected: " + selectedTrait);
        removeTrait.onClick.RemoveAllListeners();
        removeTrait.onClick.AddListener(delegate { RemoveTrait(unit, selectedTrait); });
        removeTrait.interactable = true;
    }

    public void RemoveTrait(GameObject unit, Trait selectedTrait)
    {
        Debug.Log("Trait Removed: " + selectedTrait);
        
        for (int i = 0; i < PersistantSave.ps.unit.Length; i++)
        {
            if(unit.GetComponent<ApplyTrait>().listAssign == PersistantSave.ps.unit[i].unitAssign)
            {
                var selectedList = PersistantSave.ps.unit[i];
                selectedList.trait.Remove(selectedTrait);
                OpenTraitRemoval(unit);
            }
        }
        
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
