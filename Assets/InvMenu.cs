using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// controls how the inventory items are displayed
public class InvMenu : MonoBehaviour {

    public GameObject player;
    GameObject handsGridHolder;

    GridHolder prevHolder, nextHolder;
    GameObject prevGrid;
    Vector2 prevMouseLoc, prevItemAnchorPos;
    // use lastButton to reassign and reset items
    MovableButton lastButton;

    // variables for ui size
    // gridsize: width and height of a gridsquare
    // grid spacing: the space between grid squares
    // handle height: the height of the drag handle for an inv container
    float gridSize = 70, gridSpaceing = 6, handleHeight = 20, closeWidth;

    // click parameters
    // clickDur: the time to register a double click
    // clickTimer: keeps track of the time from the last click
    float clickDur = 0.5f, clickTimer = 1;
    bool doubleClick = false;

    MenuUtil ui;

    void Start () {
       
        closeWidth = handleHeight;
        ui = MenuUtil.get();
        
        // build the inventories
        setupInv ();
    }

    void setupInv () {

        buildPlayerHands (player.GetComponent<InvPlayer> ());
        buildPlayerBag (player.GetComponent<InvPlayer> ());

    }

    void OnEnable () {
        setupInv ();
    }

    void OnDisable () {
        // destroy everything
        List<RectTransform> rl = getChildList ();
        // Debug.Log("child list count " + rl.Count);
        for (int i = 0; i < rl.Count; i++) {
            Destroy (rl[i].gameObject, 0.01f);
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetButtonDown ("Fire1") && clickTimer < clickDur) {
            MouseDoubleClick ();
            doubleClick = true;
        } else if (Input.GetButtonDown ("Fire1")) {
            MouseDown ();
            clickTimer = 0;
            doubleClick = false;
        } else if (Input.GetButtonUp ("Fire1") && !doubleClick) {
            MouseUp ();
            
        }
        clickTimer += Time.deltaTime;

    }

    void MouseDoubleClick () {
        Debug.Log ("double click");
        Vector2 mp = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        RectTransform gridForm = findHover(mp.x, mp.y);

        if (gridForm == null)
            return;
        GameObject grid = gridForm.GetChild(0).gameObject;

        if (grid != null) {
            
            int[] loc = calcIndexInGrid (grid.GetComponent<RectTransform> (), prevMouseLoc.x, prevMouseLoc.y);
            GridHolder prevHolder = grid.GetComponent<GridHolder> ();
            GameObject item = prevHolder.get (loc[0], loc[1]);
            InvContainer itemContainer = item.GetComponent<InvContainer> ();
            if (itemContainer != null) {
                buildInv (itemContainer);
            }
        }
    }

    void MouseDown () {
        Debug.Log ("pointer down");
        Vector2 mp = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        prevMouseLoc = Input.mousePosition;
        // Debug.Log ("mp: " + mp);
        RectTransform hovered = findHover (mp.x, mp.y);

        // Debug.Log ("grid pos: " + mp);
        if (hovered != null) {
            // Debug.Log ("hovering " + hovered.gameObject.name);
            RectTransform gridHolder = (RectTransform) hovered.GetChild (0);
            prevGrid = gridHolder.gameObject;
            prevHolder = gridHolder.gameObject.GetComponent<GridHolder> ();
            // new Vector2 ((Input.mousePosition.x - gridHolder.position.x) / gridSize, (gridHolder.position.y - Input.mousePosition.y) / gridSize);
            // Debug.Log ("down grid pos: " + prevGridLoc);
            hovered.SetAsLastSibling ();
        }

    }

