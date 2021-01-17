using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUtil
{
    static MenuUtil single;
    GameObject panelImageBase, panelTextBase, blankPanel;

    float defaultSize = 100;
    public static GameObject create(){
        return new GameObject();
    }

    MenuUtil(){
        // Setup prefab for image panel
        panelImageBase = new GameObject ();
        RectTransform r = panelImageBase.AddComponent<RectTransform> ();
        panelImageBase.AddComponent<CanvasRenderer> ();
        Image i = panelImageBase.AddComponent<Image> ();

        Vector2 v = new Vector2 (0, 1);
        r.anchorMin = v;
        r.anchorMax = v;
        r.pivot = v;
        r.anchoredPosition = new Vector2 (0, 1);
        r.sizeDelta = new Vector2 (defaultSize, defaultSize);
        i.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);

        // setup prefab for text panel
        panelTextBase = new GameObject ();
        r = panelTextBase.AddComponent<RectTransform> ();
        panelTextBase.AddComponent<CanvasRenderer> ();
        Text t = panelTextBase.AddComponent<Text> ();

        r.anchorMin = v;
        r.anchorMax = v;
        r.pivot = v;
        r.anchoredPosition = new Vector2 (0, 1);
        r.sizeDelta = new Vector2 (defaultSize, defaultSize);

        t.color = new Color (0, 0, 0, 1);
        t.fontSize = (int) (defaultSize / 2);
        t.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");

        // setup blank Panel
        blankPanel = new GameObject ();
        r = blankPanel.AddComponent<RectTransform> ();
        blankPanel.AddComponent<CanvasRenderer> ();

        r.anchorMin = v;
        r.anchorMax = v;
        r.pivot = v;
        r.anchoredPosition = new Vector2 (0, 1);
        r.sizeDelta = new Vector2 (defaultSize, defaultSize);

    }

    public static MenuUtil get(){
        if (MenuUtil.single == null){
            MenuUtil.single = new MenuUtil();
        }
        return MenuUtil.single;
    }

    // make basic panel with image component that is gridSize x gridSize
    public GameObject makeImagePanel (float x, float y, GameObject g) {
        return makeImagePanel (x, y, defaultSize, defaultSize, g);
    }

    // make basic panel with image component
    public GameObject makeImagePanel (float x, float y, float w, float h, GameObject p) {

        GameObject panel = GameObject.Instantiate (panelImageBase);
        RectTransform rt = (RectTransform) panel.transform;
        panel.transform.parent = p.transform;
        // panel.transform.localScale = new Vector3(1, 1, 1);
        rt.anchoredPosition = new Vector2 (x, y);
        rt.sizeDelta = new Vector2 (w, h);

        return panel;
    }

    // make a basic panel with no graphic component
    public GameObject makeBlankPanel (float x, float y, float w, float h, GameObject p) {

        GameObject panel = GameObject.Instantiate (blankPanel);
        RectTransform rt = (RectTransform) panel.transform;
        panel.transform.parent = p.transform;
        // panel.transform.localScale = new Vector3(1, 1, 1);
        rt.anchoredPosition = new Vector2 (x, y);
        rt.sizeDelta = new Vector2 (w, h);

        return panel;
    }

    // make a basic panel with a text component
    public GameObject makeTextPanel (float x, float y, float w, float h, GameObject p) {
        GameObject panel = GameObject.Instantiate (panelTextBase);
        RectTransform rt = (RectTransform) panel.transform;
        panel.transform.SetParent(p.transform);
        // panel.transform.localScale = new Vector3(1, 1, 1);
        rt.anchoredPosition = new Vector2 (x, y);
        rt.sizeDelta = new Vector2 (w, h);

        return panel;
    }
}
