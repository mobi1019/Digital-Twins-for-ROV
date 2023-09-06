// point cloud subscriber script 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROSBridgeLib; 
using SimpleJSON;

using ROSBridgeLib.sensor_msgs; 
using ROSBridgeLib.std_msgs; 
// using PointCloud;

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


    public new static string GetMessageTopic() 
    {
        // return "/orb_slam3/all_points"; // orb slam pt cloud topic 
        return "/dense_cloud"; // dense pt cloud node topic from ROS
    }

    public new static string GetMessageType() 
    {
        return "sensor_msgs/PointCloud2"; 
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new PointCloud2Msg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {
       
        PointCloud2Msg ptClouds = (PointCloud2Msg)msg;

        // test for time/latency
        // long secs = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        // Debug.Log("Time after drawing point clouds in milliseconds = " + secs);

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
       
        for (int n = 0; n < size; n++)
        {
            // only pt cloud x,y,z cordinates sent, not colour
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
