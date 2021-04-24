using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LeverTransform : MonoBehaviour
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

    public GameObject leverPivot;
    public GameObject rodPivot;

    public GameObject waterContainer;
    public GameObject water;

    private float pumpTime; // Frames remaining for leg rotation in 1 half-rotation frame
    private static int pumpFrames = 20;    // Length of half-rotation of lin frames
    private static int numFrames = 6;
    private int frameNum;       // Current half-rotation frame (6 frames)

    private float direction;
    private Vector3 waterPos;
    private Vector3 waterScale;

    
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
        waterPos = waterContainer.transform.position;
        waterScale = water.transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        // Start animation
        if (pumpTime <= 0) {
            if (frameNum <= -1) {
                waterContainer.transform.position = waterPos;
                water.transform.localScale = waterScale;
                if (Input.GetKey(KeyCode.P)) {
                    frameNum = 0;
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
            }
        }
        if (pumpTime > 0) {
            turnLever();
        }
    }

    private void turnLever() {
        float scaleMult = 80f;
        float leverMult = 1f;
        float posEnd = -.5f;
        if (Math.Abs(frameNum%2)==1) { // Lever Down
            leverMult = .5f;
            if (frameNum != 1) {
                Vector3 posNew = new Vector3(
                    waterContainer.transform.position.x, 
                    posEnd - Math.Abs(posEnd-waterPos.y)*((pumpFrames-pumpTime)/pumpFrames)*1, 
                    waterContainer.transform.position.z
                );
                waterContainer.transform.position = posNew;
            }
            if (pumpTime-pumpSpeed*leverMult < 0) {
                water.transform.localScale = waterScale;
                waterContainer.transform.position = waterPos;
            }
        }
        else { // Lever Up
            Vector3 posNew = new Vector3(
                waterContainer.transform.position.x, 
                waterPos.y - Math.Abs(posEnd-waterPos.y)*((pumpFrames-pumpTime)/pumpFrames), 
                waterContainer.transform.position.z
            );
            waterContainer.transform.position = posNew;
            Vector3 scale = new Vector3 (
                waterScale.x,
                waterScale.y+(waterScale.y*scaleMult * (pumpFrames-pumpTime)/pumpFrames),
                waterScale.z
            );
            water.transform.localScale = scale;
        }
        if (frameNum >= 0) {
            leverPivot.transform.rotation *= Quaternion.AngleAxis(90f*leverMult*direction/(pumpFrames), Vector3.forward);
            rodPivot.transform.rotation *= Quaternion.AngleAxis(-85f*leverMult*direction/(pumpFrames), Vector3.forward);
        }
        pumpTime -= pumpSpeed*leverMult;
    }


}
