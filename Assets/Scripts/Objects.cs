using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Objects : MonoBehaviour
{
    public List<GameObject> staticObjects;
    public GameObject grid;
    private bool[,] objGrid;
    private bool[,] soilGrid;
    private bool[,] permGrid;
    private int cols;
    private int rows;

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
        // set grid dimensions
        cols = grid.GetComponent<GridContainer>().gridX;
        rows = grid.GetComponent<GridContainer>().gridY;
        // initialize empty grid
        objGrid = new bool[cols,rows];
        soilGrid = new bool[cols,rows];
        permGrid = new bool[cols, rows];
        for (int col = 0; col < cols; col++) {
            for (int row = 0; row < rows; row++) {
                objGrid[col,row] = true;
                soilGrid[col,row] = false;
                permGrid[col, row] = false;
            }
        }
        // place static objects
        foreach (GameObject obj in staticObjects) {
            int col = obj.GetComponent<Position>().column-1;
            int row = obj.GetComponent<Position>().row-1;
            objGrid[col,row] = false;
            permGrid[col, row] = true;
        }

    }

    public bool isEmpty(int col, int row) {
        return objGrid[col-1,row-1];
    }

    public bool isSoil(int col, int row) {
        return soilGrid[col-1,row-1];
    }

    public bool isPermanent(int col, int row) {
        return permGrid[col-1, row-1];
    }

    // specifically for soil
    public void setSquare(int col, int row, bool status) {
        objGrid[col-1, row-1] = status;
        soilGrid[col-1, row-1] = !status;
    }

    public void setSoil(int col, int row, bool status) {
        soilGrid[col-1, row-1] = status;
    }

}
