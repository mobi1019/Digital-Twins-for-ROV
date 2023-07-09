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
		float sway = BlueRovPoseSubscriber.position.y;           
		float heave = BlueRovPoseSubscriber.position.z;
		float surge = BlueRovPoseSubscriber.position.x;
		float yaw = BlueRovPoseSubscriber.rotation.z;
		float roll = BlueRovPoseSubscriber.rotation.y;
		float pitch = BlueRovPoseSubscriber.rotation.x;
		float w = BlueRovPoseSubscriber.rotation.w;
		
		Vector3 movement = new Vector3(-surge, heave, sway);        // Make a vector for translation using the user input. 
		Quaternion rotation = new Quaternion(roll, yaw, -1 * pitch, w);       // Make a vector for rotation using the user input. 
																	 // Add relative force, i.e. force in the local coordinate system, using the Vector3 created by user Input. 
		if (initial_position){
            initial_position = false;
            transform.position = movement;
            transform.rotation = rotation;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position , movement, step);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, step);
        // Debug.Log(BlueRovPoseSubscriber.all);
    }

}