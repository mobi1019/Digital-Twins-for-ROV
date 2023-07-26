// This script serves a subscriber for Pose type message and can be edited according to the user's need. Here, it receives 
// control commands and button functions which are then called by their respective scripts. 
//Called by RosInitializer, therefor doesn't need to be attached to an object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROSBridgeLib; // Calling the Rosbridge library
using SimpleJSON;

using ROSBridgeLib.sensor_msgs; // Calling RosBridge message types that come under geometry_ msgs (So that we can use Pose message type)
using ROSBridgeLib.std_msgs; // Calling RosBridge message types that come under geometry_ msgs (So that we can use Pose message type)
// using PointCloud;

// Ball subscriber:
public class BlueRovCloudSubscriber : ROSBridgeSubscriber
{

    public static uint height;
    public static uint width;

    public static byte[] data;

    public static uint row_step;
    public static uint point_Step;
    
    public static byte[] byteArray;
    public static int size;

    public static Vector3[] pcl;

    public static Color[] pcl_color;

    // public static float searchRadius = 0.1f;


    public new static string GetMessageTopic() // To get the topic name
    {
        return "/orb_slam3/all_points"; // Define the topic's name
        // return "/dense_cloud"; // Define the topic's name
    }

    public new static string GetMessageType() //To get the topic type
    {
        return "sensor_msgs/PointCloud2"; // Defining the topic type
    }

    // This function converts JSon to Pose Message
    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new PointCloud2Msg(msg);
    }

    // This function should fire on each received ROS message
    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {
       
        PointCloud2Msg ptClouds = (PointCloud2Msg)msg;
        size = ptClouds.GetData().GetLength(0);
        byteArray = new byte[size];
        byteArray = ptClouds.GetData();
        size = (int)(size / ptClouds.GetPointStep());
        pcl = new Vector3[size];
        pcl_color = new Color[size];

        int x_posi;
        int y_posi;
        int z_posi;

        float x;
        float y;
        float z;

        int rgb_posi;
        int rgb_max = 255;

        float r;
        float g;
        float b;

        //この部分でbyte型をfloatに変換         
        for (int n = 0; n < size; n++)
        {
            x_posi = (int)(n *ptClouds.GetPointStep() + 0);
            y_posi = (int)(n *ptClouds.GetPointStep() + 4);
            z_posi = (int)(n *ptClouds.GetPointStep() + 8);

            x = System.BitConverter.ToSingle(byteArray, x_posi);
            y = System.BitConverter.ToSingle(byteArray, y_posi);
            z = System.BitConverter.ToSingle(byteArray, z_posi);


            // rgb_posi = (int)(n * ptClouds.GetPointStep() + 16);]
            // b = byteArray[rgb_posi + 0];
            // g = byteArray[rgb_posi + 1];
            // r = byteArray[rgb_posi + 2];

            // r = r / rgb_max;
            // g = g / rgb_max;
            // b = b / rgb_max;

            pcl[n] = new Vector3(x, z, y);
            // pcl_color[n] = new Color(r, g, b);
        }
    }  
}
