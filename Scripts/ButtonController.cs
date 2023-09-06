// this controller loads the simultion scene from the DT interface
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour
{
    public void SimulationScene(){
        SceneManager.LoadScene(6);
    }

}