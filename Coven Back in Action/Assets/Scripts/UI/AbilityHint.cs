using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHint : MonoBehaviour
{
    public string description;
    public wPlayer wPlayer;
    public int AbilityLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (AbilityLevel == 0)
        {
            description = wPlayer.AbilityZero.description;
            Debug.Log("Ability Zero Description set");
        }
        else if (AbilityLevel == 1)
        {
            description = wPlayer.AbilityOne.description;
            Debug.Log("Ability One Description set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        TooltipManager._instance.SetAndShowAbilityHint(description);
        Debug.Log("Hovered Mouse over Button for Ability " + AbilityLevel);
    }

    public void OnPointerExit()
    {
        TooltipManager._instance.HideToolTip();
    }
}
