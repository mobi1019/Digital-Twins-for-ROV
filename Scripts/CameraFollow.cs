using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    // public GameObject rosOO;

    void Start()
    {
        // lock and hide Cursor

    }

    void Update()
    {
        // Debug.Log(Input.mousePosition.ToString());
        // Debug.Log(Input.GetMouseButton(0));
        if (Input.GetMouseButton(0)){
            transform.eulerAngles += mouseSensitivity * new Vector3(x:-Input.GetAxis("Mouse Y"),y:Input.GetAxis("Mouse X"),z:0);
        }
        
    }

}