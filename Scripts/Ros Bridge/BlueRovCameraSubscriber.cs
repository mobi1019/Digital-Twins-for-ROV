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


// Ball subscriber:
public class BlueRovCameraSubscriber : ROSBridgeSubscriber
{
    public static byte[] data; 

    
    public new static string GetMessageTopic() // To get the topic name
    {
        return "/camera/image_raw"; // Define the topic's name
    }

    public new static string GetMessageType() //To get the topic type
    {
        return "sensor_msgs/Image"; // Defining the topic type
    }

    // This function converts JSon to Pose Message
    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ImageMsg(msg);
    }

    // This function should fire on each received ROS message
    public new static void CallBack(ROSBridgeMsg msg) //msg is the recieved message
    {

        ImageMsg image = (ImageMsg) msg;
        data = image.GetImage();
        int imageHeight = (int)image.GetHeight();
        int imageWidth = (int)image.GetWidth();
        int step = (int)image.GetRowStep();
        Texture2D texROS = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
       
        Color[] colors = new Color [imageWidth * imageHeight];
        for (int i = 0; i < imageWidth; i++)
        {
            for (int j = 0; j < imageHeight; j++)
            {
                byte B = data[j * step + i * 3];
                byte G = data[j * step + i * 3 + 1];
                byte R = data[j * step + i * 3 + 2];
                Color c = new Color((float)R / 255.0f, (float)G / 255.0f, (float)B / 255.0f);
                colors[j * imageWidth + i] = c;
                //tex.SetPixel(i, height - j, c);
            }
        }
        texROS.SetPixels(colors);
        texROS.Apply();
        GameObject cam_image = GameObject.Find("canvasImage");
        cam_image.GetComponent<RawImage>().texture = texROS;
    }
}