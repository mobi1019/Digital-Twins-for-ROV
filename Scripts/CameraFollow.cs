// this controller handles viewpoint 1
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 2f;

    private GameObject robot;

    private float zdistance = 3;

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
        transform.position = new Vector3(robot.transform.position.x, (robot.transform.position.y), (robot.transform.position.z - zdistance));
    }

}

