using System.Collections;
using System.Collections.Generic;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using TbsFramework.Grid;

public class cPlayer : MonoBehaviour
{
    public GameObject playerInfo;

    public Camera cam;
    public CellGrid cellGrid;
    public Transform ptPlayerInfo;
    bool isRang;
    wPlayer widget;
    public GameObject wStats;
 


       
    private void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            if (cellGrid.GetCurrentSelectedUnit())
            {
                if (!widget)
                {
                    SpawnPlayerInfo();
                }
                else
                {
                    RefreshPlayerInfo();
                }
            }
            else  
            {
                Destroy(widget?.gameObject);
                widget = null;
            }
        }*/
        ShowWidget();
    }

    public void ShowWidget()
    {
        if (cellGrid.GetCurrentSelectedUnit())
        {
            if (!widget)
            {
                SpawnPlayerInfo();
            }
            else
            {
                RefreshPlayerInfo();
            }
        }
        else
        {
            Destroy(widget?.gameObject);
            widget = null;
        }
    }

    void SpawnPlayerInfo()
    {
        isRang = true;
        widget = Instantiate(playerInfo, ptPlayerInfo).GetComponent<wPlayer>();
        TbsFramework.Units.Unit curUnit = cellGrid.GetCurrentSelectedUnit();
        ExperimentalUnit curExpUnit = curUnit.gameObject.GetComponent<ExperimentalUnit>();
        widget.InitUI(curExpUnit.unitType, isRang, curExpUnit, cellGrid);
    }

    void RefreshPlayerInfo()
    {
        TbsFramework.Units.Unit curUnit = cellGrid.GetCurrentSelectedUnit();
        ExperimentalUnit curExpUnit = curUnit.gameObject.GetComponent<ExperimentalUnit>();
        widget.InitUI(curExpUnit.unitType, isRang, curExpUnit, cellGrid);
    }

    /*void SpawnPlayerInfo()
    {
        isRang = true;
        RaycastHit hit;
       
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject())
        {


            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                ExperimentalUnit playerUnit = hit.transform.GetComponent<ExperimentalUnit>();
                GameManager.gm.CurrentUnit = hit.transform.gameObject;
                if (playerUnit != null)
                {
                    // this were range goes
                    if (widget == null)
                    {
                        widget = Instantiate(playerInfo, this.gameObject.transform).GetComponent<wPlayer>(); ;
                    }
                widget.InitUI(playerUnit.unitType,isRang, playerUnit, cellGrid); 
                }
                else
                {
                    Destroy(widget?.gameObject);
                    widget = null;

                }

            }
        }
    }*/
}
