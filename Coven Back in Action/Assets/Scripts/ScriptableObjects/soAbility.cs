using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Team/Create Ability")]
public class soAbility : ScriptableObject
{
    public string abilityName;
    public GameObject prefab;
    public int unlockLevel;
}
