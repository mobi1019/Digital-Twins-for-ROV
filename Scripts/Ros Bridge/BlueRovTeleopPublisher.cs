// A compressed Image publisher for forward-facing camera. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib; // Calling the Rosbridge library
using ROSBridgeLib.geometry_msgs; // Calling RosBridge message types that come under sensor_ msgs (So that we can use Compressed Image message type)
using SimpleJSON;

public class BlueRovTeleopPublisher:ROSBridgePublisher {
    
    public static string GetMessageTopic() {
        return "/bluerov2/thruster_manager/input";
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


