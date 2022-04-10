using System.Collections;
using System.Collections.Generic;
using TbsFramework.Cells;
using UnityEngine;

public class BlockCell : Square
{

    public override void MarkAsReachable()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public override void MarkAsPath()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    public override void MarkAsHighlighted()
    {
        GetComponent<Renderer>().material.color = Color.magenta;
    }

    public override void UnMark()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
    public override Vector3 GetCellDimensions()
    {
        return GetComponent<Renderer>().bounds.size;
    }
}
