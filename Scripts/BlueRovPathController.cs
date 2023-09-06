// This controller processes teleop messages
using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using UnityEngine;
using UnityEngine.UI;

public class BlueRovPathController : MonoBehaviour
{
    public GameObject rosObj;
    public static PoseMsg pose;
    public static PointMsg point;
    public static QuaternionMsg orientation;
    void Start()
    {
        rosObj = GameObject.Find("BlueRov");
  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            float x = -0.5f;
            float y = 4.0f;
            float z = -0.5f;

            float o_x = 1.0f;
            float o_y = 1.0f;
            float o_z = 1.0f;
            float o_w = 1.0f;

            point = new PointMsg(x, y, z); 
            orientation = new QuaternionMsg(o_x, o_y, o_z, o_w);
            pose = new PoseMsg (point, orientation); 
            Debug.Log(pose);
            rosObj.GetComponent<ROSInitializer>().ros.Publish(BlueRovPathPlanningPublisher.GetMessageTopic(), pose);// Calling the publisher script and publishing the message
        }

    }
    

}
