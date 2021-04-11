using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GridSquare : MonoBehaviour
{

    public Material material1;
    public Material material2;

    void Start() {
        GetComponent<MeshRenderer>().material = material1;
    }

    void OnMouseOver() {
        GetComponent<MeshRenderer>().material = material2;
    }

    void OnMouseExit() {
        GetComponent<MeshRenderer>().material = material1;
    }


}
