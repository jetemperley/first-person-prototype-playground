using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootReciever : MonoBehaviour
{
    public Launch launch;
    void OnTriggerStay(){
        // Debug.Log("feet on ground");
        launch.feetOnGround();
    }
}
