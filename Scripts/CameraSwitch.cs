using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    private GameObject Cam1;
    private GameObject Cam2;
    private GameObject Cam3;

    void Start()
    {
        // lock and hide Cursor
        Cam1 = GameObject.Find("Cam1");
        Cam2 = GameObject.Find("Cam2");
        // Cam3 = GameObject.Find("Cam3");
        Cam1.SetActive(true);
        Cam2.SetActive(false);
        // Cam3.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown ("1")){
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            // Cam3.SetActive(false);
       }
       if (Input.GetKeyDown ("2")){
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            // Cam3.SetActive(false);
       }
    //     if (Input.GetKeyDown ("3")){
    //         Cam1.SetActive(false);
    //         Cam2.SetActive(false);
    //         Cam3.SetActive(true);
    //    }
    }


}