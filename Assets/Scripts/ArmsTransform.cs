using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ArmsTransform : MonoBehaviour
{
    // public float isLeft;  // Input for which leg goes forward first (-1 = left leg forward)
    // private float turnTime; // Frames remaining for leg rotation in 1 half-rotation frame
    // private float repoTime; // Frames remaining for leg reposition before leg turn animation
    // private int turnFrames = 40;    // Length of half-rotation of leg in frames
    // private int repoFrames;    // Length of reposition rotation in frames
    // private float rangeAngle = 10f;
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

    // private void print(String text) {
    //     // if (legRight == 1) {
    //         Debug.Log(text);
    //     // }
    // }

    // private void print(float text) {
    //     // if (legRight == 1) {
    //         Debug.Log(text);
    //     // }
    // }

    // private void print(int text) {
    //     // if (legRight == 1) {
    //         Debug.Log(text);
    //     // }
    // }
    // void Start()
    // {
    //     frameNum = -1;

    //     turnTime = -1;
    //     repoTime = -1;

    //     turnStart = (isLeft+1)*90;
    //     turnEnd = turnStart;
    //     repoStart = 180;
    //     repoEnd = repoStart;

    //     offsetY = 40f;
    //     offsetZ = -30f;

    //     isX = true;

    //     repoAngle = 180;//-180*isLeft;
    //     direction = -1;
    //     reverse = 1;
    //     repoFrames = GameObject.Find("Player Controller").GetComponent<PlayerController>().turnFrames;
    //     moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerController>().moveSpeed;

    // }

    // // Update is called once per frame
    // void Update() 
    // {
    //     // float yAdj = 8f;
    //     float yOff = 8f;
    //     float zOff = 4f;
    //     // Start animation
    //     if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) 
    //     {
    //         if (turnTime < 0 && repoTime < 0 && frameNum < 0) 
    //         {
    //             moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerController>().moveSpeed;
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
    //                 // GameObject.Find("Player Controller").GetComponent<PlayerTransform>().turnEnd = repoEnd;
    //             }
    //         }
    //     }

    //     // Continue animation
    //     if (frameNum >= 0) {
    //         moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerController>().moveSpeed;
    //         turnStart = turnEnd;
    //         if (frameNum == 0) { 
    //             // print("frame0- " +transform.position);
    //             frameNum = 1;
    //             turnEnd = ((isLeft+1)*(90-rangeAngle)+rangeAngle) * direction;
    //             offsetY = -yOff*isLeft;
    //             offsetZ = zOff*isLeft;
    //         } else if (frameNum == 1) { // middle
    //             frameNum = 2;
    //             turnEnd = (isLeft+1)*90;
    //             offsetY = yOff*isLeft;
    //             offsetZ = -zOff*isLeft;
    //         } else if (frameNum == 2) { // front
    //             frameNum = 3;
    //             turnEnd = ((isLeft+1)*(-90+rangeAngle)-rangeAngle) * direction;
    //             offsetY = yOff*isLeft;
    //             offsetZ = -zOff*isLeft;
    //         } else if (frameNum == 3) { // middle
    //             frameNum = 4;
    //             turnEnd = (isLeft+1)*90;
    //             offsetY = -yOff*isLeft;
    //             offsetZ = zOff*isLeft;
    //         } else if (frameNum == 4) { // back
    //             frameNum = 5;
    //             turnEnd = ((isLeft+1)*(90-rangeAngle)+rangeAngle) * direction;
    //             offsetY = -yOff*isLeft;
    //             offsetZ = zOff*isLeft;
    //         } else if (frameNum == 5) { // middle
    //             frameNum = -1;
    //             turnEnd = (isLeft+1)*90;
    //             offsetY = yOff*isLeft;
    //             offsetZ = -zOff*isLeft;
    //         }
    //         print(frameNum);
    //         turnEnd *= reverse;
    //         offsetY = offsetY * direction * reverse;
    //         offsetZ = offsetZ * direction;
    //     }

    //     if (turnTime >= 0) {
    //         print(turnTime);
    //         movePlayer();
    //     }
        
    // }

    // public void move(float rEnd, float tFrames) {
    //     print("move" + turnTime);
    //     repoStart = repoEnd;
    //     repoEnd = rEnd;
    //     print("repoEnd: "+repoEnd);
    //     if (Math.Abs(repoEnd) == 90) {
    //         isX = false;
    //         if (repoEnd == 90){
    //             reverse = -1;
    //         } else {
    //             reverse = 1;
    //         }
    //     }
    //     else {
    //         isX = true;
    //         if (repoEnd == 0){
    //             reverse = -1;
    //         } else {
    //             reverse = 1;
    //         }
    //     }
    //     if (repoStart != repoEnd) 
    //     {
    //         float range = repoEnd-repoStart;
    //         if (range > 180) {
    //             range = -90;
    //         } else if (range < -180) {
    //             range = 90;
    //         }
    //         repoAngle = repoStart + range;
    //     }
    //     moveSpeed = GameObject.Find("Player Controller").GetComponent<PlayerController>().moveSpeed;
    //     turnTime = tFrames-moveSpeed*2;
    //     direction = -direction;
    //     frameNum = 0;
    // }



    // private void movePlayer() 
    // {
    //     if (turnTime >= 0) {
    //         float range = turnEnd-turnStart;
    //         if (range > 180) {
    //             range = -rangeAngle
    //             ;
    //         } else if (range < -180) {
    //             range = rangeAngle
    //             ;
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
    //         transform.rotation = Quaternion.Euler(turnAngle, repoAngle, 10f);    
    //         turnTime -= moveSpeed;
    //         transform.position = transform.position - moveDelta;
    //         turnTime -= moveSpeed;
    //     }
    // }


}
