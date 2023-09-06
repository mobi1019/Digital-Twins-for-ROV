// pressure subscriber script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ROSBridgeLib; 
using ROSBridgeLib.sensor_msgs; 
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

    public new static void CallBack(ROSBridgeMsg msg) 
    {
        FluidPressureMsg pressureData = (FluidPressureMsg)msg;
        pres = pressureData.GetFluidPressure();
        pressure_on_canvas = GameObject.Find("Pressure").GetComponent<Text>();
        pressure_on_canvas.GetComponent<Text>().text = "Pressure :" + (int)(pres);
       
    }
  
}