    void MouseUp () {
        Debug.Log ("pointer up");
        Vector2 mp = new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        GridHolder takeFrom = prevHolder;
        RectTransform dropOn = findHover (mp.x, mp.y);
        // Debug.Log("dropping on " + dropOn.name);
        GameObject gridPanel = null;
        GridHolder putIn = null;
        int xdrop = 0;
        int ydrop = 0;
        // Debug.Log ("dropOn is null: " + (dropOn == null));
        if (dropOn != null) {
            gridPanel = dropOn.GetChild (0).gameObject;
            putIn = gridPanel.GetComponent<GridHolder> ();
            // Debug.Log ("putIn is null: " + (putIn == null));

            RectTransform dropTrans = gridPanel.GetComponent<RectTransform> ();
            Vector2 dropLoc = new Vector2 ((Input.mousePosition.x - dropTrans.position.x) / gridSize, (dropTrans.position.y - Input.mousePosition.y) / gridSize);
            xdrop = (int) dropLoc.x;
            ydrop = (int) dropLoc.y;
            Debug.Log ("drop loc: " + dropLoc);
        }

        // if itemHolder is not null, you are holding an item button
        GameObject itemHolder = null;
        RectTransform itemTrans = null;
        if (lastButton != null) {
            // Debug.Log("last button not null");
            itemHolder = lastButton.gameObject.transform.parent.gameObject;
            // Debug.Log("itemHolder is null: " + (itemHolder == null));
            itemTrans = itemHolder.GetComponent<RectTransform> ();
            // Debug.Log("itemTrans is null: " + (itemTrans == null));
        }

        // if there is an itemp
        if (itemHolder != null) {

            int[] prevLoc = calcIndexInGrid (prevGrid.GetComponent<RectTransform> (), prevMouseLoc.x, prevMouseLoc.y);

            if (takeFrom == null)
                return;

            GameObject item = takeFrom.get (prevLoc[0], prevLoc[1]);

            // and if there is somewhere to put it
            if (putIn != null && putIn.canFit (item, xdrop - lastButton.x, ydrop - lastButton.y)) {
                Debug.Log ("moving item");

                takeFrom.remove (prevLoc[0], prevLoc[1]);
                putIn.add (item, xdrop - lastButton.x, ydrop - lastButton.y);
                // set the new position

                Debug.Log ($"drop pos {xdrop} {ydrop} \nbutton pos: {lastButton.x} {lastButton.y}");
                itemTrans.anchoredPosition = new Vector2 ((xdrop - lastButton.x) * gridSize, -(ydrop - lastButton.y) * gridSize);
                itemTrans.SetParent (gridPanel.transform, false);
                // Debug.Log ("moved the item");

            } else if (putIn == null) { // else drop on the floor
                Debug.Log ("dropping");
                GameObject g = takeFrom.remove (prevLoc[0], prevLoc[1]);
                g.transform.position = player.GetComponent<InvPlayer> ().handsAnchor.transform.position;
                Destroy (itemTrans.gameObject, 0.02f);

            } else { // or drop on the floor
                Debug.Log ("returning to position");
                // return to pos
                itemTrans.anchoredPosition = prevItemAnchorPos;
                // Debug.Log ("returned item to previos pos");
            }
        }

        lastButton = null;
        prevGrid = null;
        prevHolder = null;
    }

    // assume lower left relative coords
    int[] calcIndexInGrid (RectTransform gridHolder, float x, float y) {
        int[] arr = new int[2];

        Vector2 posInGrid = new Vector2 ((x - gridHolder.position.x) / gridSize, (gridHolder.position.y - y) / gridSize);
        arr[0] = (int) posInGrid.x;
        arr[1] = (int) posInGrid.y;
        // Debug.Log("index loc " + arr[0] + " " + arr[1]);
        return arr;
    }

    // build an inv from an obj with a InvContainer component
    void buildInv (GameObject g) {
        InvContainer c = g.GetComponent<InvContainer> ();
        if (c != null) {
            buildInv (c);
        }
    }

