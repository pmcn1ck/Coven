using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Team/Create Unit")]
public class soUnit : ScriptableObject
{
    public string unitName;
    public GameObject unitPrefab;
    public soAbility[] ability;
}
