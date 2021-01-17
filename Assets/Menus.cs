using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour {

    public GameObject PauseScreen, HUD;
    float pauseCooldown = 0.2f, pauseTime = 0;

    void Start () {

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown (KeyCode.Tab) && pauseTime <= 0) {
            if (PauseScreen.activeSelf) {
                // is paused, set to unpause
                Look.enabled = true;
                PauseScreen.SetActive(false);
                HUD.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            } else {
                // is playing, set to pause
                Look.enabled = false;
                PauseScreen.SetActive(true);
                HUD.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
            }
            pauseTime = pauseCooldown;
        }
        if (pauseTime > 0){
            pauseTime -= Time.deltaTime;
        }

    }


}