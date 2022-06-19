using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TraitLists { ShieldBearer1, ShieldBearer2, Shotgunner1, Shotgunner2, Ranger1, Ranger2, Spotter1, Spotter2}

[System.Serializable]
public class UnitSaves
{
    public TraitLists unitAssign;
    public List<Trait> trait;
    //public soAbility[] ability;
}

public class PersistantSave : MonoBehaviour
{
    public UnitSaves[] unit;
    public static PersistantSave ps;


    private void Awake()
    {
        if (ps == null)
        {
            DontDestroyOnLoad(this.gameObject);
            ps = this;
        }
        else if (ps != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit[0].trait.Add(GameManager.gm.trait[0]);
        }
    }


}
