// latency publisher script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib; 
using ROSBridgeLib.geometry_msgs; 
using ROSBridgeLib.std_msgs; 
using SimpleJSON;

public class LatencyPublisher:ROSBridgePublisher {
    
    public static string GetMessageTopic() {
        return "/LatencyTest";
    }  
    
    public static string GetMessageType() {
        return "std_msgs/Int64";
    }

    public static string ToYAMLString(Int64Msg msg) {
        return msg.ToYAMLString();
    }
    public new static ROSBridgeMsg ParseMessage(JSONNode msg) {
        return new Int64Msg(msg);
  } 
  
}


