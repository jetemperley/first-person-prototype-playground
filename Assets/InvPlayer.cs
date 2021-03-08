using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attaches to a player that can hold and pickup inventory items
public class InvPlayer : MonoBehaviour {

    public GameObject handsItemL = null;
    public GameObject handsItemR = null;
    public GameObject handsAnchorL;
    public GameObject handsAnchorR;
    public float scaleFactor = 1f;
    public GameObject backPack = null, backPackAnchor;

    // Start is called before the first frame update
    void Start () {
        backPack = null;
    }

    // Update is called once per frame
    void Update () {

    }

    public bool addToHandsL (GameObject g) {
        if (handsItemL == null) {
            //Debug.Log("not null");
            handsItemL = g;
            addToHand(handsAnchorL, handsItemL);
            return true;
        }
        return false;
    }

    public bool addToHandsR (GameObject g) {
        if (handsItemR == null) {
            //Debug.Log("not null");
            handsItemR = g;

            addToHand (handsAnchorR, handsItemR);
            return true;
        }
        return false;
    }

    void addToHand (GameObject handAnchor, GameObject handItem) {
        handItem.layer = 11;

        // Rigidbody rb = handItem.GetComponent<Rigidbody>();
        // Rigidbody rbh = handAnchor.GetComponent<Rigidbody>();
        // rbh.mass = rbh.mass + rb.mass;
        // Destroy(rb, 0.001f);
        // handItem.transform.position = handAnchor.transform.position;
        // handItem.transform.rotation = handAnchor.transform.rotation;
        
        // handItem.transform.parent = handAnchor.transform;
        
        


        // rb.freezeRotation = true;
        // handItem.transform.localRotation =  handItem.transform.localRotation*Quaternion.Euler(45, 0, 0);

        // handsItemR.GetComponent<Rigidbody> ().isKinematic = true;
        //handsItemR.GetComponent<Collider> ().enabled = false;

        // move and parent the object to the hand
        
        // handItem.transform.localRotation = handAnchor.transform.localRotation;
        // handItem.transform.position = handsAnchorR.transform.position;
        // handItem.transform.localPosition = handItem.transform.localPosition + new Vector3 (0, 0, 0.1f);
        // handItem.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);

        // ConfigurableJoint fj = handItem.AddComponent<ConfigurableJoint>();
        // JointDrive jd = fj.angularXDrive;
        // jd.positionSpring = 0;
        // jd.positionDamper = 0;
        // fj.angularXDrive = jd;
        // fj.angularYZDrive = jd;
        // ConfigurableJoint fj = handItem.AddComponent<ConfigurableJoint> ();
        // fj.connectedBody = handAnchor.GetComponent<Rigidbody>();
        
        // fj.xMotion = ConfigurableJointMotion.Locked;
        // fj.yMotion = ConfigurableJointMotion.Locked;
        // fj.zMotion = ConfigurableJointMotion.Locked;
        // fj.angularXMotion = ConfigurableJointMotion.Locked;
        // fj.angularYMotion = ConfigurableJointMotion.Locked;
        // fj.angularZMotion = ConfigurableJointMotion.Locked;

        // move the hand item rigidbody to the hand and add a fixed joint??? no parenting?
        handItem.transform.position = handAnchor.transform.position;
        handItem.transform.rotation = gameObject.transform.rotation;
        FixedJoint fj = handItem.AddComponent<FixedJoint>();
        // fj.connectedBody = handAnchor.GetComponent<Rigidbody>();
        fj.connectedBody = gameObject.GetComponent<Rigidbody>();
        fj.massScale = handItem.GetComponent<Rigidbody> ().mass;


        

    }

    public GameObject removeFromHandsL () {
        GameObject g = removeFromHand(handsItemL, handsAnchorL);
        handsItemL = null;
        return  g;
    }

    public GameObject removeFromHandsR () {
        GameObject g = removeFromHand(handsItemR, handsAnchorR);
        handsItemR = null;
        return  g;
    }

    GameObject removeFromHand(GameObject handItem, GameObject hand){
        // handItem.GetComponent<Rigidbody> ().isKinematic = false;
        handItem.GetComponent<Collider> ().enabled = true;
        handItem.transform.parent = null;

        // Rigidbody rb = handItem.AddComponent<Rigidbody>();
        // rb.freezeRotation = false;
        // rb.useGravity = true;
        // Rigidbody rbh = hand.GetComponent<Rigidbody>();
        // rb.mass = rbh.mass-1;
        // rbh.mass = 1;

        handItem.transform.localScale = new Vector3 (1, 1, 1);
        GameObject g = handItem;
        handItem = null;
        FixedJoint fj = g.GetComponent<FixedJoint> ();
        if (fj != null)
            Destroy (fj, 0.05f);

        g.layer = 1;
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

    public bool canFitBag (GameObject g) {
        InvContainer ic = g.GetComponent<InvContainer> ();
        return ic != null && ic.isBackPack && backPack == null;
    }
}