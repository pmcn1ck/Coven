using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wTraitSelectButton : MonoBehaviour
{
    public Text textButton;
    private TeamManagement parent;
    private GameObject unit;
    private Trait selectedTrait;
    public void InitUI(GameObject _unit, Trait _trait, TeamManagement _parent)
    {
        unit = _unit;
        textButton.text = _trait.Name;
        parent = _parent;
        selectedTrait = _trait;
    }
    public void OnPressed()
    {
        parent.SelectTrait(unit, selectedTrait);
    }
}
