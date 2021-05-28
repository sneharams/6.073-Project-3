using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Plant : MonoBehaviour
{
    public GameObject[] stages;
    public GameObject meterObj;
    public GameObject sunObj;
    public AudioSource audio;
    public GameObject finalObj;
    public GameObject gridSquareObj;
    public GameObject statusObj;
    public Material[] zoneMaterials;

    public int value;

    public bool isFlower;
    public float xp;
    public float saturation;
    public bool canHarvest;
    private float goalXp = 100f; //100f;
    public float idealSaturation;
    private float satTolerance = .1f;
    private float minTol = .1f;
    private float maxTol = .6f;
    private float stageTime = 17f;//5f;
    private int currStage;
    private float timer;
    private int numHover;
    private bool watered;
    private float pullTime;
    private float scaleTime;
    private float bounceTime;
    private bool doneHarvest;
    private SaturationMeter meter;
    private SunMeter sun;
    private Vector3 fullScale;
    private Vector3 startPos;
    private GridSquare gridSquare;
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
        meter = meterObj.GetComponent<SaturationMeter>();
        sun = sunObj.GetComponent<SunMeter>();
        saturation = .2f;
        xp = 10f;
        timer = stageTime;
        currStage = 0;
        fullScale = stages[currStage].transform.localScale;
        stages[currStage].transform.localScale = new Vector3(0f, 0f, 0f);
        stages[currStage].SetActive(true);
        numHover = 0;
        canHarvest = false;
        watered = true;
        pullTime = 0f;
        bounceTime = 0f;
        scaleTime = 0f;
        startPos = stages[currStage].transform.position;
        doneHarvest = false;
        finalObj.SetActive(false);
        gridSquare = gridSquareObj.GetComponent<GridSquare>();
    }

    void Update() {
        // print(timer, Time.deltaTime);
        if (pullTime > 0) { // .5
            float yOff = (float) Math.Sin((.5f-pullTime)/.5f*(Math.PI/2));
            stages[currStage].transform.position = new Vector3 (
                startPos.x,
                startPos.y+yOff/4,
                startPos.z
            );
            pullTime -= Time.deltaTime;
            if (pullTime <= 0) {
                stages[currStage].transform.position = new Vector3 (
                    startPos.x,
                    startPos.y+.25f,
                    startPos.z
                );
            }
        }  else if (bounceTime > 0) { //.25
            float yOff = (float) Math.Sin((.25f-bounceTime)/.25f*(Math.PI/2));
            stages[currStage].transform.position = new Vector3 (
                startPos.x,
                startPos.y+.25f-yOff/8,
                startPos.z
            );
            bounceTime -= Time.deltaTime;
            if (bounceTime <= 0) {
                stages[currStage].transform.position = new Vector3 (
                    startPos.x,
                    startPos.y+.125f,
                    startPos.z
                );
            }
        } else if (scaleTime > 0) { // .5
            print("scale", scaleTime);
            float yOff = (float) Math.Sin(((.5f-scaleTime)/.5f)*(Math.PI/4));
            stages[currStage].transform.position = new Vector3 (
                startPos.x,
                startPos.y+.125f+yOff/4,
                startPos.z
            );
            // stages[currStage].transform.localScale = new Vector3 (
            //     1f+yOff/2,
            //     1f+yOff/2,
            //     1f+yOff/2
            // );
            scaleTime -= Time.deltaTime;
        } 
        else if (doneHarvest) {
            doneHarvest = false;
            audio.Play();
            reset();
            gridSquare.status = GridSquare.Status.HasSoil;
        } 
        else {
            timer -= Time.deltaTime * sun.timeMult;
        // if (xp >= 0) {
            if (timer < 0 || watered == true) {
                if (!watered) {
                    saturation -= .01f;
                    if (isFlower) {
                        saturation -= .005f;
                    }
                } else {
                    watered = false;
                }
                
                int zone = 1; // 1-good, 2-neutral, 3-bad
                if (saturation >= idealSaturation - satTolerance && saturation <= idealSaturation + satTolerance) {
                    xp += 4f;
                    zone = 0;
                } else if (saturation < minTol || saturation > maxTol) {
                    xp -= 1f;
                    zone = 2;
                }
                statusObj.GetComponent<MeshRenderer>().material = zoneMaterials[zone];
                if (xp >= goalXp) {
                    if (currStage < stages.Length-1) {
                        // stages[currStage].SetActive(false);
                        startPos = stages[currStage].transform.position;
                        stages[currStage].transform.localScale = new Vector3(1f,1f,1f);
                        currStage += 1;
                        fullScale = stages[currStage].transform.localScale;
                        stages[currStage].SetActive(true);
                        xp = 0f;
                    } else {
                        canHarvest = true;
                        stages[0].SetActive(false);
                        startPos = stages[currStage].transform.position;
                        if (isFlower && !finalObj.activeSelf) {
                            finalObj.SetActive(true);
                        }
                        //xp = 100f;
                        xp = 100f;
                    }
                }
                if (saturation < 0) {
                    saturation = 0f;
                }
                if (xp < 0) {
                    xp = 0f;
                }

                float scaleMult = 1f*(xp/goalXp);
                Vector3 newScale = new Vector3(scaleMult, scaleMult, scaleMult);
                stages[currStage].transform.localScale = newScale;
                meter.setSaturation(saturation, zone);
                timer = stageTime;
            }
        // }
        }
        
    }

    public int harvestPlant() {
        pullTime = .5f;
        bounceTime = .25f;
        scaleTime = .5f;
        doneHarvest = true;
        print("hi");
        return value;
    }
    public void water() {
        saturation += .1f;
        if (saturation > 1) {
            saturation = 1f;
        }
        watered = true;
    }


    public void hover(int state) {
        numHover += state;
    }

    public void init() {
        saturation = .2f;
        xp = 0f;
        timer = stageTime;
        currStage = 0;
        fullScale = stages[currStage].transform.localScale;  
        stages[currStage].transform.localScale = new Vector3(0f, 0f, 0f);
        stages[currStage].SetActive(true);
        numHover = 0;
        watered = true;
        canHarvest = false;
        print("int");
        pullTime = 0f;
        bounceTime = 0f;
        scaleTime = 0f;
        startPos = stages[currStage].transform.position;
        doneHarvest = false;
        finalObj.SetActive(false);
    }

    public void reset() {
        print("re");
        stages[currStage].transform.localScale = fullScale;
        stages[currStage].transform.position = startPos;
        foreach (GameObject stage in stages) {
            stage.SetActive(false);
        }
        currStage = 0;
        canHarvest = false;
        finalObj.SetActive(false);
    }

    public void showStatus(bool status) {
        if (status != statusObj.activeSelf) {
            statusObj.SetActive(status);
        }
    }

    public void getInfo() {
        // print("____________");
        // print("Sat", saturation, idealSaturation);
        // print("XP ", xp, goalXp);
    }

}
