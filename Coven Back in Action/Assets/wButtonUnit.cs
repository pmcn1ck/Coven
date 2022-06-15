using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wButtonUnit : MonoBehaviour
{
    public Text textButton;
    private TeamManagement parent;
    private ExperimentalUnit unit;
    public void InitUI(ExperimentalUnit _unit, TeamManagement _parent)
    {
        unit = _unit;
        textButton.text = unit.Name;
        parent = _parent;
    }

    public void OnPressed()
    {
        parent.UnitSelect(unit);
    }
}
