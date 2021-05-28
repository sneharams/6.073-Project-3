using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SaturationMeter : MonoBehaviour
{
    // public GameObject plantObj;
    public GameObject water;
    public GameObject drop;
    // public GameObject

    public Material[] zoneMaterials;

    // private Plant plant;

    private float saturation;
    private float saturationPrev;

    private bool updateLevel;
    private Vector3 fullLevel;
    private float dropBottom;
    public float dropRange;
    private int zone;




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
        // plant = plantObj.GetComponent<Plant>();
        saturation = .1f;
        saturationPrev = 0f;
        updateLevel = true;
        fullLevel = water.transform.localScale;
        dropBottom = drop.transform.position.y;
        zone = 1;
        drop.GetComponent<MeshRenderer>().material = zoneMaterials[zone];
    }

    void Update() {
        // dropBottom = new Vector3 (
        //     drop.transform.position.x,
        //     dropBottom.y,
        //     drop.transform.position.z
        // );
        if (updateLevel) {
            Vector3 level = new Vector3 (
                fullLevel.x,
                saturation*fullLevel.y,
                fullLevel.z
            );
            water.transform.localScale = level;

            float satDiff = saturation - saturationPrev;
            float newY = drop.transform.position.y + satDiff*dropRange;
            saturationPrev = saturation;
            // Vector3 height = new Vector3 (
            //     drop.transform.position.x,
            //     newY,
            //     drop.transform.position.z
            // );
            // drop.transform.position = height;
            updateLevel = false;
        }
    }

    public void setSaturation(float plantSat, int satZone) {
        if (plantSat != saturation) {
            updateLevel = true;
        }
        saturation = plantSat;
        if (zone != satZone) {
            zone = satZone;
            drop.GetComponent<MeshRenderer>().material = zoneMaterials[zone];
        }
    }

    public Material getZone() {
        return zoneMaterials[zone];
    }
}
