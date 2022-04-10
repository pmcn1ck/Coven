using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public GameObject cam;
    // Update is called once per frame

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        
    }
    void Update()
    {
       transform.rotation = cam.transform.rotation;
    }
}
