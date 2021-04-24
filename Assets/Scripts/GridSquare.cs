using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GridSquare : MonoBehaviour
{
    public GameObject objects;
    public GameObject shovelButton;
    public GameObject controller;
    public GameObject grid;
    public GameObject patch;
    public GameObject patchNorth;
    public GameObject patchEast;

    // Materials
    public Material materialNormal;
    public Material materialValid;
    public Material materialHover;

    // Positioning
    [HideInInspector] public int xPos;
    [HideInInspector] public int yPos;

    
    private bool isValid;
    // private int placeMode;
    private bool isHover;
    private PlayerController player;
    private Objects objGrid;
    private GridContainer container;
    private PlaceSoil clickMode;
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
    void Start() {
        GetComponent<MeshRenderer>().material = materialNormal; 
        isValid = false;
        isHover = false;
        patch.SetActive(false);
        patchNorth.SetActive(false);
        // patchSouth.SetActive(false);
        patchEast.SetActive(false);
        // patchWest.SetActive(false);
        player = controller.GetComponent<PlayerController>();
        objGrid = objects.GetComponent<Objects>();
        clickMode = shovelButton.GetComponent<PlaceSoil>();
        container = grid.GetComponent<GridContainer>();
    }

    void Update() {
        // Update Status
        if (player.isMoving) {
            GetComponent<MeshRenderer>().material = materialNormal;
            isValid = false;
            if (objGrid.isEmpty(xPos, yPos) && patch.activeSelf) {
                patch.SetActive(false);
            }
        } 
        else {
            checkSoil();
            if (clickMode.placeMode == 1) {
                checkAdd();
            } else if (clickMode.placeMode == 2) {
                checkRemove();
            } else {
                checkMove();
            }
            // Update Material
            if (isValid) {
                if (isHover) {
                    if (clickMode.placeMode == 1) {
                        objGrid.setSoil(xPos, yPos, true);
                    } else if (clickMode.placeMode == 2) {
                        objGrid.setSoil(xPos, yPos, false);
                    } 
                    GetComponent<MeshRenderer>().material = materialHover;
                } else {
                    if (clickMode.placeMode == 1) {
                        objGrid.setSoil(xPos, yPos, false);
                    } else if (clickMode.placeMode == 2) {
                        objGrid.setSoil(xPos, yPos, true);
                    }
                    GetComponent<MeshRenderer>().material = materialValid;
                }
            } 
            else {
                if (objGrid.isEmpty(xPos, yPos) && patch.activeSelf) {
                    patch.SetActive(false);
                    // removeSoil();
                }
                GetComponent<MeshRenderer>().material = materialNormal;
            }
        }
    }

    // private void addSoil() {
    //     if (objGrid.isSoil(xPos, yPos)) {
    //         if (objGrid.isSoil(xPos-1, yPos)) {
    //             patchWest.SetActive(true);
    //         }
    //         if (objGrid.isSoil(xPos+1, yPos)) {
    //             patchEast.SetActive(true);
    //         }
    //         if (objGrid.isSoil(xPos, yPos-1)) {
    //             patchSouth.SetActive(true);
    //         }
    //         if (objGrid.isSoil(xPos, yPos+1)) {
    //             patchNorth.SetActive(true);
    //         }
    //     }
    // }

    private void checkSoil() {
        if (objGrid.isSoil(xPos, yPos)) {
            patch.SetActive(true);
            if (objGrid.isSoil(xPos+1, yPos)) {
                patchEast.SetActive(true);
            } else {
                patchEast.SetActive(false);
            }
            if (objGrid.isSoil(xPos, yPos+1)) {
                patchNorth.SetActive(true);
            } else {
                patchNorth.SetActive(false);
            }
        } else {
            patch.SetActive(false);
            patchNorth.SetActive(false);
            patchEast.SetActive(false);
        }
    }

    // private void removeSoil() {
    //     if (objGrid.isSoil(xPos-1, yPos)) {
    //         patchWest.SetActive(false);
    //         container.removeEast(xPos-1, yPos);
    //     }
    //     if (objGrid.isSoil(xPos+1, yPos)) {
    //         patchEast.SetActive(false);
    //         container.removeWest(xPos+1, yPos);
    //     }
    //     if (objGrid.isSoil(xPos, yPos-1)) {
    //         patchSouth.SetActive(false);
    //         container.removeSouth(xPos, yPos-1);
    //     }
    //     if (objGrid.isSoil(xPos, yPos+1)) {
    //         patchNorth.SetActive(false);
    //         container.removeNorth(xPos, yPos+1);
    //     }
    // }

    private void checkAdd() {
        int pX = player.xPos;
        int pY = player.yPos;
        isValid = false;
        if (objGrid.isEmpty(xPos, yPos)) {
            bool validX = (xPos+1 == pX || xPos-1 == pX) && yPos == pY;
            bool validY = (yPos+1 == pY || yPos-1 == pY) && xPos == pX;
            bool validC = (xPos+1 == pX || xPos-1 == pX) && (yPos+1 == pY || yPos-1 == pY);
            if (validX || validY || validC) {
                isValid = true;
            }
        }
    }

    private void checkRemove() {
        int pX = player.xPos;
        int pY = player.yPos;
        isValid = false;
        if (!objGrid.isEmpty(xPos, yPos) && !objGrid.isPermanent(xPos,yPos)) {
            bool validX = (xPos+1 == pX || xPos-1 == pX) && yPos == pY;
            bool validY = (yPos+1 == pY || yPos-1 == pY) && xPos == pX;
            bool validC = (xPos+1 == pX || xPos-1 == pX) && (yPos+1 == pY || yPos-1 == pY);
            if (validX || validY || validC) {
                isValid = true;
            }
        }
    }

    private void checkMove() {
        int pX = player.xPos;
        int pY = player.yPos;
        isValid = false;
        if (objGrid.isEmpty(xPos, yPos)) {
            if (xPos == pX || yPos == pY) {
                if (!(xPos == pX && yPos == pY)) {
                    isValid = checkObstacles();
                }
            }
        }
    }

    private bool checkObstacles() 
    {
        int pX = player.xPos;
        int pY = player.yPos;
        int iStart = 0;
        int iEnd = 0;
        bool isX = true;
        if (xPos == pX) 
        {
            isX = true;
            if (yPos > pY) {
                iStart = pY+1;
                iEnd = yPos;
            } else {
                iStart = yPos+1;
                iEnd = pY;
            }
        } 
        else 
        {
            isX = false;
            if (xPos > pX) {
                iStart = pX+1;
                iEnd = xPos;
            } else {
                iStart = xPos+1;
                iEnd = pX;
            }
        }
        int x = xPos;
        int y = yPos;
        for (int i = iStart; i < iEnd; i++) 
        {
            if (isX) {
                y = i;
            } else {
                x = i;
            }
            if (!objGrid.isEmpty(x, y)) {
                return false;
            }
        }
        return true;
    }

    void OnMouseOver() {
        isHover = true;
    }

    void OnMouseExit() {
        isHover = false;
    }

    void OnMouseUp() {
        if (isValid) {
            if (clickMode.placeMode == 1) {
                patch.SetActive(true);
                objGrid.setSquare(xPos, yPos, false);
            } else if (clickMode.placeMode == 2) {
                patch.SetActive(false);
                objGrid.setSquare(xPos, yPos, true);
            } else {
                player.clickX = xPos;
                player.clickY = yPos;
            }
        }
    }


}
