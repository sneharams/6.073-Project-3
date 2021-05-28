using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MeterContainer : MonoBehaviour
{
    // public GameObject[] stages;
    // public GameObject menu;
    // public float xp;
    // public float saturation;
    // private float goalXp = 100f;
    // private float idealSaturation = .3f;
    // private float satTolerance = .1f;
    // private float stageTime = 30f;
    // private int currStage;
    // private float timer;
    // private int numHover;

    public GameObject[] fillBars;
    private int fillLevel;
    private bool collecting;
    private bool dispensing;
    private Vector3 fillScale;

    private int fillFrames = 40;
    // private int fillTime;

    private float fillSpeed;

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
        foreach (GameObject bar in fillBars) {
            bar.SetActive(false);
        }
        fillLevel = 0;
        collecting = false;
        dispensing = false;
        // fillTime = 0;
        fillSpeed = 0f;
    }


    public void collect(float speed) {
        // fillTime = fillFrames;
        if (fillLevel < 4) {
            fillScale = fillBars[fillLevel].transform.localScale;
            Vector3 scale = new Vector3 (
                fillScale.x,
                0f,
                fillScale.z
            );
            // print("COLLECT");
            // print(fillScale.x);
            // print(fillLevel);
            fillBars[fillLevel].transform.localScale = scale;
            fillBars[fillLevel].SetActive(true);
            fillSpeed = speed;
            collecting = true;
        }
    }

    void Update() {
        float time = Time.deltaTime*100;
        if (collecting) {
            float currX = fillBars[fillLevel].transform.localScale.x;
            float newX = currX + (fillFrames*fillSpeed*(time/fillFrames));
            // print(newX, fillLevel);
            Vector3 scale = new Vector3 (
                newX,
                fillScale.y,
                fillScale.z
            );
            fillBars[fillLevel].transform.localScale = scale;
            if (newX >= fillScale.x) {
                fillBars[fillLevel].transform.localScale = fillScale;
                fillLevel += 1;
                collecting = false;
            }
        }
        else if (dispensing) {
            float currX = fillBars[fillLevel].transform.localScale.x;
            float newX = currX - (fillFrames*fillSpeed*(time/fillFrames));
            Vector3 scale = new Vector3 (
                newX,
                fillScale.y,
                fillScale.z
            );
            fillBars[fillLevel].transform.localScale = scale;
            if (newX <= 0f) {
                fillBars[fillLevel].transform.localScale = fillScale;
                dispensing = false;
                fillBars[fillLevel].SetActive(false);
            }
        }
    }


    public bool dispense() {
        if (fillLevel == 0) {
            return false;
        }
        dispensing = true;
        fillLevel -= 1;
        return true;
    }

    public bool canDispense() {
        if (fillLevel == 0) {
            return false;
        }
        return true;
    }

}
