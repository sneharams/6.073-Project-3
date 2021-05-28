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
    private bool isActive;

    private GridSquare activeSquare;

    public List<GameObject> staticObjects;

    [HideInInspector] public int seedType;

    [HideInInspector] public int moveType;

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
        // Debug.Log(output);
    }
    void Start() {
        moveType = 4;
        // moveType = 4;
        seedType = 0;
        isActive = false;
        gridSquares = new List<List<GameObject>>();
        Vector3 sourcePos = sourceSquare.transform.position;
        for (int iX = 1; iX <= gridX; iX++) {
            List<GameObject> rowList = new List<GameObject>();
            for (int iY = 1; iY <= gridY; iY++) {
                Vector3 pos = new Vector3(sourcePos.x-iX*4f, sourcePos.y, sourcePos.z-iY*4f);
                GameObject square = Instantiate(sourceSquare, pos, sourceSquare.transform.rotation, transform) as GameObject;
                // Set Square Variables
                GridSquare settings = square.GetComponent<GridSquare>();
                settings.xPos=iX;
                settings.yPos=iY;
                settings.status = GridSquare.Status.IsEmpty;
                rowList.Add(square);
            }
            gridSquares.Add(rowList);
        }
        foreach (GameObject obj in staticObjects) {
            int col = obj.GetComponent<Position>().column-1;
            int row = obj.GetComponent<Position>().row-1;
            gridSquares[col][row].GetComponent<GridSquare>().status = GridSquare.Status.HasObject;
            // grid.GetComponent<GridContainer>().setObject(col, row);
            // permGrid[col, row] = true;
        }
        sourceSquare.SetActive(false);
    }


    // private GridSquare getSquare(int xPos, int yPos) {
    //     return gridSquares[xPos-1,yPos-1].GetComponent<GridSquare>();
    // }

    // public GridSquare.Status getStatus(int xPos, int yPos) {
    //     return getSquare(xPos, yPos).status;
    // }

    public bool hasSoil(int xPos, int yPos) {
        if (xPos <= 0 || xPos > gridX || yPos <= 0 || yPos > gridY) {
            return false;
        }
        // bool isActive = gridSquares[xPos-1][yPos-1].GetComponent<GridSquare>().patch.activeSelf;
        bool isActive = gridSquares[xPos-1][yPos-1].GetComponent<GridSquare>().patchState;
        // return isActive || isChangeSoil;
        return isActive;
    }

    public bool isEmpty(int xPos, int yPos) {
        if (xPos <= 0 || xPos > gridX || yPos <= 0 || yPos > gridY) {
            return false;
        }
        return gridSquares[xPos-1][yPos-1].GetComponent<GridSquare>().status == GridSquare.Status.IsEmpty;
    }

    public void setActiveState(bool state, GridSquare square) {
        print("SET_ACTIVE_STATE");
        isActive = state;
        if (state == true) {
            print("|_SET");
            if (activeSquare != null) {
                print("| |_REPLACE");
                print("| |--> Old Square", activeSquare.xPos, activeSquare.yPos);
                activeSquare.triggerEvent(-1);
            }
            // print("|_SET");
            print("|--> New Square", square.xPos, square.yPos);
            activeSquare = square;
            // print(activeSquare.xPos, activeSquare.yPos);
        } else {
            print("|_UNSET");
            if (activeSquare != null) {
                print("| |_CAN UNSET");
                activeSquare.triggerEvent(-1);
                activeSquare = null;
                print("| |--> Called Square", square.xPos, square.yPos);
            } else {
                print("| |_CAN'T UNSET");
                print("| |--> shouldn't reach (no square to unset)");
            }
        }
    }

    public void setMoveState() {
        if (activeSquare != null) {
            activeSquare.triggerEvent(-1);
            activeSquare = null;
        }
    }

    public bool isWithinGrid(int xPos, int yPos) {
        if (xPos <= 0 || xPos > gridX || yPos <= 0 || yPos > gridY) {
            return false;
        } else {
            return !hasObject(xPos, yPos);
        }
    }

    public bool hasObject(int xPos, int yPos) {
        return gridSquares[xPos-1][yPos-1].GetComponent<GridSquare>().status == GridSquare.Status.HasObject;
    }

    public bool getDeleteState(int xPos, int yPos) {
        if (xPos <= 0 || xPos > gridX || yPos <= 0 || yPos > gridY) {
            return false;
        }
        return gridSquares[xPos-1][yPos-1].GetComponent<GridSquare>().isDelete;
    }

    public bool isSquareActive() {
        return isActive;
    }

    public void setSeed(int seed) {
        seedType = seed;
    }

    public void unsetSeed() {
        seedType = 0;
    }

    public void setObject(int xPos, int yPos) {
        // gridSquares[xPos][yPos].GetComponent<GridSquare>().status = GridSquare.Status.HasObject;
    }

    public void setMoveType(int change) {
        moveType = change;
        print(moveType);
    }
}
