using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SeedOption : MonoBehaviour
{
    public GameObject seed;
    public GameObject halo;
    public GameObject gridObj;
    public int seedType;
    private bool isHover;

    private GridContainer grid;

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
        seed.SetActive(false);
        halo.SetActive(false);
        grid = gridObj.GetComponent<GridContainer>();
        isHover = false;
    }

    void Update() {
        if (grid.seedType == seedType && !halo.activeSelf) {
            halo.SetActive(true);
        } else if (grid.seedType != seedType && halo.activeSelf) {
            halo.SetActive(false);
        }
        if ((isHover || halo.activeSelf) && !seed.activeSelf) {
            seed.SetActive(true);
        } else if (!isHover && !halo.activeSelf && seed.activeSelf) {
            seed.SetActive(false);
        }
    }

    void OnMouseOver() {
        isHover = true;
        seed.SetActive(true);
    }

    void OnMouseExit() {
        isHover = false;
        if (!halo.activeSelf) {
            seed.SetActive(false);
        }
    }

    void OnMouseUp() {
        if (halo.activeSelf) {
            grid.unsetSeed();
            halo.SetActive(false);
        } else {
            grid.setSeed(seedType);
            halo.SetActive(true);
        }
        
    }


    // public void water() {
    //     saturation += .1f;
    //     if (saturation > 1) {
    //         saturation = 1f;
    //     }
    // }


    // public void hover(int state) {
    //     numHover += state;
    // }

    // public void init() {
    //     saturation = .2f;
    //     xp = 0f;
    //     timer = stageTime;
    //     currStage = 0;
    //     stages[currStage].SetActive(true);
    //     numHover = 0;
    // }

    // public void reset() {
    //     foreach (GameObject stage in stages) {
    //         stage.SetActive(false);
    //     }
    // }

    // public void getInfo() {
    //     print("____________");
    //     print("Sat", saturation, idealSaturation);
    //     print("XP ", xp, goalXp);
    // }

}