    GameObject buildPlayerHands (InvPlayer inv) {
        GameObject[, ] bag = new GameObject[1, 1];
        string name = "Hands";

        // make a mainpanel here
        GameObject mainPanel = makeMainPanel (1, 1, gameObject);
        mainPanel.name = "HandsPanel";

        GameObject gridHolder = ui.makeBlankPanel (0, -handleHeight, bag.GetLength (0) * gridSize, bag.GetLength (1) * gridSize, mainPanel);
        handsGridHolder = gridHolder;
        gridHolder.name = "Grid Holder";
        GridPlayerHandsHolder gph = gridHolder.AddComponent<GridPlayerHandsHolder> ();
        gph.p = player.GetComponent<InvPlayer> ();
        gph.color = new Color (0, 0, 0, 0);

        makeGrid (bag.GetLength (0), bag.GetLength (1), gridHolder);
        GameObject handle = makeMoveHandle (bag, name, mainPanel);
        handle.GetComponent<RectTransform> ().sizeDelta = new Vector2 (mainPanel.GetComponent<RectTransform> ().sizeDelta.x, handleHeight);
        // build all the items in the contents
        GameObject obj = inv.handsItem;
        if (obj != null)
            buildItem (obj, gridHolder);

        return mainPanel;
    }

    // FIX THIS
    void buildPlayerBag (InvPlayer inv) {
        GameObject[, ] bag = new GameObject[1, 1];
        string name = "Back";
        List<ObjLoc> cont = null;

        // make a mainpanel here
        GameObject mainPanel = makeMainPanel (1, 1, gameObject);
        mainPanel.name = "BackPackPanel";

        GameObject gridHolder = ui.makeBlankPanel (0, -handleHeight, bag.GetLength (0) * gridSize, bag.GetLength (1) * gridSize, mainPanel);
        gridHolder.name = "Grid Holder";
        GridPlayerBagHolder gbh = gridHolder.AddComponent<GridPlayerBagHolder> ();
        gbh.p = player.GetComponent<InvPlayer> ();
        gbh.color = new Color (0, 0, 0, 0);

        makeGrid (bag.GetLength (0), bag.GetLength (1), gridHolder);
        GameObject handle = makeMoveHandle (bag, name, mainPanel);
        handle.GetComponent<RectTransform> ().sizeDelta = new Vector2 (mainPanel.GetComponent<RectTransform> ().sizeDelta.x, handleHeight);

        GameObject obj = inv.backPack;
        if (obj != null)
            buildItem (obj, gridHolder);

        // build all the items in the contents
        if (cont != null) {
            for (int i = 0; i < cont.Count; i++) {
                buildItem (cont[i], gridHolder);
            }
        }
    }

    // builds an item at a specific grid position
    void buildItem (ObjLoc obj, GameObject gridHolder) {
        float r = Random.Range (0.0f, 1.0f);
        float g = Random.Range (0.0f, 1.0f);
        float b = Random.Range (0.0f, 1.0f);
        Color c = new Color (r, g, b, 1);
        int[, ] top = obj.g.GetComponent<InvObj> ().top;

        // holds the tetris piece snugly
        GameObject itemHolder = ui.makeImagePanel (obj.x * gridSize, -gridSize * obj.y, top.GetLength (0) * gridSize, top.GetLength (1) * gridSize, gridHolder);
        itemHolder.GetComponent<Image> ().color = new Color (1, 1, 1, 0);

        for (int x = 0; x < top.GetLength (0); x++) {
            for (int y = 0; y < top.GetLength (1); y++) {
                if (top[x, y] == 1) {
                    GameObject p = ui.makeImagePanel (x * gridSize, -y * gridSize, gridSize, gridSize, itemHolder);
                    p.GetComponent<Image> ().color = c;
                    MovableButton butt = p.AddComponent<MovableButton> ();
                    butt.x = x;
                    butt.y = y;
                    butt.container = this;
                }
            }
        }
    }

