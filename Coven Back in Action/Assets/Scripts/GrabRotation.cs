using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    float ratationSpeed = 1f;


    private void OnMouseDrag()
    {
        float XaxisRotaion = Input.GetAxis("Mouse X") * ratationSpeed;
       // float YaxisRotation = Input.GetAxis("Mouse Y") * ratationSpeed;

        transform.Rotate(Vector3.down, XaxisRotaion);
       // transform.Rotate(Vector3.right, YaxisRotation);
    }
}
