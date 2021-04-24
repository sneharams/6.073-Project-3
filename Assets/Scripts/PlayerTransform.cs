using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


public class PlayerTransform : MonoBehaviour
{

    // [System.Serializable]
    // public enum MoveSpeed {
    //     speed_1x,
    //     speed_2x,
    //     speed_4x
    // };

    // // PUBLIC VARIABLES
    // public MoveSpeed speed;
    // [Tooltip("Must be divisible by move speed")] public int turnFrames;


    // // POSITIONING
    // [HideInInspector] public float moveSpeed;
    // [HideInInspector] public int clickX;
    // [HideInInspector] public int clickY;
    // private int xPos;
    // private int yPos;
    
    // // TIME VARIABLES (FRAMES)
    // private static int keyFrames = 6;    // number of segments for animation
    // private float moveKFDelta;
    // private float moveFrames;
    // private float moveTime;
    // private float turnTime;

    // // PLAYER ROTATION ANGLES
    // private float turnStart;
    // [HideInInspector]
    // public float turnEnd;

    // // PLAYER TRANSLATION CONSTANTS
    // private static float bounceDelta = -2f; // player height displacement for steps
    // private float bounceDirection; // keyFrame directional switch
    // private static float bounceDeltaKF = bounceDelta * keyFrames; // adjust for keyFrames
    // private static float moveDelta = 40f;  // horizontal player displacement
    // private static Vector3 forward = new Vector3( 0f, 0f, -moveDelta);
    // private static Vector3 back    = new Vector3( 0f, 0f,  moveDelta);
    // private static Vector3 left    = new Vector3( moveDelta, 0f,  0f);
    // private static Vector3 right   = new Vector3(-moveDelta, 0f,  0f);
    // private Vector3 moveVector; // holds current translation vector

    // // private CameraTransform camera;




    // // Start is called before the first frame update
    // void Start()
    // {
    //     // camera = GameObject.Find("Player Camera").GetComponent<CameraTransform>();
    //     moveFrames = 64+(keyFrames*moveSpeed);
    //     moveKFDelta = moveFrames/keyFrames; // horizontal player displacement for a keyFrame
    //     switch (speed) {
    //         case MoveSpeed.speed_1x:
    //             moveSpeed = 1;
    //             break;
    //         case MoveSpeed.speed_2x:
    //             moveSpeed = 2;
    //             break;
    //         case MoveSpeed.speed_4x:
    //             moveSpeed = 4;
    //             break;
    //     }
    //     moveVector = forward;
    //     moveTime = 0;
    //     turnTime = -1;
    //     turnStart = 180;
    //     turnEnd = 180;
    //     bounceDirection = bounceDeltaKF;
    //     xPos = 1;
    //     yPos = 1;
    //     clickX = 1;
    //     clickY = 1;
    //     // print(moveFKDelta);
    // }

    // // Update is called once per frame
    // void Update() {
    //     switch (speed) {
    //         case MoveSpeed.speed_1x:
    //             moveSpeed = 1;
    //             break;
    //         case MoveSpeed.speed_2x:
    //             moveSpeed = 2;
    //             break;
    //         case MoveSpeed.speed_4x:
    //             moveSpeed = 4;
    //             break;
    //     }
    //     if (moveTime <= 0) { 
    //         moveFrames = 64+(keyFrames*moveSpeed);
    //         moveKFDelta = moveFrames/keyFrames; // horizontal player displacement for a keyFrame
    //         if (clickX != xPos) {
    //             turnStart = turnEnd;
    //             if (clickX < xPos) {
    //                 turnEnd = 90;
    //                 moveVector = left;
    //                 xPos -= 1;
    //             } else {
    //                 turnEnd = -90;
    //                 moveVector = right;
    //                 xPos += 1;
    //             }
    //             moveTime = Math.Abs(moveFrames);
    //             turnTime = -1;
    //             if (turnStart != turnEnd) {
    //                 turnTime = turnFrames;
    //             } else {
    //                 GameObject.Find("Left Arm").GetComponent<ArmsTransform>().move(turnEnd, 120);
    //             }
    //         } 
    //         else if (clickY != yPos) {
    //             turnStart = turnEnd;
    //             if (clickY < yPos) {
    //                 turnEnd = 0;
    //                 moveVector = back;
    //                 yPos -= 1;
    //             } else {
    //                 turnEnd = 180;
    //                 moveVector = forward;
    //                 yPos += 1;
    //             }
    //             moveTime = Math.Abs(moveFrames);
    //             turnTime = -1;
    //             if (turnStart != turnEnd) {
    //                 turnTime = turnFrames;
    //             } else {
    //                 GameObject.Find("Left Arm").GetComponent<ArmsTransform>().move(turnEnd, 120);
    //             }
    //         } 
    //         else if (Input.GetKey(KeyCode.W)) {
    //             turnStart = turnEnd;
    //             turnEnd = 180;
    //             turnTime = turnFrames;
    //         }
    //         else if (Input.GetKey(KeyCode.S)) {
    //             turnStart = turnEnd;
    //             turnEnd = 0;
    //             turnTime = turnFrames;
    //         }
    //         else if (Input.GetKey(KeyCode.D)) {
    //             turnStart = turnEnd;
    //             turnEnd = -90;
    //             turnTime = turnFrames;
    //         }
    //         else if (Input.GetKey(KeyCode.A)) {
    //             turnStart = turnEnd;
    //             turnEnd = 90;
    //             turnTime = turnFrames;
    //         }
    //     }

    //     // update to be less dumb
    //     if (moveTime > 0 || turnTime >= 0) {
    //         float yOff = bounceDirection;
    //         // print(moveTime);
    //         if (moveTime <= moveKFDelta) {
    //             // print(1);
    //             yOff = -bounceDirection;
    //         } else if (moveTime <= moveKFDelta*2) {
    //             // print(2);
    //             yOff = bounceDirection;
    //         } else if (moveTime <= moveKFDelta*3) {
    //             // print(3);
    //             yOff = -bounceDirection;
    //         } else if (moveTime <= moveKFDelta*4) {
    //             // print(4);
    //             yOff = bounceDirection;
    //         } else if (moveTime <= moveKFDelta*5) {
    //             // print(5);
    //             yOff = -bounceDirection;
    //         }
            
    //         movePlayer(yOff);
    //     }
    // }

    // private void movePlayer(float yOff) {
    //     if (turnTime >= 0) {
    //         float range = turnEnd-turnStart;
    //         if (range > 180) {
    //             range = -90;
    //         } else if (range < -180) {
    //             range = 90;
    //         }
    //         float turnAngle = turnStart + range/turnFrames * (turnFrames-turnTime);
    //         transform.rotation = Quaternion.Euler(0, turnAngle , 0);
    //         turnTime -= moveSpeed;
    //     } else {
    //         // if (moveTime < moveSpeed){
    //         //     Vector3 moveVectorKF = new Vector3(moveVector.x, yOff, moveVector.z);
    //         //     Vector3 moveDelta = (moveVectorKF * moveTime) / (moveFrames * 10);
    //         //     transform.position = transform.position + moveDelta;
    //         //     moveTime = 0;
    //         // } else {
    //             Vector3 moveVectorKF = new Vector3(moveVector.x, yOff, moveVector.z);
    //             Vector3 moveDelta = (moveVectorKF * moveSpeed) / (moveFrames * 10);
    //             transform.position = transform.position + moveDelta;
    //             // camera.move(moveDelta);
    //             moveTime -= moveSpeed;
    //         // }
    //     }
    // }

    // private void print(String text) {
    //     // Debug.Log(text);
    // }


}
