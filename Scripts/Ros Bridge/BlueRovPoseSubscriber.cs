// This script serves a subscriber for Pose type message and can be edited according to the user's need. Here, it receives 
// control commands and button functions which are then called by their respective scripts. 
//Called by RosInitializer, therefor doesn't need to be attached to an object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROSBridgeLib; // Calling the Rosbridge library
using SimpleJSON;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.auv_msgs;
using ROSBridgeLib.nav_msgs; // Calling RosBridge message types that come under geometry_ msgs (So that we can use Pose message type)


// Ball subscriber:
public class BlueRovPoseSubscriber : ROSBridgeSubscriber
{
    public static Vector3 position; // A vector3 that will store translation vectors
    public static Quaternion rotation; // A Quaternion which will store rotation vectors for roll, pitch and yaw in the first three
                                        // values and button functions in the last value

    public static NEDMsg result;

    public static string dad;
   
    public new static string GetMessageTopic() // To get the topic name
    {
        return "/odometry"; // Define the topic's name
    }

    public new static string GetMessageType() //To get the topic type
    {
        return "nav_msgs/Odometry"; // Defining the topic type
    }

    // This function converts JSon to Pose Message
    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new OdometryMsg(msg);
    }

    // This function should fire on each received ROS message
    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {

        // Debug.Log("Recieved Message : " + msg.ToYAMLString()); // Prints the recieved message

        OdometryMsg OdometryData = (OdometryMsg)msg; 

        position.x = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetX();
        position.y = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetY();
        position.z = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetZ();
        
        rotation.x = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetX();
        rotation.y = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetY();
        rotation.z = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetZ();
        rotation.w = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetW();
    }
}