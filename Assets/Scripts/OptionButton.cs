using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class OptionButton : MonoBehaviour
{
    public GameObject square;
    public GameObject cam;
    public int clickMode;
    [HideInInspector] public int isHover;

    private Vector3 scale;
    private Vector3 scale2;

    void Awake() {
        scale = transform.localScale;
        scale2 = transform.localScale * 1.2f;
    }
    

    void Start() {
        isHover = 0;
        // transform.eulerAngles = cam.GetComponent<CameraController>().getCameraAngle();
        transform.localScale = scale;
    }

    void OnEnable() {
        isHover = 0;
        // transform.eulerAngles = cam.GetComponent<CameraController>().getCameraAngle();
        transform.localScale = scale;
    }

    void Update() {
        // print(isHover);
        if (isHover > 0) {
            transform.localScale = scale2;
        } else {
            transform.localScale = scale; 
        }
    }


    // void OnMouseOver() {
    //     transform.localScale = scale * 1.2f;
    // }

    // void OnMouseExit() {
    //     transform.localScale = scale;
    // }


    public void toggle() {
        square.GetComponent<GridSquare>().triggerEvent(clickMode);
    }


}
