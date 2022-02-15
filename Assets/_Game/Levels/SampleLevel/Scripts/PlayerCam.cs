using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float camSpeed = 0.5f;

    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * -camSpeed;
        float zAxisValue = Input.GetAxis("Vertical") * -camSpeed;

        //transform.position += new Vector3(transform.forward * xAxisValue, transform.forward * xAxisValue,

        transform.position += transform.right * -xAxisValue;
        transform.position += transform.up * -zAxisValue;
        //transform.position += new Vector3(transform.)
    }
}