    // builds a single item in the panel, with no location and 1x1 size
    void buildItem (GameObject itm, GameObject gridHolder) {
        float r = Random.Range (0.0f, 1.0f);
        float g = Random.Range (0.0f, 1.0f);
        float b = Random.Range (0.0f, 1.0f);
        Color c = new Color (r, g, b, 1);
        // Debug.Log ("rgb: " + r + " " + g + " " + b);

        // holds the tetris piece snugly
        GameObject itemHolder = ui.makeImagePanel (0, 0, gridSize, gridSize, gridHolder);
        itemHolder.GetComponent<Image> ().color = new Color (1, 1, 1, 0);

        int[, ] top = itm.GetComponent<InvObj> ().top;

        for (int x = 0; x < top.GetLength (0); x++) {
            for (int y = 0; y < top.GetLength (1); y++) {
                if (top[x, y] == 1) {
                    GameObject p = ui.makeImagePanel (x * gridSize, -y * gridSize, gridSize, gridSize, itemHolder);
                    p.GetComponent<Image> ().color = c;
                    MovableButton mb = p.AddComponent<MovableButton> ();
                    mb.x = x;
                    mb.y = y;
                    mb.container = this;
                }
            }
        }

    }

    // makes a panel that has room for a handle  at the top and xy grid elements
    GameObject makeMainPanel (int x, int y, GameObject parent) {
        GameObject mainPanel = ui.makeImagePanel (gridSize, -gridSize, x * gridSize, y * gridSize + handleHeight, parent);
        mainPanel.name = "MainPanel";
        return mainPanel;
    }

    // makes a grid out of panels
    void makeGrid (int xi, int yi, GameObject parent) {
        for (int y = 0; y < yi; y++) {
            for (int x = 0; x < xi; x++) {
                ui.makeImagePanel (x * gridSize + gridSpaceing / 2, y * (-gridSize) - gridSpaceing / 2, gridSize - gridSpaceing, gridSize - gridSpaceing, parent);
            }
        }
    }

    // make a movement button/handle at the top of a panel (parent) with the text component displaying title
    GameObject makeMoveHandle (GameObject[, ] bag, string title, GameObject parent) {
        RectTransform parentRect = parent.GetComponent<RectTransform> ();
        GameObject b = ui.makeTextPanel (0, 0, parentRect.sizeDelta.x - closeWidth, handleHeight, parent);
        b.AddComponent<MovableButton> ();
        Text t = b.GetComponent<Text> ();
        t.text = title;
        t.fontSize = (int)(handleHeight/2);
        return b;
    }

    // builds an inventory widnow based on the invContainer
    void buildInv (InvContainer c) {
        GameObject[, ] bag = c.b.bag;
        string name = c.name;
        List<ObjLoc> cont = c.b.contents;

        // make a mainpanel here
        GameObject mainPanel = makeMainPanel (bag.GetLength (0), bag.GetLength (1), gameObject);

        GameObject gridHolder = ui.makeBlankPanel (0, -handleHeight, bag.GetLength (0) * gridSize, bag.GetLength (1) * gridSize, mainPanel);
        gridHolder.name = "Grid Holder";
        GridBagHolder gbh = gridHolder.AddComponent<GridBagHolder> ();
        gbh.bag = c;
        gbh.color = new Color (0, 0, 0, 0);

        makeGrid (bag.GetLength (0), bag.GetLength (1), gridHolder);
        makeMoveHandle (bag, name, mainPanel);
        makeCloseButton (mainPanel);

        // build all the items in the contents
        if (cont != null) {
            for (int i = 0; i < cont.Count; i++) {
                buildItem (cont[i], gridHolder);
            }
        }

    }

    void makeCloseButton (GameObject parent) {
        RectTransform parentRect = parent.GetComponent<RectTransform> ();
        GameObject b = ui.makeImagePanel (parentRect.sizeDelta.x - closeWidth, 0, closeWidth, handleHeight, parent);
        b.GetComponent<Image> ().color = new Color (1, 0, 0, 1);
        b.AddComponent<CloseButton> ();
    }

    

    public class MovableButton : Button, IDragHandler, IPointerDownHandler {
        // button that moves its parent around like a handle
        RectTransform rt;
        Vector2 prev;
        public int x, y;
        public InvMenu container;

