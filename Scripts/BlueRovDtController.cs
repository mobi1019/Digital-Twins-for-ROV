// This controller subscribes to pose topics from ros
using ROSBridgeLib;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BlueRovDtController : MonoBehaviour
{

    public bool initial_position = true;
    public float speed = 0.1F;

    void Update()
    {
		float pos_Y = BlueRovPoseSubscriber.position.y;           
		float pos_Z = BlueRovPoseSubscriber.position.z;
		float pos_X = BlueRovPoseSubscriber.position.x;
		float rot_Z = BlueRovPoseSubscriber.rotation.z;
		float rot_Y = BlueRovPoseSubscriber.rotation.y;
		float rot_X = BlueRovPoseSubscriber.rotation.x;
		float w = BlueRovPoseSubscriber.rotation.w;
		
        // During testing different systems had different coordinates conversion
        // gazebo UUVSimulation
		// Vector3 movement = new Vector3(pos_X, pos_Z, pos_Y);      
		// Quaternion rotation = new Quaternion(-1 * rot_Y, rot_Z, -1 * rot_X, w);       

        //orb-slam3 with harbour dataset      
        Vector3 movement = new Vector3(pos_X, -pos_Z, -pos_Y);        
		Quaternion rotation = new Quaternion(-1 * rot_X, rot_Z, -1 * rot_Y, w);

		if (initial_position){
            initial_position = false;
            // transform.position = movement;
            // transform.rotation = rotation;
        }

        float step = speed * Time.deltaTime;
        // transform.position = movement;
        // transform.rotation = rotation;
        transform.position = Vector3.MoveTowards(transform.position , movement, step);
        transform.rotation = rotation;
    }

}