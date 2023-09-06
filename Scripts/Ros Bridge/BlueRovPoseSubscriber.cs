// pose subscriber script
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROSBridgeLib; 
using SimpleJSON;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.auv_msgs;
using ROSBridgeLib.nav_msgs; 


public class BlueRovPoseSubscriber : ROSBridgeSubscriber
{
    public static Vector3 position; 
    public static Quaternion rotation; 
                                        

    public static int i = 0;

    // canvas pose varaibles
    public static Text pose_on_canvas_x;
    public static Text pose_on_canvas_y;
    public static Text pose_on_canvas_z;
    public new static string GetMessageTopic() 
    {
        // return "/odometry"; // sparus rosbag topic
        // return "/bluerov2/pose_gt"; // uuv simulation topic
        // return "/out_topic"; // uuv simulation throttled topic
        return "/orb_slam3/camera_pose"; //orb_slam pose topic
        // return "/out_pose"; //orb_slam throttled pose topic
    }

    public new static string GetMessageType() 
    {
        // return "nav_msgs/Odometry"; //uuv simulation message type
        return "geometry_msgs/PoseStamped"; // orb slam message type
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        // return new OdometryMsg(msg); // uuv simulation topic
        return new PoseStampedMsg(msg); // orb slam message
    }

    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {
            
            // // for uuv simulation
            // OdometryMsg OdometryData = (OdometryMsg)msg; 
            // position.x = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetX();
            // position.y = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetY();
            // position.z = OdometryData.GetPoseWithCovariance().GetPose().GetPosition().GetZ();
            // rotation.x = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetX();
            // rotation.y = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetY();
            // rotation.z = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetZ();
            // rotation.w = OdometryData.GetPoseWithCovariance().GetPose().GetOrientation().GetW();
            
            // test for time / latency
            // long secs = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            // Debug.Log("Time after recieving image = " + secs);

            // // for orb slam
            PoseStampedMsg PoseStampedData = (PoseStampedMsg)msg; 
            position.x = PoseStampedData.GetPose().GetPosition().GetX();
            position.y = PoseStampedData.GetPose().GetPosition().GetY();
            position.z = PoseStampedData.GetPose().GetPosition().GetZ();
            rotation.x = PoseStampedData.GetPose().GetOrientation().GetX();
            rotation.y = PoseStampedData.GetPose().GetOrientation().GetY();
            rotation.z = PoseStampedData.GetPose().GetOrientation().GetZ();
            rotation.w = PoseStampedData.GetPose().GetOrientation().GetW();

            // place pose on canvas
            pose_on_canvas_x = GameObject.Find("Position-x").GetComponent<Text>();
            pose_on_canvas_y = GameObject.Find("Position-y").GetComponent<Text>();
            pose_on_canvas_z = GameObject.Find("Position-z").GetComponent<Text>();
            pose_on_canvas_x.GetComponent<Text>().text = "Position-x : " + position.x;
            pose_on_canvas_y.GetComponent<Text>().text = "Position-y : " + position.y;
            pose_on_canvas_z.GetComponent<Text>().text = "Position-z : " + position.z;
        
    }
}

