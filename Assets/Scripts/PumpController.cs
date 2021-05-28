using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PumpController : MonoBehaviour
{
    #region Class Variables

    [System.Serializable] public enum PumpSpeed {
        Slow,
        Normal,
        Fast
    };

    // PUBLIC VARIABLES
    public PumpSpeed speed;
    private float pumpSpeed;
    public ParticleSystem water;
    public ParticleSystem water2;

    public GameObject leverPivot;
    public GameObject rodPivot;

    // public GameObject waterContainer;
    // public GameObject water;

    public GameObject pail;
    public GameObject waterLevel;
    public GameObject waterMeter;
    

    private float pumpTime; // Frames remaining for leg rotation in 1 half-rotation frame
    private static int pumpFrames = 40;    // Length of half-rotation of lin frames
    private static int numFrames = 8;
    private float pailTime;
    private static int pailFrames = 40;
    private int frameNum;       // Current half-rotation frame (6 frames)

    private float direction;
    // private Vector3 waterPos;
    private Vector3 waterScale;
    private Vector3 pailScale;
    private Vector3 levelScale;
    private Quaternion leverStart;
    private Quaternion rodStart;
    private float currLevel;
    private MeterContainer meter;
    private bool startPump;

    private AudioSource audio;

    
    #endregion

    #region Console Print Methods
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

    #endregion

    void Start()
    {
        if (speed == PumpSpeed.Slow) {
            pumpSpeed = 1f;
        } else if (speed == PumpSpeed.Normal) {
            pumpSpeed = 2f;
        } else {
            pumpSpeed = 4f;
        }
        frameNum = -3;
        pumpTime = -1;
        direction = 1;
        pailTime = -1f;
        //WATER POUR
        water.Pause();
        water2.Pause();
        audio = GetComponent<AudioSource>();


        // waterPos = waterContainer.transform.position;
        waterScale = waterLevel.transform.localScale;
        pailScale = pail.transform.localScale;
        levelScale = waterLevel.transform.localScale;
        leverStart = leverPivot.transform.rotation;
        rodStart = rodPivot.transform.rotation;
        currLevel = .01f;
        Vector3 levelStart = new Vector3(
            levelScale.x,
            .01f,
            levelScale.z
        );
        Vector3 pailStart = new Vector3(
            pailScale.x,
            0f,
            pailScale.z
        );
        // waterLevel.transform.localScale = levelStart;
        pail.transform.localScale = pailStart;
        pail.SetActive(false);
        waterLevel.SetActive(false);
        meter = waterMeter.GetComponent<MeterContainer>();
        startPump = false;
    }

    // Update is called once per frame
    void Update() {
        // Start animation
        if (pumpTime <= 0 && pailTime < 0) {
            if (frameNum <= -1) {
                // waterContainer.transform.position = waterPos;
                // water.transform.localScale = waterScale;
                if (startPump) {
                    frameNum = 0;
                    pailTime = pailFrames;
                    pail.SetActive(true);
                    startPump = false;
                }
            } 
            if (frameNum >= 0) {
                direction = -1;
                if (frameNum%2 == 0) {
                    direction = 1;
                }
                pumpTime = pumpFrames;
                frameNum += 1;
                if (frameNum > numFrames) {
                    frameNum = -1;
                }
                currLevel = waterLevel.transform.localScale.y;
            }
        }
        if (pumpTime > 0 || pailTime >= 0) {
            if (frameNum == 1 && !water.isPlaying && pailTime < 0) {
                Vector3 levelStart = new Vector3(
                    levelScale.x,
                    .01f,
                    levelScale.z
                );
                waterLevel.transform.localScale = levelStart;
                waterLevel.SetActive(true);
                water.Play();
                water2.Play();
                audio.Play();
                print("playering water");
            }
            // if (frameNum == -1 && pailTime >= 0 && water.isPlaying) {
            //     water.Pause();
            //     water2.Pause();
            // }
            turnLever();
        }
    }

    public void pump() {
        startPump = true;
    }

    private void turnLever() {
        float scaleMult = 80f;
        float leverMult = 1f;
        float posEnd = -.5f;
        float time = Time.deltaTime*100*pumpSpeed;
        if (pailTime < 0) {
            if (Math.Abs(frameNum%2)==1) { // Lever Down
                leverMult = .5f;
                if (frameNum != 1) {
                    // Vector3 posNew = new Vector3(
                    //     // waterContainer.transform.position.x, 
                    //     posEnd - Math.Abs(posEnd-waterPos.y)*((pumpFrames-pumpTime)/pumpFrames)*1, 
                    //     // waterContainer.transform.position.z
                    // );
                    // waterContainer.transform.position = posNew;
                    
                }
                if (pumpTime-time*leverMult <= 0) {
                    // waterLevel.transform.localScale = waterScale;
                    // waterContainer.transform.position = waterPos;
                    if (frameNum == -1) {
                        pailTime = pailFrames;
                        Vector3 pailStart = new Vector3(
                            pailScale.x,
                            0f,
                            pailScale.z
                        );
                        Vector3 levelStart = new Vector3(
                            levelScale.x,
                            0f,
                            levelScale.z
                        );
                        // waterLevel.transform.localScale = levelStart;
                        // pail.transform.localScale = pailStart;
                        // pail.SetActive(false);
                        // waterLevel.SetActive(false);
                    }
                }
            }
            else { // Lever Up
                // Vector3 posNew = new Vector3(
                //     waterContainer.transform.position.x, 
                //     waterPos.y - Math.Abs(posEnd-waterPos.y)*((pumpFrames-pumpTime)/pumpFrames), 
                //     waterContainer.transform.position.z
                // );
                // waterContainer.transform.position = posNew;
                // Vector3 levelStart = new Vector3(
                //     levelScale.x,
                //     .01f,
                //     levelScale.z
                // );
                // Vector3 scale = new Vector3 (
                //     waterScale.x,
                //     waterScale.y+(waterScale.y*scaleMult * (pumpFrames-pumpTime)/pumpFrames),
                //     waterScale.z
                // );
                // waterLevel.transform.localScale = scale;
                float levelRange = waterScale.y/(numFrames/2);
                float currFrame = (pumpFrames-pumpTime)/pumpFrames;
                Vector3 scale2 = new Vector3 (
                    levelScale.x,
                    currLevel + (levelRange/pumpFrames)*currFrame,//(levelScale.y/(numFrames/2)/pumpFrames)*(pumpFrames-pumpTime),
                    levelScale.z
                );
                waterLevel.transform.localScale = scale2;
                if (pumpTime-time*leverMult <= 0) {
                    meter.collect(pumpSpeed);
                }
            }
            if (frameNum >= 0) {
                leverPivot.transform.rotation *= Quaternion.AngleAxis(90f*leverMult*direction/(pumpFrames), Vector3.forward);
                rodPivot.transform.rotation *= Quaternion.AngleAxis(-85f*leverMult*direction/(pumpFrames), Vector3.forward);
            }
            pumpTime -= time*leverMult;
            if (pumpTime <= 0 && Math.Abs(frameNum%2)!=1) {
                leverPivot.transform.rotation = leverStart;
                rodPivot.transform.rotation = rodStart;
            }
        } else {
            Vector3 scale = new Vector3 (
                pailScale.x,
                (pailScale.y/pailFrames)*(pailFrames-pailTime),
                pailScale.z
            );
            if (frameNum == -1) {
                scale = new Vector3 (
                        pailScale.x,
                        pailScale.y - (pailScale.y/pailFrames)*(pailFrames-pailTime),
                        pailScale.z
                );
            } 
            pail.transform.localScale = scale;
            pailTime -= time;
            if (pailTime < 0) {
                Vector3 levelStart = new Vector3(
                    levelScale.x,
                    0.01f,
                    levelScale.z
                );
                if (frameNum == -1) {
                    Vector3 pailStart = new Vector3(
                        pailScale.x,
                        0f,
                        pailScale.z
                    );
                    // waterLevel.transform.localScale = levelStart;
                    pail.transform.localScale = pailStart;
                    pail.SetActive(false);
                } else {
                    pail.transform.localScale = pailScale;
                    // waterLevel.transform.localScale = levelStart;
                }
                
            }
        }
    }


}
