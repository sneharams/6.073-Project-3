using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SunMeter : MonoBehaviour
{
    public GameObject sun;
    public Light sunLight;

    private float angleStart = 90f;
    private float angleEnd = -90f;
    private float time;
    private float printTime;
    private float timeInDay = 480f;

    public int timeMult;





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
        printTime = 5f;
        time = (printTime/24f)*timeInDay; 
        timeMult = 1; 
    }

    void Update() {
        float lightAngle = time/timeInDay * 180;
        float sunAngle = 90 - lightAngle;
        // print(sunAngle);
        Quaternion sunRotation = Quaternion.Euler(0f, 0f, sunAngle);
        // Vector3 sunRotation = new Vector3(0f, 0f, sunAngle);
        sun.transform.localRotation = sunRotation;
        lightAngle = (timeInDay-time)/timeInDay * 190;
        lightAngle -= 5;
        Quaternion lightRotation = Quaternion.Euler(lightAngle, 60f, 0f);
        sunLight.transform.localRotation = lightRotation;
        time += Time.deltaTime * timeMult;
        if (time > timeInDay) {
            time = 0f;
        }
    }
}