        void Start () {
            rt = gameObject.transform.parent.gameObject.GetComponent<RectTransform> ();
            gameObject.name = "MovableButton";
            // Debug.Log("anchoredPosition " + rt.anchoredPosition);

        }

        public void OnPointerDown (PointerEventData data) {
            // Debug.Log("down");
            prev = data.position;
            // Debug.Log("button index: " + x + " " +  y);

            if (container != null) {
                container.lastButton = this;
                // Debug.Log ("set last button");
                container.prevItemAnchorPos = ((RectTransform) gameObject.transform.parent).anchoredPosition;

            }
        }
        public void OnDrag (PointerEventData data) {

            rt.position = new Vector2 (rt.position.x - (prev.x - data.position.x), rt.position.y - (prev.y - data.position.y));
            prev = data.position;
        }
    }

    public class CloseButton : Button, IPointerDownHandler {

        public void OnPointerDown (PointerEventData data) {

            Destroy (gameObject.transform.parent.gameObject, 0.05f);
        }
    }

    //use the grid holder as an interface to swap items, display, and interact with  bags
    public interface GridHolder {

        bool add (GameObject g, int x, int y);
        GameObject remove (int x, int y);
        GameObject get (int x, int y);
        bool canFit (GameObject g, int x, int y);
    }

    public class GridBagHolder : Image, GridHolder {
        public InvContainer bag;

        public bool add (GameObject g, int x, int y) {
            return bag.b.addItem (g, x, y);
        }
        public GameObject remove (int x, int y) {
            return bag.b.removeItem (x, y);
        }

        public GameObject get (int x, int y) {
            return bag.b.bag[x, y];
        }
        public bool canFit (GameObject g, int x, int y) {
            return bag.b.canFit (g, x, y);
        }

    }

    public class GridPlayerHandsHolder : Image, GridHolder {
        public InvPlayer p;
        public bool add (GameObject g, int x, int y) {
            return p.addToHands (g);
        }
        public GameObject remove (int x, int y) {

            return p.removeFromHands ();
        }
        public GameObject get (int x, int y) {
            return p.handsItem;
        }
        public bool canFit (GameObject g, int x, int y) {
            if (g == null)
                return false;
            return p.handsItem == null;
        }

    }

    public class GridPlayerBagHolder : Image, GridHolder {
        public InvPlayer p;
        public bool add (GameObject g, int x, int y) {

            return p.addBackPack (g);
        }
        public GameObject remove (int x, int y) {
            return p.removeBackPack ();
        }
        public GameObject get (int x, int y) {
            return p.backPack;
        }
        public bool canFit (GameObject g, int x, int y) {
            return p.canFitBag (g);
        }
    }

    // takes a pixel location and returns the first panel in the heirarchy
    // it is hovering over
    RectTransform findHover (float x, float y) {

        List<RectTransform> rl = getChildList ();
        for (int i = 0; i < rl.Count; i++) {
            if (InvMenu.isInside (x, y, rl[i])) {
                return rl[i];
            }
        }
        return null;
    }

    // finds if a coordinate is inside a rectangle
    static bool isInside (float x, float y, RectTransform r) {

        if (x > r.anchoredPosition.x && x < r.anchoredPosition.x + r.sizeDelta.x && -y < r.anchoredPosition.y && -y > r.anchoredPosition.y - r.sizeDelta.y) {
            // Debug.Log("rect inside r: " + r.anchoredPosition);
            return true;
        }
        return false;
    }

    //gets the list of children transforms for the object this script is attached to
    List<RectTransform> getChildList () {

        List<RectTransform> list = new List<RectTransform> ();
        RectTransform rt = gameObject.GetComponent<RectTransform> ();
        for (int i = rt.childCount - 1; i >= 0; i--) {
            list.Add ((RectTransform) rt.GetChild (i));
        }
        return list;

    }

}