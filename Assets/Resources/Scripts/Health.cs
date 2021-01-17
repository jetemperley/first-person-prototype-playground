using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    bool left = false, right = false;
    public Canvas displayTarget;
    GameObject screen;
    MenuUtil ui;
    StatusBar health, stamina;
    

    void Start(){
        screen = displayTarget.gameObject;
        ui = MenuUtil.get();
        
        GameObject hb = makeStatBar(10, 10, 150, 25);
        health = new StatusBar(100, hb.GetComponent<RectTransform>());

        GameObject sb = makeStatBar(10,45, 150, 25);
        sb.GetComponent<Image>().color = Color.yellow;
        stamina = new StatusBar(100, sb.GetComponent<RectTransform>());
    }

    void Update(){
        if (stamina.currentStat < stamina.maxStat){
            stamina.addStat(Time.deltaTime*5);
        }
    }

    void addHealth(float x){
        health.addStat(x);
        //Debug.Log("damage applied");
    }

    void kill(){
        health.setStat(-1f);
    }

    void crushL(bool cl){
        left = cl;
        checkCrush();
    }

    void crushR(bool cr){
        right = cr;
        checkCrush();
    }

    void checkCrush(){
        if (left && right){
            kill();
            Debug.Log("player crushed");
        }
        
    }

    public float getStam(){
        return stamina.currentStat;
    }

    public void addStam(float f){
        stamina.addStat(f);
    }

    GameObject makeStatBar(float x, float y, float width, float height){

        float margin = 2;

        GameObject hb = ui.makeImagePanel(0, 0, width, height, screen);
        Vector2 v =  new Vector2(0, 0);
        RectTransform rt = hb.GetComponent<RectTransform>();
        rt.anchorMin = v;
        rt.anchorMax = v;
        rt.pivot = v;
        rt.anchoredPosition = new Vector2(x, y);
        hb.GetComponent<Image>().color = Color.black;

        GameObject hbi = ui.makeImagePanel(0, 0, width - (margin*2), height - (margin*2), hb);
        rt = hbi.GetComponent<RectTransform>();
        rt.anchorMin = v;
        rt.anchorMax = v;
        rt.pivot = v;
        rt.anchoredPosition = new Vector2(margin, margin);
        hbi.GetComponent<Image>().color = Color.red;

        Text text = ui.makeTextPanel(0, 0, width - (margin*2), height - (margin*2), hbi).GetComponent<Text>();
        text.fontSize = 15;

        return hbi;

    }

    class StatusBar{

        public float maxStat, currentStat, maxWidth;
        RectTransform uiBar;
        Text label;

        public StatusBar(float max, RectTransform bar){
            maxStat = max;
            currentStat = maxStat;
            uiBar = bar;
            maxWidth = bar.sizeDelta.x;
            label = bar.transform.GetChild(0).GetComponent<Text>();
            label.text = "" + currentStat;
        }

        public void update(){
            if (currentStat > maxStat)
                currentStat = maxStat;

            float width = (currentStat/maxStat) * maxWidth;
            uiBar.sizeDelta = new Vector2(width, uiBar.sizeDelta.y);
            label.text = "" + currentStat;
        }

        public void addStat(float x){
            currentStat += x;
            update();
        }

        public void setStat(float x){
            currentStat = x;
            update();
        }

    }
}
