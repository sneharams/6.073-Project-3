using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Grid : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public GameObject sourceSquare;

    private List<List<GameObject>> gridSquares;
    // public float sourceX = 0.45;
    // public float sourceY;
    // public float sideLength = .1f;


    void Start() {
        gridSquares = new List<List<GameObject>>();
        for (int x = 1; x <= gridX; x++) {
            List<GameObject> rowList = new List<GameObject>();
            for (int y = 1; y <= gridY; y++) {
                // rowList.Add(Instantiate(GameObject.Find("1"), transform, ))
            }
        }
    }

    void OnUpdate() {

    }



}
