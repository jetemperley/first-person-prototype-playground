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
            // handsItemL.GetComponent<Rigidbody> ().isKinematic = true;
            //handsItemL.GetComponent<Collider> ().enabled = false;
            handsItemL.transform.parent = handsAnchorL.transform;

            handsItemL.transform.localRotation = handsAnchorL.transform.localRotation;
            handsItemL.transform.position = handsAnchorL.transform.position;
            handsItemL.transform.localPosition = handsItemL.transform.localPosition + new Vector3 (0, 0, 0.1f);

            handsItemL.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
            // ConfigurableJoint fj = g.AddComponent<ConfigurableJoint>();
            // fj.xMotion = ConfigurableJointMotion.Locked;
            // fj.yMotion = ConfigurableJointMotion.Locked;
            // fj.zMotion = ConfigurableJointMotion.Locked;
            // JointDrive jd = fj.angularXDrive;
            // jd.positionSpring = 20f;
            // jd.positionDamper = -10f;
            // fj.angularXDrive = jd;
            // fj.angularYZDrive = jd;
            FixedJoint fj = g.AddComponent<FixedJoint>();
            fj.connectedBody = handsAnchorL.GetComponent<Rigidbody>();
            g.layer = 11;;
            return true;
        }
        return false;
    }

    public bool addToHandsR (GameObject g) {
        if (handsItemR == null) {
            //Debug.Log("not null");
            handsItemR = g;
            // handsItemR.GetComponent<Rigidbody> ().isKinematic = true;
            //handsItemR.GetComponent<Collider> ().enabled = false;
            handsItemR.transform.parent = handsAnchorR.transform;

            handsItemR.transform.localRotation = handsAnchorR.transform.localRotation;
            handsItemR.transform.position = handsAnchorR.transform.position;
            handsItemR.transform.localPosition = handsItemR.transform.localPosition + new Vector3 (0, 0, 0.1f);

            handsItemR.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
            // ConfigurableJoint fj = g.AddComponent<ConfigurableJoint>();
            // fj.xMotion = ConfigurableJointMotion.Locked;
            // fj.yMotion = ConfigurableJointMotion.Locked;
            // fj.zMotion = ConfigurableJointMotion.Locked;
            // JointDrive jd = fj.angularXDrive;
            // jd.positionSpring = 20f;
            // jd.positionDamper = -10f;
            // fj.angularXDrive = jd;
            // fj.angularYZDrive = jd;
            FixedJoint fj = g.AddComponent<FixedJoint>();
            fj.connectedBody = handsAnchorR.GetComponent<Rigidbody>();
            
            g.layer = 11;
            return true;
        }
        return false;
    }

    public GameObject removeFromHandsL () {
        handsItemL.GetComponent<Rigidbody> ().isKinematic = false;
        handsItemL.GetComponent<Collider> ().enabled = true;
        handsItemL.transform.parent = null;

        handsItemL.transform.localScale = new Vector3 (1, 1, 1);
        GameObject g = handsItemL;
        handsItemL = null;
        FixedJoint fj = g.GetComponent<FixedJoint>();
        if (fj != null)
            Destroy(fj, 0.05f);
        g.layer = 1;
        return g;
    }

    public GameObject removeFromHandsR () {
        handsItemR.GetComponent<Rigidbody> ().isKinematic = false;
        handsItemR.GetComponent<Collider> ().enabled = true;
        handsItemR.transform.parent = null;

        handsItemR.transform.localScale = new Vector3 (1, 1, 1);
        GameObject g = handsItemR;
        handsItemR = null;
        FixedJoint fj = g.GetComponent<FixedJoint>();
        if (fj != null)
            Destroy(fj, 0.05f);

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

    public bool canFitBag(GameObject g){
        InvContainer ic = g.GetComponent<InvContainer>();
        return ic != null && ic.isBackPack && backPack == null;
    }
}