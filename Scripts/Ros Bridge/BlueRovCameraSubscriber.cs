// camera subscriber script for canvas element
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROSBridgeLib; 
using SimpleJSON;
using ROSBridgeLib.sensor_msgs; 

public class BlueRovCameraSubscriber : ROSBridgeSubscriber
{
    public static byte[] data; 

    
    public new static string GetMessageTopic() 
    {
        return "/camera/image_raw"; // orb slam topic
        // return "/bluerov2/camera_front/camera_image"; // uuv simulator bluerov topic
  
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/Image"; 
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ImageMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ImageMsg image = (ImageMsg) msg;
        data = image.GetImage();
        int imageHeight = (int)image.GetHeight();
        int imageWidth = (int)image.GetWidth();
        Texture2D imageTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.Alpha8, false);

        imageTexture.LoadRawTextureData(data);
        imageTexture.Apply();
        GameObject cam_image = GameObject.Find("canvasImage");
        cam_image.GetComponent<RawImage>().texture = imageTexture;
    }
}