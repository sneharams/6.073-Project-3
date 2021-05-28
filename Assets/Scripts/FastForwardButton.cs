using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class FastForwardButton : MonoBehaviour
{
    public GameObject sunObj;
    public Material[] materials;

    private bool isHover;
    private bool isPressed;
    private SunMeter sun;

    private Vector3 scale;
    private Vector3 scale2;
    private int matIndex;

    void Awake() {
        scale = transform.localScale;
        scale2 = transform.localScale * 1.2f;
    }



    private void print(params object[] list) 
    {
        string output = "";
        foreach (object item in list) {
            if (item is string) {
                output += item + ": ";
            } else {
                output += item + ", ";
            }
        }
        output = output.Substring(0, output.Length-2);
        Debug.Log(output);
    }

    void Start() {
        isHover = false;
        isPressed = false;
        sun = sunObj.GetComponent<SunMeter>();
        transform.localScale = scale;
        matIndex = 0;
        GetComponent<MeshRenderer>().material = materials[matIndex];
    }

    void Update() {
        if (isHover || isPressed) {
            transform.localScale = scale2;
        }
        else {
            transform.localScale = scale;
        }
    }

    void OnMouseOver() {
        isHover = true;
    }

    void OnMouseExit() {
        isHover = false;
    }

    void OnMouseUp() {
        matIndex += 1;
        
        // isPressed = true;
        if (matIndex == 4) { //5) {
            matIndex = 0;
            sun.timeMult = 1;
            isPressed = false;
            GetComponent<MeshRenderer>().material = materials[matIndex];
        } 
        // else if (matIndex == 3) {//4) {
        //     sun.timeMult = 0;
        //     GetComponent<MeshRenderer>().material = materials[matIndex];
        // }
        else {
            sun.timeMult *= 4;
            GetComponent<MeshRenderer>().material = materials[matIndex];
        }
    }
}
