using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickerLight : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }    
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        float curDelay = Random.Range(0.01f, timeDelay);
        yield return new WaitForSeconds(curDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        curDelay = Random.Range(0.01f, timeDelay);
        yield return new WaitForSeconds(curDelay);
        isFlickering = false;

    }

}
