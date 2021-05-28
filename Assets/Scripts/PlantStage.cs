using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlantStage : MonoBehaviour
{
    public GameObject plant;
    
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
    }


    void OnMouseOver() {
        plant.GetComponent<Plant>().hover(1);
    }

    void OnMouseExit() {
        plant.GetComponent<Plant>().hover(-1);
    }

}
