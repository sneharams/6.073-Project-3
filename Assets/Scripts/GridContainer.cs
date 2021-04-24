using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GridContainer : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public GameObject sourceSquare;
    public GameObject player;
    
    public Material normalMaterial;
    public Material hoverMaterial;
    public Material hoverMaterialOccupied;
    // public Material activeMaterial;

    private List<List<GameObject>> gridSquares;


    void Start() {
        gridSquares = new List<List<GameObject>>();
        Vector3 sourcePos = sourceSquare.transform.position;
        for (int iX = 1; iX <= gridX; iX++) {
            List<GameObject> rowList = new List<GameObject>();
            for (int iY = 1; iY <= gridY; iY++) {
                Vector3 pos = new Vector3(sourcePos.x-iX*4f, sourcePos.y, sourcePos.z-iY*4f);
                GameObject square = Instantiate(sourceSquare, pos, sourceSquare.transform.rotation, transform) as GameObject;
                // Set Square Variables
                square.GetComponent<GridSquare>().xPos=iX;
                square.GetComponent<GridSquare>().yPos=iY;
                rowList.Add(square);
            }
            gridSquares.Add(rowList);
        }
        sourceSquare.SetActive(false);
    }
}
