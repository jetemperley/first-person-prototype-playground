using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvContainer : MonoBehaviour {

    public int sizeX = 5, sizeY = 5;
    public string name = "bagName";
    public bool isBackPack = false;
    public Bag b;

    void Start () {
        b = new Bag (sizeX, sizeY);
    }

}