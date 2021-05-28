using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SquareMenu : MonoBehaviour
{
    #region Class Variables
    public GameObject cam;
    public GameObject square;
    public GameObject meter;

    // Buttons
    [Header("Buttons")]
    public GameObject soilButton;
    public GameObject deleteButton;
    public GameObject moveButton;
    public GameObject waterButton;
    public GameObject seedButton;
    public GameObject harvestButton;
    private GridSquare gridSquare;

    private Vector3 startPos;

    private Vector3 slideDelta = new Vector3(0f, 2.5f, 0f);


    #endregion

    void Start() {
        Vector3 angle = cam.GetComponent<CameraController>().getCameraAngle();
        startPos = moveButton.transform.position;
        soilButton.transform.eulerAngles = angle;
        waterButton.transform.eulerAngles = angle;
        moveButton.transform.eulerAngles = angle;
        deleteButton.transform.eulerAngles = angle;
        seedButton.transform.eulerAngles = angle;
        harvestButton.transform.eulerAngles = angle;
        gridSquare = square.GetComponent<GridSquare>();
        if (gridSquare.status == GridSquare.Status.HasObject && meter.activeSelf) {
            meter.SetActive(false);
        } else if (gridSquare.status != GridSquare.Status.HasObject && !meter.activeSelf) {
            meter.SetActive(true);
        }
    }

    void OnEnable() {
        Vector3 angle = cam.GetComponent<CameraController>().getCameraAngle();
        soilButton.transform.eulerAngles = angle;
        waterButton.transform.eulerAngles = angle;
        moveButton.transform.eulerAngles = angle;
        deleteButton.transform.eulerAngles = angle;
        seedButton.transform.eulerAngles = angle;
        harvestButton.transform.eulerAngles = angle;
        if (gridSquare.status == GridSquare.Status.HasObject && meter.activeSelf) {
            meter.SetActive(false);
        } else if (gridSquare.status != GridSquare.Status.HasObject && !meter.activeSelf) {
            meter.SetActive(true);
        }
    }

    void Update() {
        Vector3 angle = cam.GetComponent<CameraController>().getCameraAngle();
        soilButton.transform.eulerAngles = angle;
        waterButton.transform.eulerAngles = angle;
        moveButton.transform.eulerAngles = angle;
        deleteButton.transform.eulerAngles = angle;
        seedButton.transform.eulerAngles = angle;
        harvestButton.transform.eulerAngles = angle;
        GridSquare gridSquare = square.GetComponent<GridSquare>();
        if (gridSquare.status == GridSquare.Status.HasObject && meter.activeSelf) {
            meter.SetActive(false);
        } else if (gridSquare.status != GridSquare.Status.HasObject && !meter.activeSelf) {
            meter.SetActive(true);
        }
        if (soilButton.activeSelf && !gridSquare.status.Equals(GridSquare.Status.HasObject)) {
            if (soilButton.GetComponent<OptionButton>().isHover > 0) {
                gridSquare.setSoil(true);
            } else {
                gridSquare.setSoil(false);
            }
        } else{
            if (deleteButton.activeSelf) {
                if (deleteButton.GetComponent<OptionButton>().isHover > 0) {
                    gridSquare.setDelete(true);
                } else {
                    gridSquare.setDelete(false);
                }
            }
            if (seedButton.activeSelf) {
                if (seedButton.GetComponent<OptionButton>().isHover > 0) {
                    gridSquare.setPlant(true);
                } else {
                    gridSquare.setPlant(false);
                }
            }
        } 
    }

    public void initOptions(
        bool isAdd, 
        bool canWater, 
        bool canSeed, 
        bool canMove, 
        bool isDelete, 
        bool canPump,
        bool nearSack,
        bool canHarvest
    ) {
        int counter = 0;
        counter += setMove(counter, canMove);
        counter += setDelete(counter, isDelete);
        counter += setSoil(counter, isAdd, nearSack);
        counter += setWater(counter, canWater, canPump);
        counter += setSeed(counter, canSeed);
        counter += setHarvest(counter, canHarvest);
    }
    private int setSoil(int pos, bool isAdd, bool nearSack) {
        if (isAdd || nearSack) {
            soilButton.transform.position += slideDelta * pos;
            if (nearSack) {
                soilButton.transform.position += slideDelta/2; // adjust height for sack
            }
            soilButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    private int setDelete(int pos, bool isDelete) {
        if (isDelete) {
            deleteButton.transform.position += slideDelta * pos;
            deleteButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    private int setMove(int pos, bool canMove) {
        if (canMove) {
            moveButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    private int setWater(int pos, bool canWater, bool canPump) {
        if (canWater || canPump) {
            waterButton.transform.position += slideDelta * pos;
            if (canPump) {
                waterButton.transform.position += slideDelta*3/2; // height adjustment for pump
            }
            waterButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    private int setSeed(int pos, bool canSeed) {
        if (canSeed) {
            seedButton.transform.position += slideDelta * pos;
            seedButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    private int setHarvest(int pos, bool canHarvest) {
        if (canHarvest) {
            harvestButton.transform.position += slideDelta * pos;
            harvestButton.SetActive(true);
            return 1;
        }
        return 0;
    }

    public void reset() {
        seedButton.transform.position = startPos;
        moveButton.transform.position = startPos;
        deleteButton.transform.position = startPos;
        waterButton.transform.position = startPos;
        soilButton.transform.position = startPos;
        harvestButton.transform.position = startPos;
        soilButton.SetActive(false);
        waterButton.SetActive(false);
        seedButton.SetActive(false);
        deleteButton.SetActive(false);
        moveButton.SetActive(false);
        harvestButton.SetActive(false);
    }
}
