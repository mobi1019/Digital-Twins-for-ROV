// path planning publisher script
using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib; 
using ROSBridgeLib.geometry_msgs; 
using SimpleJSON;

public class BlueRovPathPlanningPublisher:ROSBridgePublisher {
    
    public static string GetMessageTopic() {
        return "/path_planner"; // uuv simulation thruster manager topic
    }  
    
    public static string GetMessageType() {
        return "geometry_msgs/Pose";
    }

    public static string ToYAMLString(PoseMsg msg) {
        return msg.ToYAMLString();
    }
    public new static ROSBridgeMsg ParseMessage(JSONNode msg) {
        return new PoseMsg(msg);
  } 
  
}
