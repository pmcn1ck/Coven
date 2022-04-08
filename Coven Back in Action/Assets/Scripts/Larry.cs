using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Larry : MonoBehaviour
{
    //this is not intended to be the final "Unit" class. That class will probably be based on this one, which is for trial-and-erroring basic systems.
    public GridBehavior map;
    public List<GameObject> path = new List<GameObject>();
    public int currentGridX = 0;
    public int currentGridY = 0;
    public int initiativeBonus = 4;
    public int initiativeThisRound = 0;

    public void SetPath()
    {
        path = map.path;
        path.Reverse();
    }

    public void MoveLarry()
    {
        StopAllCoroutines();
        StartCoroutine(Movement());
    }

    public IEnumerator Movement()
    {
        foreach (GameObject space in path)
        {
            Vector3 currentPos = transform.position;
            Vector3 newPos = space.transform.position;
            newPos.y = 1.25f;
            float elapsedTime = 0f;
            float waitTime = .25f;

            while (elapsedTime < waitTime)
            {
                this.transform.position = Vector3.Lerp(currentPos, newPos, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = newPos;
            currentGridX = space.GetComponent<GridStat>().x;
            currentGridY = space.GetComponent<GridStat>().y;
        }
        path.Clear();
        MapSync();
    }
    
    public void MapSync()
    {
        Debug.Log("Setting starting position for navigation to position of unit in current turn");
        map.startX = currentGridX;
        map.startY = currentGridY;
    }
}
