using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeManager : MonoBehaviour
{
    public List<Larry> ActiveUnits = new List<Larry>();
    public List<Larry> ActiveUnitsInOrder = new List<Larry>();
    public int counter = 0;
    public int round = 0;

    public void Start()
    {
        IterateRound();
    }

    public void SortIntoTurnOrder()
    {
        for (int i = 0; i < ActiveUnits.Count; i++)
        {
            ActiveUnits[i].initiativeThisRound = Random.Range(0, 100) + ActiveUnits[i].initiativeBonus;
            if (i == 0)
            {
                ActiveUnitsInOrder.Add(ActiveUnits[i]);
            }
            else
            {
                for (int j = 0; j < ActiveUnitsInOrder.Count; j++)
                {
                    if (ActiveUnits[i].initiativeThisRound > ActiveUnitsInOrder[j].initiativeThisRound)
                    {
                        ActiveUnitsInOrder.Insert(j, ActiveUnits[i]);
                        break;
                    }
                    else if (j == ActiveUnitsInOrder.Count - 1)
                    {
                        ActiveUnitsInOrder.Add(ActiveUnits[i]);
                        break;
                    }
                }
            }
        }
    }

    public void IterateRound()
    {
        ActiveUnitsInOrder.Clear();
        round++;
        counter = 0;
        SortIntoTurnOrder();
        Debug.Log("Now beginning round " + round);
    }

    public void IterateTurn()
    {
        counter++;
        if (counter >= ActiveUnitsInOrder.Count)
        {
            IterateRound();
        }
        ActiveUnitsInOrder[counter].MapSync();
    }

    public void MoveUnit()
    {
        ActiveUnitsInOrder[counter].SetPath();
        ActiveUnitsInOrder[counter].MoveLarry();
    }
}
