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
        set { _param_value = Mathf.Clamp(value, -500f, 500f); }
    }

    public static string displayText;

    public static double xf,yf,zf,xt,yt,zt;
    private float increment = 20.0f;
    private static float _param_value;
    private static Vector3Msg force;
    private static Vector3Msg torque;
    private static WrenchMsg wrench;

    public static Text speed_on_canvas;
 
    void Start()
    {
        rosObj = GameObject.Find("BlueRov");
        param_value = 0;
        xt = 0;
        yt = 0;
        zt = 0;
        xf = 0;
        yf = 0;
        zf = 0;
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



        if (Input.GetKeyDown(KeyCode.RightArrow)){
            ytorque = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            xtorque = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            ztorque = true;
        }
        // 

        if (Input.GetKeyDown(KeyCode.A)){
            xforce = true;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            yforce = true;
        }
        if (Input.GetKeyDown(KeyCode.W)){
            zforce = true;
        }
        // 

        if (Input.GetKeyUp(KeyCode.Q)){
            increaseparam = true;
            Increment(increaseparam);
        }
        if (Input.GetKeyUp(KeyCode.E)){
            decreaseparam = true;
            Decrement(decreaseparam);
        }

        speed_on_canvas = GameObject.Find("Speed").GetComponent<Text>();
        speed_on_canvas.GetComponent<Text>().text = "Thrust Speed : " + displayText ;
       
        if ( increaseparam|| decreaseparam || zforce || yforce || xforce || ztorque || ytorque || xtorque){
            double[] valuesF = new double[3];
            double[] valuesT = new double[3];
            if (param_value == 0.0f){
                valuesF = GetForce(true, true, true);

                valuesT = GetTorque(true, true, true);
            } else {
                valuesF = GetForce(zforce, yforce, xforce);

                valuesT = GetTorque(ztorque, ytorque, xtorque);
            }


            force = new Vector3Msg(
                valuesF[0], valuesF[1], valuesF[2]
            );

            torque = new Vector3Msg(
                valuesT[0], valuesT[1], valuesT[2]
            );

            wrench = new WrenchMsg (force, torque); // Defining the message that needs to be published 
            Debug.Log(wrench);
            rosObj.GetComponent<ROSInitializer>().ros.Publish(BlueRovTeleopPublisher.GetMessageTopic(), wrench);// Calling the publisher script and publishing the message
        }       
    }

    public void Increment(bool increaseparam){
        if (increaseparam){
            Debug.Log(param_value);
            param_value += increment;
            displayText = param_value.ToString();
        }
        Debug.Log(param_value);
    }

    public void Decrement(bool decreaseparam){
        if (decreaseparam){
            param_value -= increment;
            displayText = param_value.ToString();
        }
        Debug.Log(param_value);
    }

    public double[] GetForce(bool zforce,bool yforce,bool xforce){
        if (zforce){
            zf = (double)(param_value);
        } 
        if (yforce){
            yf = (double)(param_value);
        } 
        if (xforce){
            xf = (double)(param_value);
        } 
        double[] result = {xf,yf,zf};
        return result;
    }

    public double[] GetTorque(bool ztorque,bool ytorque,bool xtorque){
        if (ztorque){
            zt = (double)(param_value);
        } 
        if (ytorque){
            yt = (double)(param_value);
        } 
        if (xtorque){
            xt = (double)(param_value);
        } 
        double[] result = {xt,yt,zt};
        return result;  
    }
    // public void OnGUI()
    // {
    //     GUI.contentColor = Color.black;
    //     GUI.Label(new Rect(0,0,250,250), displayText);
    // }
    

}
