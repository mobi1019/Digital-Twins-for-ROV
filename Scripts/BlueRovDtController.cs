/* This scipt uses simple chnages in the transform of the AUV rather than applying actual force on the body. 
 * Therefore this script cannot be used regularly as this will not give updates about the body's velocity and accelaration.
 * This is also impractical as in the actual vehicle, the movement will be done by the thrusters applying forces at various points.
 * Therefore this script serves only as a stand-in when quick testing is needed. 
 */

using ROSBridgeLib;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BlueRovDtController : MonoBehaviour
{
    // public GameObject waterLevel;                   // GameObject assosicated with the water surface.  
    // public GameObject AUVLevel;                     // The AUV's GameObject.

    public bool initial_position = true;
    public float speed = 0.001F;

    void Update()
    {
        // The following lines of code are used to import control values coming from ROS,  subscribed by the Subscriber script
		float pos_Y = BlueRovPoseSubscriber.position.y;           
		float pos_Z = BlueRovPoseSubscriber.position.z;
		float pos_X = BlueRovPoseSubscriber.position.x;
		float rot_Z = BlueRovPoseSubscriber.rotation.z;
		float rot_Y = BlueRovPoseSubscriber.rotation.y;
		float rot_X = BlueRovPoseSubscriber.rotation.x;
		float w = BlueRovPoseSubscriber.rotation.w;
		
		Vector3 movement = new Vector3(pos_X, pos_Z, pos_Y);        // Make a vector for translation using the user input. 
		Quaternion rotation = new Quaternion(-1 * rot_Y, rot_Z, -1 * rot_X, w);       // Make a vector for rotation using the user input. 

      
		if (initial_position){
            initial_position = false;
            //transform.position = movement;
            //transform.rotation = rotation;
        }

        float step = speed * Time.deltaTime;
        transform.position = movement;//Vector3.MoveTowards(transform.position , movement, step);
        transform.rotation = rotation;//Quaternion.Lerp(transform.rotation, rotation, step);
        // Debug.Log(BlueRovPoseSubscriber.all);
    }

}