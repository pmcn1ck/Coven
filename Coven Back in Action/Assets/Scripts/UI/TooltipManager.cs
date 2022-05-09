using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;

    public TextMeshProUGUI textComponent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void UpdateToolTip(int maxHP, int curHP)
    {
        
    }
    public void SetAndShowToolTip(int attack, int defense)
    {
        gameObject.SetActive(true);
        textComponent.text = "Attack: " + attack + "\n" + "Defense: " + defense;
    }

    public void SetAndShowAbilityHint(string description)
    {
        gameObject.SetActive(true);
        textComponent.text = description;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty; 
    }
}
