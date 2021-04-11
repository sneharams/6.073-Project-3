using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ArmsTransform : MonoBehaviour
{
    public float moveSpeed; // Input speed of leg (must be same as Player move speed)
    public float legRight;  // Input for which leg goes forward first (-1 = left leg forward)
    private float turnTime; // Frames remaining for leg rotation in 1 half-rotation frame
    private float repoTime; // Frames remaining for leg reposition before leg turn animation
    private int turnFrames = 36;    // Length of half-rotation of leg in frames
    private int repoFrames = 60;    // Length of reposition rotation in frames
    private int frameNum;       // Current half-rotation frame (6 frames)

    private float turnStart;    // Start angle of leg rotation
    private float turnEnd;      // End angle of leg rotation
    private float repoStart;    // Start angle of reposition rotation
    private float repoEnd;      // End and of reposition rotation
    private float repoAngle;    // Angle of leg reposition when player turns
    private float offsetY;      // Vertical offset for leg rotations
    private float offsetZ;      // Horizontal offset for leg rotations
    private Boolean isX;        // If player is walking along x-axis
    private float reverse;   // player is walking direction sign (-1 = negative)
    private float direction;    // Current leg that goes forward first (-1 = left leg forward)

    
    void Start()
    {
        frameNum = 0;

        turnTime = -1;
        repoTime = -1;

        turnStart = 0;
        turnEnd = turnStart;
        repoStart = 180;
        repoEnd = repoStart;

        offsetY = 40f;
        offsetZ = -30f;

        isX = true;

        repoAngle = 180;
        direction = legRight;
        reverse = 1;
    }

    // Update is called once per frame
    void Update() 
    {
        // Start animation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) 
        {
            if (turnTime < 0 && repoTime < 0 && frameNum <= 0) 
            {
                if (Input.GetKey(KeyCode.W)) 
                {
                    // print("FORWARD");
                    repoStart = repoEnd;
                    repoEnd = 180;
                    isX = true;
                    reverse = 1;
                }
                else if (Input.GetKey(KeyCode.S)) 
                {
                    // print("BACK");
                    repoStart = repoEnd;
                    repoEnd = 0;
                    isX = true;
                    reverse = -1;
                }
                else if (Input.GetKey(KeyCode.D)) 
                {
                    // print("RIGHT");
                    repoStart = repoEnd;
                    repoEnd = -90;
                    isX = false;
                    reverse = 1;
                }
                else if (Input.GetKey(KeyCode.A)) 
                {
                    // print("LEFT");
                    repoStart = repoEnd;
                    repoEnd = 90;
                    isX = false;
                    reverse = -1;
                }
                direction = -direction;
                frameNum = 1;
                // Turn
                turnTime = turnFrames;
                turnStart = turnEnd;
                turnEnd = 20 * direction * reverse;
                // Reposition
                repoTime = -1;
                if (repoStart != repoEnd) 
                {
                    repoTime = repoFrames;
                }
                // Translation Offset
                offsetY = 40f * direction * reverse;
                offsetZ = -20f * direction;
            }
        }

        // Continue animation
        if (turnTime < 0 && frameNum > 0) {
            turnTime = turnFrames;
            turnStart = turnEnd;
            if (frameNum == 1) {        // middle
                // print("1");
                frameNum = 2;
                turnEnd = 0;
                offsetY = -40f;
                offsetZ = 20f;
            } else if (frameNum == 2) { // front
                // print("2");
                frameNum = 3;
                turnEnd = -20 * direction;
                offsetY = -40f;
                offsetZ = 20f;
            } else if (frameNum == 3) { // middle
                // print("3");
                frameNum = 4;
                turnEnd = 0;
                offsetY = 40f;
                offsetZ = -20f;
            } else if (frameNum == 4) { // back
                // print("4");
                frameNum = 5;
                turnEnd = 20 * direction;
                offsetY = 40f;
                offsetZ = -20f;
            } else if (frameNum == 5) { // middle
                // print("5");
                frameNum = 0;
                turnEnd = 0;
                offsetY = -40f;
                offsetZ = 20f;
            }
            turnEnd *= reverse;
            offsetY = offsetY * direction * reverse;
            // print(offsetY);
            offsetZ = offsetZ * direction;
        }

        if (turnTime >= 0) {
            movePlayer();
        }
        
    }

    private void print(String text) {
        if (legRight == 1) {
            // Debug.Log(text);
        }
    }

    private void movePlayer() 
    {
        if (repoTime >= 0) 
        {
            float range = repoEnd-repoStart;
            if (range > 180) {
                range = -90;
            } else if (range < -180) {
                range = 90;
            }
            repoAngle = repoStart + range/repoFrames * (repoFrames-repoTime);
            transform.rotation = Quaternion.Euler(0, repoAngle , 0);
            repoTime -= moveSpeed;
        } 
        else 
        {
            float range = turnEnd-turnStart;
            if (range > 180) {
                range = -20;
            } else if (range < -180) {
                range = 20;
            }
            float turnAngle = turnStart + range/turnFrames * (turnFrames-turnTime);
            print("" + ((offsetY * moveSpeed) / (turnFrames*10)));
            Vector3 moveDelta = new Vector3
            (
                0f, 
                (offsetY * moveSpeed) / (turnFrames*10), 
                (offsetZ * moveSpeed) / (turnFrames*10)
            );
            if (!isX) {
                moveDelta = new Vector3 
                (
                    (offsetZ * moveSpeed) / (turnFrames*10), 
                    (offsetY * moveSpeed) / (turnFrames*10), 
                    0f 
                );
                
            } 
            transform.rotation = Quaternion.Euler(turnAngle, repoAngle, 10f);            
            turnTime -= moveSpeed;
            transform.position = transform.position - moveDelta/3;
            turnTime -= moveSpeed;
            // if (turnTime < 0) {
            //     print("leg end");
            // }
        }
    }


}
