using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    
    public CapsuleCollider cc;
    public Camera cam;
    GameObject g;
    bool crouched = false;
    Vector3 v;

    void Start()
    {
        v = new Vector3(0, 0.6f, 0);
        g = cam.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            if (crouched){
                uncrouch();
            } else {
                crouch();
            }
        }
    }

    void crouch(){
        cc.height = 1;
        g.transform.localPosition = g.transform.localPosition -v;
        crouched = true;
    }

    void uncrouch(){
        cc.height = 2;
        g.transform.localPosition = g.transform.localPosition +v;
        crouched = false;
    }
}
