using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    // Start is called before the first frame update
    public float damagePerSecond = 10;

    void OnTriggerStay(Collider obj){
        obj.gameObject.SendMessage("addHealth", -damagePerSecond*Time.fixedDeltaTime);
    }  
}
