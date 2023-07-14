using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using UnityEngine;
using UnityEngine.UI;

public class BlueRovTeleOpController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rosObj;
    public static float param_value
    {
        get { return _param_value; }
        set { _param_value = Mathf.Clamp(param_value, -20f, 100f); }
    }
    private float increment = 20.0f;
    private static float _param_value;
    private static Vector3Msg force;
    private static Vector3Msg torque;
    private static WrenchMsg wrench;
 
    void Start()
    {
        rosObj = GameObject.Find("BlueRov");
        OnGUI();
    }

    // Update is called once per frame
    void Update()
    {
        bool zforce = false;
        bool yforce = false;
        bool xforce = false;
        bool ztorque = false;
        bool ytorque = false;
        bool xtorque = false;
        bool increaseparam  = false;
        bool decreaseparam = false;



        if (Input.GetKey(KeyCode.RightArrow)){
            ytorque = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            xtorque = true;
        }
        if (Input.GetKey(KeyCode.UpArrow)){
            ztorque = true;
        }
        // 

        if (Input.GetKey(KeyCode.A)){
            xforce = true;
        }
        if (Input.GetKey(KeyCode.D)){
            yforce = true;
        }
        if (Input.GetKey(KeyCode.W)){
            zforce = true;
        }
        // 

        if (Input.GetKey(KeyCode.Q)){
            increaseparam = true;
        }
        if (Input.GetKey(KeyCode.E)){
            decreaseparam = true;
        }
       
        Increment(increaseparam);
        Decrement(decreaseparam);

        double[] valuesF = GetForce(zforce, yforce, xforce);

        double[] valuesT = GetTorque(ztorque, ytorque, xtorque);

        force = new Vector3Msg(
           valuesF[0], valuesF[1], valuesF[2]
        );

        torque = new Vector3Msg(
            valuesT[0], valuesT[1], valuesT[2]
        );

        Debug.Log(force);
        Debug.Log(torque);

        wrench = new WrenchMsg (force, torque); // Defining the message that needs to be published 
        Debug.Log(wrench);
        rosObj.GetComponent<ROSInitializer>().ros.Publish(BlueRovTeleopPublisher.GetMessageTopic(), wrench);// Calling the publisher script and publishing the message
    }

    public void Increment(bool increaseparam){
        Debug.Log(increaseparam);
        if (increaseparam){
            param_value += increment;
        }
    }

    public void Decrement(bool decreaseparam){
        Debug.Log(decreaseparam);
        if (decreaseparam){
            param_value -= increment;
        }
    }

    public double[] GetForce(bool zforce,bool yforce,bool xforce){
        Debug.Log(zforce);
        Debug.Log(yforce);
        Debug.Log(xforce);
        double z = (double)(param_value);
        double x = (double)(param_value);
        double y = (double)(param_value);
        if (zforce){
            z = (double)(param_value);
        } 
        if (yforce){
            y = (double)(param_value);
        } 
        if (xforce){
            x = (double)(param_value);
        } 
        double[] result = {x,y,z};
        Debug.Log(result);
        return result;
    }

    public double[] GetTorque(bool ztorque,bool ytorque,bool xtorque){
        double z = (double)(param_value);
        double x = (double)(param_value);
        double y = (double)(param_value);;
        if (ztorque){
            z = (double)(param_value);
        } 
        if (ytorque){
            y = (double)(param_value);
        } 
        if (xtorque){
            x = (double)(param_value);
        } 
        double[] result = {x,y,z};
        return result;  
    }
    public void OnGUI()
    {
    //   GUI.Label(Rect(0,0,100,100),param_value);
    }
    

}
