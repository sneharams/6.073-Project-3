using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class LegsTransform : MonoBehaviour
{
    // public float isLeft;  // Input for which leg goes forward first (-1 = left leg forward)
    // private float turnTime; // Frames remaining for leg rotation in 1 half-rotation frame
    // private float repoTime; // Frames remaining for leg reposition before leg turn animation
    // private int turnFrames = 40;    // Length of half-rotation of leg in frames
    // private int repoFrames;    // Length of reposition rotation in frames
    // private int frameNum;       // Current half-rotation frame (6 frames)

    // private float turnStart;    // Start angle of leg rotation
    // private float turnEnd;      // End angle of leg rotation
    // private float repoStart;    // Start angle of reposition rotation
    // private float repoEnd;      // End and of reposition rotation
    // private float repoAngle;    // Angle of leg reposition when player turns
    // private float offsetY;      // Vertical offset for leg rotations
    // private float offsetZ;      // Horizontal offset for leg rotations
    // private Boolean isX;        // If player is walking along x-axis
    // private float reverse;   // player is walking direction sign (-1 = negative)
    // private float direction;    // Current leg that goes forward first (-1 = left leg forward)
    // private float moveSpeed;


    // private void leftPrint(String text) {
    //     if (isLeft == 1) {
    //         Debug.Log(text);
    //     }
    // }

    // private void rightPrint(String text) {
    //     if (isLeft != 1) {
    //         Debug.Log(text);
    //     }
    // }

    // private void print(String text) {
    //     Debug.Log(text);
    // }
    // private void leftPrint(float text) {
    //     if (isLeft == 1) {
    //         Debug.Log("LEFT: " + text);
    //     }
    // }

    // private void rightPrint(float text) {
    //     if (isLeft != 1) {
    //         Debug.Log("RIGHT: " + text);
    //     }
    // }

    // private void print(float text) {
    //     if (isLeft == 1) {
    //         Debug.Log(text);
    //     }
    // }

    // private void leftPrint(int text) {
    //     if (isLeft == 1) {
    //         Debug.Log(text);
    // }

    // }
    // private void print(int text) {
    //     if (isLeft == 1) {
    //         Debug.Log(text);
    //     }
    // }
    // void Start()
    // {
    //     frameNum = -1;

    //     turnTime = -1;
    //     repoTime = -1;

    //     turnStart = 0;//(isLeft+1)*90;
    //     turnEnd = turnStart;
    //     repoStart = 180;
    //     repoEnd = repoStart;

    //     offsetY = 40f;
    //     offsetZ = -30f;

    //     isX = true;

    //     repoAngle = 180;//-180*isLeft;
    //     direction = isLeft;
    //     reverse = 1;

    //     repoFrames = GameObject.Find("Player Controller").GetComponent<PlayerTransform>().turnFrames;
    //     moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerTransform>().moveSpeed;

    // }

    // // Update is called once per frame
    // void Update() 
    // {
    //     // float yAdj = 8f;
    //     float yOff = 16f;
    //     float zOff = -8f;
    //     // Start animation
    //     if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) 
    //     {
    //         if (turnTime < 0 && repoTime < 0 && frameNum < 0) 
    //         {
    //             repoFrames = GameObject.Find("Player Controller").GetComponent<PlayerTransform>().turnFrames;
    //             moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerTransform>().moveSpeed;
    //             repoStart = repoEnd;
    //             if (Input.GetKey(KeyCode.W)) 
    //             {
    //                 repoEnd = 180;
    //                 isX = true;
    //                 reverse = 1;
    //             }
    //             else if (Input.GetKey(KeyCode.S)) 
    //             {
    //                 repoEnd = 0;
    //                 isX = true;
    //                 reverse = -1;
    //             }
    //             else if (Input.GetKey(KeyCode.D)) 
    //             {
    //                 repoEnd = -90;
    //                 isX = false;
    //                 reverse = 1;
    //             }
    //             else if (Input.GetKey(KeyCode.A)) 
    //             {
    //                 repoEnd = 90;
    //                 isX = false;
    //                 reverse = -1;
    //             }
    //             direction = -direction;
    //             frameNum = 0;

    //             repoTime = -1;
    //             if (repoStart != repoEnd) 
    //             {
    //                 float range = repoEnd-repoStart;
    //                 if (range > 180) {
    //                     range = -90;
    //                 } else if (range < -180) {
    //                     range = 90;
    //                 }
    //                 repoAngle = repoStart + range;
    //                 repoTime = repoFrames;
    //             }
    //         }
    //     }

    //     // Continue animation
    //     if (turnTime < 0 && frameNum >= 0) {
    //         turnTime = turnFrames;
    //         turnStart = turnEnd;
    //         if (frameNum == 0) { 
    //             frameNum = 1;
    //             turnEnd = 20*direction;//((isLeft+1)*70+20) * direction;
    //             offsetY = yOff;
    //             offsetZ = zOff;//*isLeft;
    //         } else if (frameNum == 1) { // middle
    //             frameNum = 2;
    //             turnEnd = 0;//(isLeft+1)*90;
    //             offsetY = -yOff;
    //             offsetZ = -zOff;//*isLeft;
    //         } else if (frameNum == 2) { // front
    //             frameNum = 3;
    //             turnEnd = -20*direction;//((isLeft+1)*(-70)-20) * direction;
    //             offsetY = -yOff;
    //             offsetZ = -zOff;//*isLeft;
    //         } else if (frameNum == 3) { // middle
    //             frameNum = 4;
    //             turnEnd = 0;//(isLeft+1)*90;
    //             offsetY = yOff;
    //             offsetZ = zOff;//*isLeft;
    //         } else if (frameNum == 4) { // back
    //             frameNum = 5;
    //             turnEnd = 20*direction;//((isLeft+1)*70+20) * direction;
    //             offsetY = yOff;
    //             offsetZ = zOff;//*isLeft;
    //         } else if (frameNum == 5) { // middle
    //             frameNum = -1;
    //             turnEnd = 0;//(isLeft+1)*90;
    //             offsetY = -yOff;
    //             offsetZ = -zOff;//*isLeft;
    //         }
    //         turnEnd *= reverse;
    //         offsetY = offsetY * direction * reverse;
    //         offsetZ = offsetZ * direction;
    //     }

    //     if (turnTime >= 0) {
    //         // leftPrint(turnTime);
    //         // rightPrint(turnTime);
    //         movePlayer();
    //     }
        
    // }



    // private void movePlayer() 
    // {
    //     if (repoTime >= 0) 
    //     {
    //         // float range = repoEnd-repoStart;
    //         // if (range > 180) {
    //         //     range = -90;
    //         // } else if (range < -180) {
    //         //     range = 90;
    //         // }

    //         // repoAngle = repoStart + range/repoFrames * (repoFrames-repoTime);
    //         // transform.rotation = Quaternion.Euler(0, repoAngle , 10f);
    //         repoTime -= moveSpeed;
    //     } 
    //     else 
    //     {
    //         float range = turnEnd-turnStart;
    //         if (range > 180) {
    //             range = -20;
    //         } else if (range < -180) {
    //             range = 20;
    //         }

    //         float turnAngle = turnStart + range/turnFrames * (turnFrames-turnTime);
    //         Vector3 moveDelta = new Vector3
    //         (
    //             0f, 
    //             (offsetY*moveSpeed) / (turnFrames*10), 
    //             (offsetZ*moveSpeed) / (turnFrames*10)
    //         );
    //         if (!isX) {
    //             moveDelta = new Vector3 
    //             (
    //                 (offsetZ*moveSpeed) / (turnFrames*10), 
    //                 (offsetY*moveSpeed) / (turnFrames*10), 
    //                 0f 
    //             );
                
    //         } 
    //         // print(transform.position);
    //         transform.rotation = Quaternion.Euler(turnAngle, repoAngle, 0);    
    //         turnTime -= moveSpeed;
    //         transform.position = transform.position - moveDelta;
    //         turnTime -= moveSpeed;
    //     }
    // }


}

