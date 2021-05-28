using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CoinMeter : MonoBehaviour
{
    // public GameObject plantObj;
    public GameObject meter;
    public GameObject credits;
    // public GameObject drop;
    // public GameObject

    // public Material[] zoneMaterials;

    // // private Plant plant;

    // private float saturation;
    // private float saturationPrev;

    // private bool updateLevel;
    // private Vector3 fullLevel;
    // private float dropBottom;
    // public float dropRange;
    // private int zone;
    private bool endScreenOpened;

    public int funds;




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
        funds = 0;
        add(30);
        // drop.GetComponent<MeshRenderer>().material = zoneMaterials[zone];
    }

    void Update() {
        // if (endScreenOpened) {

        // }
    }

    public void add(int value) {
        if (funds != 100) {
            funds += value;
            if (funds > 100) {
                funds = 100;
                if (!endScreenOpened) {
                    openEndScreen();
                    endScreenOpened = true;
                }
            }
            Vector3 level = new Vector3 (
                1f,
                (funds/100f),
                1f
            );
            meter.transform.localScale = level;
        }
    }

    private void openEndScreen() {
        credits.SetActive(true);
    }

    public void closeScreen() {
        credits.SetActive(false);
    }

    // public void setSaturation(float plantSat, int satZone) {
    //     if (plantSat != saturation) {
    //         updateLevel = true;
    //     }
    //     saturation = plantSat;
    //     if (zone != satZone) {
    //         zone = satZone;
    //         drop.GetComponent<MeshRenderer>().material = zoneMaterials[zone];
    //     }
    // }
}
