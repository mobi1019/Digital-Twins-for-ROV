// test controller for sending timed message (this is a test script)
using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using UnityEngine;
using UnityEngine.UI;

public class LatencyController : MonoBehaviour
{
    public GameObject rosObj;
    public static Int64Msg stamp;
    public static long secs;
    public static int nsecs;
    void Start()
    {
        rosObj = GameObject.Find("BlueRov");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)){
            secs = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            
            nsecs = 0;
            
            stamp = new Int64Msg (secs); // Defining the message that needs to be published 
            // Debug.Log(stamp);

            rosObj.GetComponent<ROSInitializer>().ros.Publish(LatencyPublisher.GetMessageTopic(), stamp);
        }


    }
}
