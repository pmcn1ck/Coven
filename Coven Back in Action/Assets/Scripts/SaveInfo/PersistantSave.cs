using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSaves
{
    public string unitType;
    public soUnit unit;
    public List<Trait> trait;
    //public soAbility[] ability;
}

public class PersistantSave : MonoBehaviour
{
    public UnitSaves[] unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit[0].trait.Add(GameManager.gm.trait[0]);
        }
    }


}
