// this controller handles viewpoint 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow3 : MonoBehaviour
{
    public float mouseSensitivity = 2f;

    private GameObject robot;

    private float zdistance = 0.3f;

    void Start()
    {
        robot = GameObject.Find("BlueRov");
    }

    void Update()
    {
        if (Input.GetMouseButton(0)){
            transform.eulerAngles += mouseSensitivity * new Vector3(x:-Input.GetAxis("Mouse Y"),y:Input.GetAxis("Mouse X"),z:0);
        }
        
    }
    void LateUpdate()
    {
        transform.position = new Vector3(robot.transform.position.x, robot.transform.position.y + zdistance, robot.transform.position.z);
    }

}

