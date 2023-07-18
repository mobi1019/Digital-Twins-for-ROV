// A compressed Image publisher for forward-facing camera. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ROSBridgeLib; // Calling the Rosbridge library
using ROSBridgeLib.sensor_msgs; // Calling RosBridge message types that come under sensor_ msgs (So that we can use Compressed Image message type)
using SimpleJSON;

public class BlueRovPressureSubscriber:ROSBridgePublisher {

    public static Text pressure_on_canvas;
    public static double pres;
    public static string GetMessageTopic() {
        return "/bluerov2/pressure";
    }  
    
    public static string GetMessageType() {
        return "sensor_msgs/FluidPressure";
    }

     public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new FluidPressureMsg(msg);
    }

    // This function should fire on each received ROS message
    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {
        FluidPressureMsg pressureData = (FluidPressureMsg)msg;
        pres = pressureData.GetFluidPressure();
        pressure_on_canvas = GameObject.Find("Pressure").GetComponent<Text>();
        pressure_on_canvas.GetComponent<Text>().text = "Pressure :" + (int)(pres);
       
    }
  
}


