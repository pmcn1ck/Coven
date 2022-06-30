using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Units;

public class Tooltip : MonoBehaviour
{
    public string message;
    public int defense;
    public int attack;
    public Texture2D cursorTexture;
    public Texture2D cursorTextureFirendly;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public ExperimentalUnit Unit;

    private void Awake()
    {
        Unit = FindObjectOfType<ExperimentalUnit>();
    }




    private void Start()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;

    }

    private void Update()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;
    }
    private void OnMouseEnter()
    {
        attack = this.gameObject.GetComponent<Unit>().AttackFactor;
        defense = this.gameObject.GetComponent<Unit>().DefenceFactor;
        TooltipManager._instance.SetAndShowToolTip(attack, defense);
        if(Unit.mouseType == eMouseType.Firendly)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            Debug.Log("mouse is over " + eMouseType.Firendly);
        }
        else
        {
            Cursor.SetCursor(cursorTextureFirendly, hotSpot, cursorMode);
            Debug.Log("mouse is over " + eMouseType.none);
        }
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideToolTip();
    } 
}
