using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushR : MonoBehaviour {
    void OnTriggerEnter (Collider obj) {
        obj.gameObject.SendMessage ("crushR", true);
    }

    void OnTriggerExit (Collider obj) {
        obj.gameObject.SendMessage ("crushR", false);
    }

}