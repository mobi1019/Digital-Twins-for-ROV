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

    void Update()
    {
        // if (BlueRovPoseSubscriber.position.x > 0 || BlueRovPoseSubscriber.position.x < 0){
        //     Debug.Log(BlueRovPoseSubscriber.position.x); 
        // }  
        // Debug.Log(BlueRovPoseSubscriber.position.y);   
        // Debug.Log(BlueRovPoseSubscriber.position.z);   
        Debug.Log(BallPoseSubscriber.position.z);   
        Debug.Log(BlueRovPoseSubscriber.position.x);   
        // Debug.Log(BlueRovPoseSubscriber.dad);   

    }

}