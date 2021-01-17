using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrigger : MonoBehaviour
{
    public Launch launch;
    public Move move;

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider c){
        // Debug.Log("launch triigger stay");
        launch.enableLaunch(c);
        move.feetOnGround();
    }
}
