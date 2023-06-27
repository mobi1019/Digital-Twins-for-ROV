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
        // // Update ball position, or whatever
        OdometryMsg OdometryData = (OdometryMsg)msg; // This converts the message to Pose type so that it can be used.
        // //ball = GameObject.Find("ball");
        // //Vector3 ballPos = ball.transform.position;
        position.x = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetX();
        dad = "you";
        // position.x = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetX(); // Vector3 position and Quaternion rotation are assigned values from the recieved message.
        // position.y = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetY();
        // position.z = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetZ();
        
        // position.y = OdometryData.GetPosition().GetY();
        // position.z = OdometryData.GetPosition().GetZ();
        // rotation.x = OdometryData.GetOrientation().GetX();
        // rotation.y = OdometryData.GetOrientation().GetY();
        // rotation.z = OdometryData.GetOrientation().GetZ();
        // rotation.w = OdometryData.GetOrientation().GetW();
        //ballPos.y = PoseData.GetPosition().GetY();
        //ballPos.z = PoseData.GetPosition().GetZ();
        //Changing ball's position to the updated position vector
        //ball.transform.position = ballPos;
    }
}