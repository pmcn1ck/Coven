using System;
using System.Collections;
using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BlockCell[] spawnLocations;
    public GameManager gm;
    public GameObject unitList;
    public CellGrid cellGrid;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        SpawnPlayerUnits();
    }


    void SpawnPlayerUnits()
    {
        int locationNum = 0;

        foreach (var item in gm.Party)
        {
            var unit = item.GetComponent<ExperimentalUnit>();
            if (unit.unitType == eUnitType.ShieldBearer)
            {
                AddUnit(locationNum, item);
                locationNum++;
            }
        }
        foreach (var item in gm.Party)
        {
            var unit = item.GetComponent<ExperimentalUnit>();
            if (unit.unitType == eUnitType.Shotgunner)
            {
                AddUnit(locationNum, item);
                locationNum++;
            }
        }
        foreach (var item in gm.Party)
        {
            var unit = item.GetComponent<ExperimentalUnit>();
            if (unit.unitType == eUnitType.Ranger)
            {
                AddUnit(locationNum, item);
                locationNum++;
            }
        }
        foreach (var item in gm.Party)
        {
            var unit = item.GetComponent<ExperimentalUnit>();
            if (unit.unitType == eUnitType.Spotter)
            {
                AddUnit(locationNum, item);
                locationNum++;
            }
        }
    }

    void AddUnit(int _locationNum, GameObject _unit)
    {
        Debug.Log("Spawning Unit");
        GameObject SpawnedUnit = Instantiate(_unit);
        ExperimentalUnit sUnit = SpawnedUnit.GetComponent<ExperimentalUnit>();
        sUnit.Cell = spawnLocations[_locationNum];
        sUnit.Cell.CurrentUnits.Add(sUnit);

        // Added by GG to handle abilities
        if (sUnit.unitType == eUnitType.Ranger)
        {
            EvaluateAbilities(sUnit);
        }

        SpawnedUnit.transform.localPosition = sUnit.Cell.transform.localPosition;
        SpawnedUnit.transform.localRotation = Quaternion.Euler(0, 0, 0);
        SpawnedUnit.transform.parent = unitList.transform;
        sUnit.PlayerNumber = 0;
        sUnit.Cell.IsTaken = sUnit.Obstructable;
        sUnit.Initialize();
        Debug.Log("it's still working");
        //this line breaks things. might come back to haunt us for not getting it working.
        cellGrid.AddUnit(SpawnedUnit.transform);
        cellGrid.AddPlayableUnit(SpawnedUnit.transform);
    }

    void EvaluateAbilities (ExperimentalUnit _sUnit)
    {
        int count = 0;
        //_sUnit.abilities = new TbsFramework.Units.Abilities.Ability[_sUnit.so_Ability.Length];
        foreach (soAbility so in _sUnit.so_Ability)
        {
            //_sUnit.abilities[count] = Instantiate(so.prefab, _sUnit.gameObject.transform).GetComponent<TbsFramework.Units.Abilities.Ability>();
            //Debug.Log("Unit Ability " + _sUnit.abilities[count]);
            //count++;
        }
    }

    void AddAbility (ExperimentalUnit _sUnit)
    {
        
    }

     
}
