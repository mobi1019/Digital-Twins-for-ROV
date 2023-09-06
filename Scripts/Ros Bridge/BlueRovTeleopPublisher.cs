// teleop publisher script
using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib; 
using ROSBridgeLib.geometry_msgs; 
using SimpleJSON;

public class BlueRovTeleopPublisher:ROSBridgePublisher {
    
    public static string GetMessageTopic() {
        return "/bluerov2/thruster_manager/input"; // uuv simulation thruster manager topic
    }  
    
    public static string GetMessageType() {
        return "geometry_msgs/Wrench";
    }

    public static string ToYAMLString(WrenchMsg msg) {
        return msg.ToYAMLString();
    }
    public new static ROSBridgeMsg ParseMessage(JSONNode msg) {
        return new WrenchMsg(msg);
  } 
  
}


