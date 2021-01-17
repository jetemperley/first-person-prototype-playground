using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnDusk : MonoBehaviour
{
    // Start is called before the first frame update
    Animator[] doorAnim; 
    void Start(){
        float rot = transform.eulerAngles.z;
        bool open = rot < 180;
        GameObject[] door = GameObject.FindGameObjectsWithTag("DawnDusk");
        doorAnim = new Animator[door.Length];
        
        for (int i = 0; i < door.Length; i++){
            doorAnim[i] = door[i].GetComponent<Animator>();
            doorAnim[i].SetBool("open", open);
        }
        Debug.Log("found " + doorAnim.Length +" doors");
        
    }

    public void setDoors(int open){

        GameObject[] door = GameObject.FindGameObjectsWithTag("DawnDusk");
        doorAnim = new Animator[door.Length];
        
        for (int i = 0; i < door.Length; i++){
            doorAnim[i] = door[i].GetComponent<Animator>();
            doorAnim[i].SetBool("open", open == 0);
        }

    }
}
