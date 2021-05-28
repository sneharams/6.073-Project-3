using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SackController : MonoBehaviour
{
    #region Class Variables

    [System.Serializable] public enum ShovelSpeed {
        Slow,
        Normal,
        Fast
    };

    // PUBLIC VARIABLES
    public ShovelSpeed speed;
    private float shovelSpeed;

    // public GameObject leverPivot;
    // public GameObject rodPivot;

    // public GameObject waterContainer;
    // public GameObject water;

    // public GameObject pail;
    // public GameObject waterLevel;
    public GameObject soilMeter;
    public GameObject shovel;

    private float shovelTime; // Frames remaining for leg rotation in 1 half-rotation frame
    private static int shovelFrames = 40;    // Length of half-rotation of lin frames
    private static int numFrames = 8;
    // private float pailTime;
    // private static int pailFrames = 40;
    private int frameNum;       // Current half-rotation frame (6 frames)

    // private float direction;
    // private Vector3 waterPos;
    // private Vector3 waterScale;
    // private Vector3 pailScale;
    // private Vector3 levelScale;
    // private Quaternion leverStart;
    // private Quaternion rodStart;
    // private float currLevel;
    private MeterContainer meter;
    private bool startShovel;
    
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
        if (speed == ShovelSpeed.Slow) {
            shovelSpeed = 1f;
        } else if (speed == ShovelSpeed.Normal) {
            shovelSpeed = 2f;
        } else {
            shovelSpeed = 4f;
        }
        frameNum = -3;
        shovelTime = -1;
        // direction = 1;
        // pailTime = -1f;
        // waterPos = waterContainer.transform.position;
        // waterScale = water.transform.localScale;
        // pailScale = pail.transform.localScale;
        // levelScale = waterLevel.transform.localScale;
        // leverStart = leverPivot.transform.rotation;
        // rodStart = rodPivot.transform.rotation;
        // currLevel = 0f;
        // Vector3 levelStart = new Vector3(
        //     levelScale.x,
        //     0f,
        //     levelScale.z
        // );
        // Vector3 pailStart = new Vector3(
        //     pailScale.x,
        //     0f,
        //     pailScale.z
        // );
        // waterLevel.transform.localScale = levelStart;
        // pail.transform.localScale = pailStart;
        // pail.SetActive(false);
        meter = soilMeter.GetComponent<MeterContainer>();
        startShovel = false;
    }

    // Update is called once per frame
    void Update() {
        // Start animation
        if (shovelTime <= 0) { // && pailTime < 0) {
            if (frameNum <= -1) {
                // waterContainer.transform.position = waterPos;
                // water.transform.localScale = waterScale;
                if (startShovel) {
                    frameNum = 0;
                    // pailTime = pailFrames;
                    // pail.SetActive(true);
                    startShovel = false;
                }
            } 
            if (frameNum >= 0) {
                // direction = -1;
                // if (frameNum%2 == 0) {
                //     direction = 1;
                // }
                shovelTime = shovelFrames;
                frameNum += 1;
                if (frameNum > numFrames) {
                    frameNum = -1;
                }
                // currLevel = waterLevel.transform.localScale.y;
            }
        }
        if (shovelTime > 0) { // || pailTime >= 0) {
            if (!shovel.activeSelf) {
                shovel.SetActive(true);
            }
            scoopDirt();
        } else {
            shovel.SetActive(false);
        }
    }

    public void shovelMethod() {
        startShovel = true;
    }

    private void scoopDirt() {
        // float scaleMult = 80f;
        // float leverMult = 1f;
        // float posEnd = -.5f;
        float time = Time.deltaTime*100*shovelSpeed
;
        // if (pailTime < 0) {
            if (Math.Abs(frameNum%2)==1) { // Lever Down
                // leverMult = .5f;
                // if (frameNum != 1) {
                //     Vector3 posNew = new Vector3(
                //         waterContainer.transform.position.x, 
                //         posEnd - Math.Abs(posEnd-waterPos.y)*((shovelFrames-shovelTime)/shovelFrames)*1, 
                //         waterContainer.transform.position.z
                //     );
                //     waterContainer.transform.position = posNew;
                    
                // }
                // if (shovelTime-time*leverMult <= 0) {
                //     water.transform.localScale = waterScale;
                //     waterContainer.transform.position = waterPos;
                //     if (frameNum == -1) {
                //         pailTime = pailFrames;
                //         Vector3 pailStart = new Vector3(
                //             pailScale.x,
                //             0f,
                //             pailScale.z
                //         );
                //         Vector3 levelStart = new Vector3(
                //             levelScale.x,
                //             0f,
                //             levelScale.z
                //         );
                //         waterLevel.transform.localScale = levelStart;
                //         pail.transform.localScale = pailStart;
                //         pail.SetActive(false);
                //     }
                // }
            }
            else { // Lever Up
                // Vector3 posNew = new Vector3(
                //     waterContainer.transform.position.x, 
                //     waterPos.y - Math.Abs(posEnd-waterPos.y)*((shovelFrames-shovelTime)/shovelFrames), 
                //     waterContainer.transform.position.z
                // );
                // waterContainer.transform.position = posNew;
                // Vector3 scale = new Vector3 (
                //     waterScale.x,
                //     waterScale.y+(waterScale.y*scaleMult * (shovelFrames-shovelTime)/shovelFrames),
                //     waterScale.z
                // );
                // water.transform.localScale = scale;
                // Vector3 scale2 = new Vector3 (
                //     levelScale.x,
                //     currLevel + (levelScale.y/(numFrames/2)/shovelFrames)*(shovelFrames-shovelTime),
                //     levelScale.z
                // );
                // waterLevel.transform.localScale = scale2;
                if (shovelTime-time <= 0) {
                    meter.collect(shovelSpeed);
                }
            }
            // if (frameNum >= 0) {
            //     leverPivot.transform.rotation *= Quaternion.AngleAxis(90f*leverMult*direction/(shovelFrames), Vector3.forward);
            //     rodPivot.transform.rotation *= Quaternion.AngleAxis(-85f*leverMult*direction/(shovelFrames), Vector3.forward);
            // }
            shovelTime -= time;
            // if (shovelTime <= 0 && Math.Abs(frameNum%2)!=1) {
            //     leverPivot.transform.rotation = leverStart;
            //     rodPivot.transform.rotation = rodStart;
            // }
        // } else {
        //     Vector3 scale = new Vector3 (
        //         pailScale.x,
        //         (pailScale.y/pailFrames)*(pailFrames-pailTime),
        //         pailScale.z
        //     );
        //     if (frameNum == -1) {
        //         scale = new Vector3 (
        //                 pailScale.x,
        //                 pailScale.y - (pailScale.y/pailFrames)*(pailFrames-pailTime),
        //                 pailScale.z
        //         );
        //     } 
        //     pail.transform.localScale = scale;
        //     pailTime -= time;
        //     if (pailTime < 0) {
        //         Vector3 levelStart = new Vector3(
        //             levelScale.x,
        //             0f,
        //             levelScale.z
        //         );
        //         if (frameNum == -1) {
        //             Vector3 pailStart = new Vector3(
        //                 pailScale.x,
        //                 0f,
        //                 pailScale.z
        //             );
        //             waterLevel.transform.localScale = levelStart;
        //             pail.transform.localScale = pailStart;
        //             pail.SetActive(false);
        //         } else {
        //             waterLevel.transform.localScale = levelStart;
        //             pail.transform.localScale = pailScale;
        //         }
                
        //     }
        // }
    }


}
