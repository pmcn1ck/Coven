using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHintV2 : MonoBehaviour
{
    public string description = "placeholder";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        TooltipManager._instance.SetAndShowAbilityHint(description);
        Debug.Log("Hovered Mouse over Button for Ability ");
    }

    public void OnPointerExit()
    {
        TooltipManager._instance.HideToolTip();
    }
}
