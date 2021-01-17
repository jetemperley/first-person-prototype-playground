using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attaches to a player that can hold and pickup inventory items
public class InvPlayer : MonoBehaviour {


    public GameObject handsItem = null;
    public GameObject handsAnchor;
    public float scaleFactor = 1f;
    public GameObject backPack = null, backPackAnchor;

    // Start is called before the first frame update
    void Start () {
        backPack = null;
    }

    // Update is called once per frame
    void Update () {

    }

    public bool addToHands (GameObject g) {
        if (handsItem == null) {
            //Debug.Log("not null");
            handsItem = g;
            handsItem.GetComponent<Rigidbody> ().isKinematic = true;
            handsItem.GetComponent<Collider> ().enabled = false;
            handsItem.transform.parent = handsAnchor.transform;

            handsItem.transform.localRotation = handsAnchor.transform.localRotation;
            handsItem.transform.position = handsAnchor.transform.position;
            handsItem.transform.localPosition = handsItem.transform.localPosition + new Vector3 (0, 0, 1f);

            handsItem.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
            return true;
        }
        return false;
    }

    public GameObject removeFromHands () {
        handsItem.GetComponent<Rigidbody> ().isKinematic = false;
        handsItem.GetComponent<Collider> ().enabled = true;
        handsItem.transform.parent = null;

        handsItem.transform.localScale = new Vector3 (1, 1, 1);
        GameObject g = handsItem;
        handsItem = null;
        return g;
    }

    public bool addBackPack (GameObject bag) {
        InvContainer b = bag.GetComponent<InvContainer> ();
        if (b != null && b.isBackPack) {
            backPack = bag;
            backPack.GetComponent<Rigidbody> ().isKinematic = true;
            backPack.GetComponent<Collider> ().enabled = false;
            GameObject anchor = backPackAnchor;
            backPack.transform.SetParent (anchor.transform);

            backPack.transform.localRotation = anchor.transform.localRotation;
            backPack.transform.position = anchor.transform.position;
            backPack.transform.localPosition = new Vector3 (0, 0, 0);

            return true;
        }
        return false;
    }

    public GameObject removeBackPack () {
        if (backPack != null) {
            backPack.GetComponent<Rigidbody> ().isKinematic = false;
            backPack.GetComponent<Collider> ().enabled = true;
            backPack.transform.parent = null;

            backPack.transform.localScale = new Vector3 (1, 1, 1);
            GameObject g = backPack;
            backPack = null;
            return g;
        }
        return null;
    }

    public bool canFitBag(GameObject g){
        InvContainer ic = g.GetComponent<InvContainer>();
        return ic != null && ic.isBackPack && backPack == null;
    }
}