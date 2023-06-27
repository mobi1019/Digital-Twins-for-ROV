using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 2f;


    void Start()
    {
        // lock and hide Cursor

    }

    void Update()
    {
        if (Input.GetMouseButton(0)){
            transform.eulerAngles += mouseSensitivity * new Vector3(x:-Input.GetAxis("Mouse Y"),y:Input.GetAxis("Mouse X"),z:0);

        }
       
    }

}