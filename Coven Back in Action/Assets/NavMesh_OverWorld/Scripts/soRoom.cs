using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Room", menuName = "CombatRoom")]
public class soRoom : ScriptableObject
{
    public bool combatArena;
    public int expRewards;
    [Tooltip("This is case sensitive")]
    public eScene Scene;
    public string eventName;
    [TextArea(15,20)]
    public string description;
}
