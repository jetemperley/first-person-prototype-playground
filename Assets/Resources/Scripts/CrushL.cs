using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushL : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider obj){
        obj.gameObject.SendMessage("crushL", true);
    }

    void OnTriggerExit(Collider obj){
        obj.gameObject.SendMessage("crushL", false);
    }
}
